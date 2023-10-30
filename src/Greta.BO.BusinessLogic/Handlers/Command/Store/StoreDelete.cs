using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Corporate.Api.EventContracts.Store;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Store;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record StoreDeleteCommand(long Id) : IRequest<StoreDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreDeleteHandler : IRequestHandler<StoreDeleteCommand, StoreDeleteResponse>
{
    protected readonly IAuthenticateUser<string> _authenticateUser;
    protected readonly IConfiguration _configuration;
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly IStoreService _service;
    private readonly IRequestClient<DeleteStoreRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="client"></param>
    /// <param name="authenticateUser"></param>
    /// <param name="configuration"></param>
    /// <param name="mapper"></param>
    public StoreDeleteHandler(
        ILogger<StoreDeleteHandler> logger,
                IStoreService service,
                IRequestClient<DeleteStoreRequestContract> client,
                IAuthenticateUser<string> authenticateUser,
                IConfiguration configuration,
                IMapper mapper)
    {
        _configuration = configuration;
        _logger = logger;
        _service = service;
        _mapper = mapper;
        _client = client;
        _authenticateUser = authenticateUser;
    }

    /// <inheritdoc />
    public async Task<StoreDeleteResponse> Handle(StoreDeleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //update store on corporate
            var data = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
            {
                __Header_user = _authenticateUser.UserId,
                BO_ClientCode = _configuration["Company:CompanyCode"],
                GuidId = (await _service.Get(request.Id)).GuidId
            });

            if (data.Is(out Response<FailResponseContract> response))
            {
                _logger.LogError("Error delete store in corporate");
                throw new BusinessLogicException(((List<string>)response.Message.ErrorMessages)[0]);
            }

            var result = await _service.Delete(request.Id);
            _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
            return new StoreDeleteResponse { Data = result };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException("Error delete stores "+ e.Message +" "+ e.InnerException?.Message);
        }
    }
}

/// <inheritdoc />
public record StoreDeleteResponse : CQRSResponse<bool>;