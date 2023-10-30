using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Greta.BO.Wix.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Greta.BO.Wix.Handlers.Commands.Webhook;

public record ProcessWebhook(string PlainText) : INotification;

public class ProcessWebhookLog : INotificationHandler<ProcessWebhook>
{
    private readonly ILogger<ProcessWebhookLog> _logger;
    private readonly WixOptions _options;
    private readonly IMediator _mediator;

    public ProcessWebhookLog(
        ILogger<ProcessWebhookLog> logger,
        WixOptions options,
        IMediator mediator
    )
    {
        _logger = logger;
        _options = options;
        _mediator = mediator;
    }

    public async Task Handle(ProcessWebhook notification, CancellationToken cancellationToken)
    {

        var key = _options.WebHookApiPublicKey;
        
        if (key == null || _options.ClientId == null)
        {
            _logger.LogWarning("Webhook process disable public key not present");
            return;
        }

        var handler = new JwtSecurityTokenHandler();

        var rsa = RSA.Create();
        var jwtToken = notification.PlainText;

        // Load PEm
        var certificateBytes = Convert.FromBase64String(key);
        rsa.ImportFromPem(Encoding.ASCII.GetString(certificateBytes));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        try
        {
            var decodedValue = handler.ReadJwtToken(jwtToken);
            handler.ValidateToken(jwtToken, validationParameters, out _);
            var payload2 = decodedValue.Payload;
            var data = payload2["data"].ToString();
            if (data == null) return;
            var valueReceive = JsonSerializer.Deserialize<WixWebHookData>(data);
            try
            {
                var classSuffix = valueReceive?.EventType.Split('.').Last();
                Console.WriteLine();
                Console.WriteLine("event occurred:");
                Console.WriteLine(classSuffix);
                var classType = Type.GetType($"Greta.BO.Api.Entities.Events.External.External{classSuffix}, Greta.BO.Api.Entities");
                var eventObject = JsonSerializer.Deserialize(valueReceive!.Data, classType!);
                await _mediator.Publish(eventObject!, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wix event {Type} with data {Data} not implemented", valueReceive?.EventType,
                    data);
            }
        }
        catch (SecurityTokenException)
        {
            _logger.LogError("Invalid webhook jwt");
        }
    }
}