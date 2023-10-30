using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using Greta.BO.BusinessLogic.Exceptions;
using System.Collections.Generic;
using MassTransit;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.Extensions.Configuration;
using Greta.BO.Api.Entities.Enum;
using Newtonsoft.Json;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.Corporate.Api.EventContracts.Ticket;
using Greta.Sdk.MassTransit.Contracts;
using Greta.BO.BusinessLogic.Specifications.Generics;

namespace Greta.BO.BusinessLogic.Handlers.Command.Store;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record StoreCreateCommand(StoreModel Entity) : IRequest<StoreCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<StoreCreateCommand>
{
    private readonly IStoreService _service;

    /// <inheritdoc />
    public Validator(IStoreService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Store name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Store>(name), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class StoreCreateHandler : IRequestHandler<StoreCreateCommand, StoreCreateResponse>
{
    private readonly ILogger _logger;
    private readonly ITicketService _ticketService;
    private readonly IRegionService _regionService;
    private readonly IAuthenticateUser<string> _authenticateUser;
    private readonly IMapper _mapper;
    private readonly IRequestClient<TicketStoreRequestContract> _client;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="ticketService"></param>
    /// <param name="client"></param>
    /// <param name="regionService"></param>
    /// <param name="authenticateUser"></param>
    /// <param name="mapper"></param>
    public StoreCreateHandler(
        ILogger<StoreCreateHandler> logger,
                IConfiguration configuration,
                ITicketService ticketService,
                IRequestClient<TicketStoreRequestContract> client,
                IRegionService regionService,
                IAuthenticateUser<string> authenticateUser,
                IMapper mapper)
    {
        _configuration = configuration;
        _logger = logger;
        _ticketService = ticketService;
        _mapper = mapper;
        _client = client;
        _authenticateUser = authenticateUser;
        _regionService = regionService;
    }

    /// <inheritdoc />
    public async Task<StoreCreateResponse> Handle(StoreCreateCommand request, CancellationToken cancellationToken)
    {
        Ticket ticket = new Ticket();
        try
        {
            //create ticket in BO
            var store = this._mapper.Map<Api.Entities.Store>(request.Entity);
            //set default values
            store.Currency = "USD";
            store.Language = "en";
            store.CashDiscount = false;
            store.CashDiscountValue = 4;

            store.GuidId = Guid.NewGuid();
            store.Region = await _regionService.Get(store.RegionId);
            ticket = new Ticket
            {
                Status = TicketStatus.PENDIENT,
                Type = TicketType.CREATE_STORE,
                State = true,
                Data = JsonConvert.SerializeObject(store),
                GuidId = Guid.NewGuid(),
                Code = $"{_configuration["Company:CompanyCode"]}_{store.Name}"
            };

            ticket = await _ticketService.Post(ticket);

            //create ticket in Corporate
            var data = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
            {
                __Header_user = _authenticateUser.UserId,
                GuidId = ticket.GuidId,
                BO_ClientCode = _configuration["Company:CompanyCode"],
                Code = ticket.Code,
                Data = ticket.Data,
                Type = ticket.Type
            }, cancellationToken);

            if (data.Is(out Response<FailResponseContract> responseFail))
            {
                await this._ticketService.Delete(ticket.Id);
                this._logger.LogError("Error create ticket in corporate");
                throw new BusinessLogicException(((List<string>)responseFail.Message.ErrorMessages)[0]);
            }

            return new StoreCreateResponse() { Data = true };
        }
        catch (Exception e)
        {
            await this._ticketService.Delete(ticket.Id);
            throw new BusinessLogicException("Error create store "+ e.Message +" "+ e.InnerException?.Message);
        }
    }
}

/// <inheritdoc />
public record StoreCreateResponse : CQRSResponse<bool>;