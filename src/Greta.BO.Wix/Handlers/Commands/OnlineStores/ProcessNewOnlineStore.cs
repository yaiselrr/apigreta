using System.Text.Json;
using Greta.BO.Wix.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Greta.BO.Wix.Handlers.Commands.ProcessNewOnlineStore;

public record ProcessNewOnlineStore(string accessCode) : INotification;

public class ProcessNewOnlineStoreLog : INotificationHandler<ProcessNewOnlineStore>
{
    private readonly ILogger<ProcessNewOnlineStoreLog> _logger;
    private readonly WixOptions _options;
    private readonly IMediator _mediator;

    public ProcessNewOnlineStoreLog(
        ILogger<ProcessNewOnlineStoreLog> logger,
        WixOptions options,
        IMediator mediator
    )
    {
        _logger = logger;
        _options = options;
        _mediator = mediator;
    }

    public async Task Handle(ProcessNewOnlineStore notification, CancellationToken cancellationToken)
    {

        var key = _options.WebHookApiPublicKey;

        if (key == null || _options.ClientId == null || _options.ClientSecret == null)
        {
            _logger.LogWarning("New Online Store process disable public key not present");
            return;
        }
        var codeToken = JsonSerializer.Deserialize<RefreshTokenResponse>(notification.accessCode);
        var request = new RestRequest($"https://www.wixapis.com/oauth/access")
                       .AddJsonBody(new
                       {
                           grant_type = "authorization_code",
                           client_id = _options.ClientId,
                           client_secret = _options.ClientSecret,
                           code = codeToken.Code
                       });
        var restClient = new RestClient("https://www.wixapis.com/oauth/access");
        var response = await restClient.PostAsync<RefreshTokenResponse>(request);

    }
}