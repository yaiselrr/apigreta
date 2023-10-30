using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalProductDeleted : INotification
{
    [JsonPropertyName("productId")] public string ProductId { get; set; }
}