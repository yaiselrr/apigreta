using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.GiftCardCommands;

public record ProcessCredictCardsCommand(List<GiftCard> giftcards) : INotification;

public class ProcessCredictCardsHandler : INotificationHandler<ProcessCredictCardsCommand>
{
    private readonly ILogger<ProcessCredictCardsHandler> _logger;
    private readonly IGiftCardService _service;
    private readonly IConfiguration _configuration;

    public ProcessCredictCardsHandler(ILogger<ProcessCredictCardsHandler> logger, IGiftCardService service,
        IConfiguration configuration)
    {
        this._logger = logger;
        this._service = service;
        _configuration = configuration;
    }

    public async Task Handle(ProcessCredictCardsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing {GiftcardsCount} gift cards", request.giftcards.Count);
        await _service.ProcessGc(request.giftcards, _configuration.GetConnectionString("DefaultConnection"));
        _logger.LogInformation("Complete process on {GiftcardsCount} gift cards", request.giftcards.Count);
    }
}