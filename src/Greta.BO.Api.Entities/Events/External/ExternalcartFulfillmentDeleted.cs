using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalFulfillmentDeleted : INotification
{
    [JsonPropertyName("orderId")] public string orderId { get; set; }
    [JsonPropertyName("fulfillmentId")] public string fulfillmentId { get; set; }
    [JsonPropertyName("fulfillmentStatus")] public string fulfillmentStatus { get; set; }
}

public class FulfillmentDeletedEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
    [JsonPropertyName("eventData")] public FulfillmentDeletedEventData eventData { get; set; }
}

public class FulfillmentDeletedEventData
{
}