using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;


namespace Greta.BO.BusinessLogic.Handlers.Queries.Role;

/// <summary>
/// Query for get rol by id
/// </summary>
/// <param name="Id">id of rol</param>
public record RoleGetByIdQuery(long Id) : IRequest<RoleGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement(string.Format("view_{0}",nameof(Role).ToLower()))
    };

    /// <inheritdoc />
    public string CacheKey => string.Format("RoleGetById{0}", Id);
}

/// <inheritdoc />
public class RoleGetByIdHandler : IRequestHandler<RoleGetByIdQuery, RoleGetByIdResponse>
{    

    private readonly IMapper _mapper;

    private readonly IRoleService _service;

     
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoleGetByIdHandler(IRoleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RoleGetByIdResponse> Handle(RoleGetByIdQuery request, CancellationToken cancellationToken = default)
    {   
        var spec = new GetByIdSpec<Api.Entities.Role>(request.Id);
        var data = await _service.Get(spec, cancellationToken);
        return data == null ? null : new RoleGetByIdResponse { Data = _mapper.Map<RoleModel>(data) };        
    }
}

/// <inheritdoc />
public record RoleGetByIdResponse : CQRSResponse<RoleModel>;
