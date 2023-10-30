using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using System.Linq;
using System.Net;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.EFCore.Middleware;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Auth;

/// <summary>
///  This Command get the data for authenticate user  and call the identity service to get Base user data and return all this data to the user
/// </summary>
/// <param name="UserId"></param>
public record AuthUserInfoQuery(string UserId) : IRequest<AuthUserInfoResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new OnlyConnectedUserRequirementRequirement(UserId)
    };
}

/// <inheritdoc />
public class AuthUserInfoHandler : IRequestHandler<AuthUserInfoQuery, AuthUserInfoResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IBOUserService _service;
    private readonly IAuthenticateUser<string> _authUser;
    private readonly IPermissionService _permissionService;
    private readonly IProfilesService _profilesService;
    private readonly IConfiguration _configuration;
    private readonly IRequestClient<UserGetInfoRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authUser"></param>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="profilesService"></param>
    /// <param name="permissionService"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    /// <param name="client"></param>
    public AuthUserInfoHandler(IAuthenticateUser<string> authUser,
        ILogger<AuthUserInfoHandler> logger,
        IBOUserService service,
        IProfilesService profilesService,
        IPermissionService permissionService,
        IMapper mapper,
        IConfiguration configuration,
        IRequestClient<UserGetInfoRequestContract> client)
    {
        this._authUser = authUser;
        _logger = logger;
        _service = service;
        this._profilesService = profilesService;
        this._permissionService = permissionService;
        _mapper = mapper;
        _configuration = configuration;
        _client = client;
    }

    /// <inheritdoc />
    public async Task<AuthUserInfoResponse> Handle(AuthUserInfoQuery request,
        CancellationToken cancellationToken = default)
    {
        //1- primero obtener el perfil de datos de identity
        //2- si hay un perfil de datos buscar el perfil local
        //3 si no hay un perfil local asumo por ahora que es el usuario inicial
        // el que esta entrando, aqui hay dos variantes una generar ese perfil ahora
        // o que en el cleinte se ejecute un wizard de inicializacion o de bienvenida
        // donde el usuario debe poner informacion importante.  

        var userId = request.UserId;
        if (userId == null)
        {
            if (!_authUser.IsAuthenticated)
                return new AuthUserInfoResponse
                {
                    Errors = new List<string> { "You most by logged" },
                    StatusCode = HttpStatusCode.Unauthorized
                };
            if (_authUser.Scope != _configuration["Company:CompanyCode"])
                return new AuthUserInfoResponse
                {
                    Errors = new List<string> { "Access denied" },
                    StatusCode = HttpStatusCode.Unauthorized
                };
            userId = _authUser.UserId;
        }

        var result = new AuthUserInfoResponse();
        //Call to Identity Service
        var remoteuser = await _client.GetResponse<UserGetInfoResponseContract>(new
        {
            UserId = userId
        });

        result.GlobalUser = remoteuser.Message;

        //get Local User information (if null then the client need generate a wellcome wizard)
        var localData = await _service.GetByUserId(userId);

        if (localData != null && localData.Expire != null)
        {
            if (localData.Expire.Value < System.DateTime.Now)
            {
                //Remove user
                await _service.Delete(localData.Id);
                return new AuthUserInfoResponse
                {
                    Errors = new List<string> { "Your support user expire" },
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
        }

        if (localData != null)
        {
            if (localData.BOProfile == null)
            {
                return new AuthUserInfoResponse
                {
                    Errors = new List<string> { "Access denied" },
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            result.BOUser = _mapper.Map<BOUserDto>(localData);
        }
        else
        {
            var hasUser = await _service.Get();

            var isFirstUser = hasUser.Count == 0;

            if (isFirstUser)
            {
                result.NeedWizard = true;
                var boB = await _permissionService.GetByAplication(1);
                var boPos = await _permissionService.GetByAplication(2);

                var profileB = await _profilesService.Get(1);
                if (profileB.Permissions.Count == 0)
                {
                    profileB.Permissions = boB;
                    await _profilesService.Put(1, profileB);
                }

                var profileP = await _profilesService.Get(2);
                if (profileP.Permissions.Count == 0)
                {
                    profileP.Permissions = boPos;
                    await _profilesService.Put(2, profileP);
                }

                var value = new Api.Entities.BOUser
                {
                    UserId = userId,
                    BOProfileId = 1,
                    POSProfileId = 2,
                    UserName = result.GlobalUser.UserName,
                    Email = result.GlobalUser.Email,
                    RoleId = 1,
                    Expire = null
                };

                await _service.Post(value);
                localData = await _service.GetByUserId(userId);
                result.BOUser = _mapper.Map<BOUserDto>(localData);
                _logger.LogInformation("Initial user {UserName} created", localData.UserName);
            }
            else
            {
                var nL = remoteuser.Message.AllowedScopes;
                var isCorporateUser = nL != null &&
                                      nL.Any(x => x.Name == "corporate" && x.Vigency == null);
                var scope = !isCorporateUser
                    ? null
                    : nL.Where(x => x.Name == _configuration["Company:CompanyCode"] &&
                                    x.Vigency != null && x.Vigency.Value > System.DateTime.UtcNow)
                        .FirstOrDefault();
                if (isCorporateUser && scope != null)
                {
                    var value = new Api.Entities.BOUser
                    {
                        UserId = userId,
                        BOProfileId = 1,
                        POSProfileId = 2,
                        UserName = result.GlobalUser.UserName,
                        Email = result.GlobalUser.Email,
                        RoleId = 1,
                        Expire = scope.Vigency
                    };

                    await _service.Post(value);
                    localData = await _service.GetByUserId(userId);
                    result.BOUser = _mapper.Map<BOUserDto>(localData);
                    _logger.LogInformation("Corporate user {Username} created", localData.UserName);
                }
                else
                {
                    return new AuthUserInfoResponse
                    {
                        Errors = new List<string> { "Access denied." },
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }
            }
        }

        return result;
    }
}

/// <inheritdoc />
public record AuthUserInfoResponse : CQRSResponse
{
    public BOUserDto BOUser { get; set; }

    public UserGetInfoResponseContract GlobalUser { get; set; }

    public bool NeedWizard { get; set; }
}

/// <inheritdoc />
public class BOUserDto : IMapFrom<Api.Entities.BOUser>
{
    public long RoleId { get; set; }

    public RoleModel Role { get; set; }

    public long BOProfileId { get; set; }
    public ProfilesModel BOProfile { get; set; }
    public long POSProfileId { get; set; }
    public ProfilesModel POSProfile { get; set; }
}