using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalFulfillmentUpdated : INotification
{
    [JsonPropertyName("trackingNumber")] public string trackingNumber { get; set; }
    [JsonPropertyName("shippingProvider")] public string shippingProvider { get; set; }
    [JsonPropertyName("trackingLink")] public string trackingLink { get; set; }
}

public class RootFulfillmentUpdated
{
    [JsonPropertyName("orderId")] public string orderId { get; set; }
    [JsonPropertyName("fulfillmentId")] public string fulfillmentId { get; set; }
    [JsonPropertyName("trackingInfo")] public TrackingInfo trackingInfo { get; set; }
}

public class TrackingInfo
{
}

public class FulfillmentUpdatedEvent
{
    [JsonPropertyName("data")] public Root data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
}