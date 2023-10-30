using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

// Triggered when an order is refunded.
public class ExternalOrderRefunded : INotification
{
    [JsonPropertyName("instanceId")] public string InstanceId { get; set; }
}