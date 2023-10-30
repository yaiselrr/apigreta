using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCartCreatedEvent : INotification
{
    [JsonPropertyName("email")] public string email { get; set; }
    [JsonPropertyName("firstName")] public string firstName { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("identityType")] public string identityType { get; set; }
    [JsonPropertyName("lastName")] public string lastName { get; set; }
    [JsonPropertyName("phone")] public string phone { get; set; }
}

public class CurrencyCartCreatedEvent
{
    [JsonPropertyName("currency")] public string currency { get; set; }
    [JsonPropertyName("symbol")] public string symbol { get; set; }
}

public class TotalsCartCreatedEvent
{
    [JsonPropertyName("discount")] public int discount { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("subtotal")] public int subtotal { get; set; }
    [JsonPropertyName("total")] public int total { get; set; }
}

public class CartCreatedEventDataCartCreatedEvent
{
    [JsonPropertyName("cartId")] public string cartId { get; set; }
    [JsonPropertyName("weightUnit")] public string weightUnit { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo buyerInfo { get; set; }
    [JsonPropertyName("currency")] public Currency currency { get; set; }
    [JsonPropertyName("totals")] public Totals totals { get; set; }
    [JsonPropertyName("creationTime")] public DateTime creationTime { get; set; }
}

public class CartCreatedEventCartCreatedEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
    [JsonPropertyName("eventData")] public CartCreatedEventData eventData { get; set; }
}