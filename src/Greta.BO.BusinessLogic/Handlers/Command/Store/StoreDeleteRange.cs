using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Corporate.Api.EventContracts.Ticket;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Store;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record StoreDeleteRangeCommand(List<long> Ids) : IRequest<StoreDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreDeleteRangeHandler : IRequestHandler<StoreDeleteRangeCommand, StoreDeleteRangeResponse>
{
    private readonly IAuthenticateUser<string> _authenticateUser;
    private readonly ITicketService _ticketService;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IStoreService _service;
    private readonly IRequestClient<TicketStoreRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="client"></param>
    /// <param name="authenticateUser"></param>
    /// <param name="ticketService"></param>
    /// <param name="configuration"></param>
    /// <param name="mapper"></param>
    public StoreDeleteRangeHandler(
        ILogger<StoreDeleteRangeHandler> logger,
        IStoreService service,
        IRequestClient<TicketStoreRequestContract> client,
        IAuthenticateUser<string> authenticateUser,
        ITicketService ticketService,
        IConfiguration configuration,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
        _client = client;
        _configuration = configuration;
        _authenticateUser = authenticateUser;
        _ticketService = ticketService;
    }

    /// <inheritdoc />
    public async Task<StoreDeleteRangeResponse> Handle(StoreDeleteRangeCommand request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ticket = new Ticket
            {
                Status = TicketStatus.PENDIENT,
                Type = TicketType.DELETE_STORE,
                State = true,
                Data = string.Join(",", request.Ids),
                GuidId = Guid.NewGuid(),
                Code = $"{_configuration["Company:CompanyCode"]}_DeleteRangeRequest"
            };

            ticket = await _ticketService.Post(ticket);

            //update store on corporate
            var data = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
            {
                __Header_user = _authenticateUser.UserId,
                GuidId = ticket.GuidId,
                BO_ClientCode = _configuration["Company:CompanyCode"],
                Code = ticket.Code,
                Data = ticket.Data,
                Type = (int)ticket.Type
            }, cancellationToken);

            if (data.Is(out Response<FailResponseContract> response))
            {
                _logger.LogError("Error delete stores in corporate");
                throw new BusinessLogicException(((List<string>)response.Message.ErrorMessages)[0]);
            }

            var result = await _service.DeleteRange(request.Ids);
            _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
            return new StoreDeleteRangeResponse { Data = result };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException("Error delete stores " + e.Message + " " + e.InnerException?.Message);
        }
    }
}

/// <inheritdoc />
public record StoreDeleteRangeResponse : CQRSResponse<bool>;