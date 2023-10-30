using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCartAbandonedEvent : INotification
{
    [JsonPropertyName("instanceId")] public string InstansceId { get; set; }
    [JsonPropertyName("data")] public string Data { get; set; }
}

public class BuyerInfo
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("identityType")] public string IdentityType { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
}

public class Totals
{
    [JsonPropertyName("subtotal")] public decimal Subtotal { get; set; }
    [JsonPropertyName("total")] public decimal Total { get; set; }
    [JsonPropertyName("formattedTotal")] public string FormattedTotal { get; set; }
}

public class CartData
{
    [JsonPropertyName("cartId")] public string CartId { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
    [JsonPropertyName("creationTime")] public CreationTime CreationTime { get; set; }
    [JsonPropertyName("abandonTime")] public AbandonTime AbandonTime { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo BuyerInfo { get; set; }
    [JsonPropertyName("itemsCount")] public int ItemsCount { get; set; }
    [JsonPropertyName("couponId")] public string CouponId { get; set; }
    [JsonPropertyName("totals")] public Totals Totals { get; set; }
    [JsonPropertyName("checkoutUrl")] public string CheckoutUrl { get; set; }
}

public class AbandonTime
{
}

public class CreationTime
{
}