using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCollectionDeleted : INotification
{
    [JsonPropertyName("collectionId")] public string CollectionId { get; set; }
}