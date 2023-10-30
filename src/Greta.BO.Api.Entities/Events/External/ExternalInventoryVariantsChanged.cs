using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalInventoryVariantsChanged : INotification
{
    [JsonPropertyName("id")] public string Id { get; set; }
    // public Dictionary<string, bool> OldValue { get; set; }
    // public Dictionary<string, bool> NewValue { get; set; }
}

public class InventoryVariantsChangedData
{
    [JsonPropertyName("inventoryItemId")] public string InventoryItemId { get; set; }
    [JsonPropertyName("externalId")] public string ExternalId { get; set; }
    [JsonPropertyName("productId")] public string ProductId { get; set; }
    [JsonPropertyName("variants")] public List<Variant> Variants { get; set; }
}

public class Variant
{

}

public class InventoryVariantsChangedEvent
{
    [JsonPropertyName("data")] public InventoryVariantsChangedData Data { get; set; }
    [JsonPropertyName("instanceId")] public string InstanceId { get; set; }
    [JsonPropertyName("eventType")] public string EventType { get; set; }
}

public class RootObject
{
    [JsonPropertyName("data")] public InventoryVariantsChangedEvent Data { get; set; }
    [JsonPropertyName("iat")] public int Iat { get; set; }
    [JsonPropertyName("exp")] public int Exp { get; set; }
}