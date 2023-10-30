using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.Corporate.Api.EventContracts.Store;
using Greta.Sdk.MassTransit.Contracts;

namespace Greta.BO.BusinessLogic.Handlers.Command.Store;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record StoreChangeStateCommand(long Id, bool State) : IRequest<StoreChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreChangeStateHandler : IRequestHandler<StoreChangeStateCommand, StoreChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IStoreService _service;
    protected readonly IAuthenticateUser<string> _authenticateUser;
    protected readonly IConfiguration _configuration;
    private readonly IRequestClient<ChangeStateStoreRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    /// <param name="client"></param>
    /// <param name="configuration"></param>
    /// <param name="authenticateUser"></param>
    public StoreChangeStateHandler(ILogger<StoreChangeStateHandler> logger, IStoreService service, IConfiguration configuration,
                IRequestClient<ChangeStateStoreRequestContract> client,
                IAuthenticateUser<string> authenticateUser, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
        _client = client;
        _configuration = configuration;
        _authenticateUser = authenticateUser;
    }

    /// <inheritdoc />
    public async Task<StoreChangeStateResponse> Handle(StoreChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            //update store on corporate
            var data = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
            {
                __Header_user = _authenticateUser.UserId,
                BO_ClientCode = _configuration["Company:CompanyCode"],
                request.State,
                GuidId = (await _service.Get(request.Id)).GuidId
            });

            if (data.Is(out Response<FailResponseContract> response))
            {
                _logger.LogError("Error update store in corporate");
                throw new BusinessLogicException(((List<string>)response.Message.ErrorMessages)[0]);
            }

            var result = await _service.ChangeState(request.Id, request.State);
            _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
            return new StoreChangeStateResponse { Data = result };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException("Error change state store " + e.Message + " " + e.InnerException?.Message);
        }
    }
}

/// <inheritdoc />
public record StoreChangeStateResponse : CQRSResponse<bool>;