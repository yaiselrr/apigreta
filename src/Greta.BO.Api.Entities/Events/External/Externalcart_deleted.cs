using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalcart_deleted : INotification
{
    [JsonPropertyName("movedToTrash")] public bool MovedToTrash { get; set; }
}

public class CartDeletedData
{
    [JsonPropertyName("entityId")] public string EntityId { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("entityFqdn")] public string EntityFqdn { get; set; }
    [JsonPropertyName("deletedEvent")] public DeletedEvent DeletedEvent { get; set; }
    [JsonPropertyName("eventTime")] public DateTime EventTime { get; set; }
    [JsonPropertyName("triggeredByAnonymizeRequest")] public bool TriggeredByAnonymizeRequest { get; set; }
}

public class DeletedEvent
{
}

public class CartDeletedEvent
{
    [JsonPropertyName("data")] public CartDeletedData Data { get; set; }
    [JsonPropertyName("instanceId")] public string InstanceId { get; set; }
    [JsonPropertyName("eventType")] public string EventType { get; set; }
}

// public class RootObjectDelete
// {
//     [JsonPropertyName("data")] public CartDeletedEvent Data { get; set; }
//     [JsonPropertyName("iat")] public int Iat { get; set; }
//     [JsonPropertyName("exp")] public int Exp { get; set; }
// }