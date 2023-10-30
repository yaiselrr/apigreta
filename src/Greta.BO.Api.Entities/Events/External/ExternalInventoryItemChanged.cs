using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalInventoryItemChanged : INotification
{
    [JsonPropertyName("inventoryItemId")] public string inventoryItemId { get; set; }
    [JsonPropertyName("externalId")] public string externalId { get; set; }
    [JsonPropertyName("productId")] public string productId { get; set; }
    [JsonPropertyName("trackInventory")] public bool trackInventory { get; set; }
}

public class InventoryItemChangedEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
}