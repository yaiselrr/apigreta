using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalOrderEvent : INotification
{
    [JsonPropertyName("data")] public string Data { get; set; }
    [JsonPropertyName("instanceId")] public string InstansceId1 { get; set; }

    // [JsonPropertyName("iat")] public long Iat { get; set; }
    // [JsonPropertyName("Exp")] public long Exp { get; set; }
}

public class BuyerInfoOrder
{
    // Wix customer ID
    [JsonPropertyName("id")] public string Id { get; set; }

    // 3 supported values: UNSPECIFIED_IDENTITY_TYPE, MEMBER, CONTACT. Deprecated (use identityType instead)
    [JsonPropertyName("type")] public string Type { get; set; }

    // 3 supported values: UNSPECIFIED_IDENTITY_TYPE, MEMBER, CONTACT. Customer type
    [JsonPropertyName("identityType")] public string IdentityType { get; set; }

    // Customer's first name
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
}

// Totals for order's line items
public class TotalsOrder
{
    // Subtotal of all the line items, before tax.
    [JsonPropertyName("subtotal")] public string Subtotal { get; set; }

    // Total shipping price, before tax.
    [JsonPropertyName("shipping")] public string Shipping { get; set; }

    // format DECIMAL_VALUE. Total tax.
    [JsonPropertyName("tax")] public string Tax { get; set; }

    // format DECIMAL_VALUE. Total calculated discount value.
    [JsonPropertyName("discount")] public string Discount { get; set; }

    // format DECIMAL_VALUE. Total price charged.
    [JsonPropertyName("total")] public string Total { get; set; }

    // Total items weight.
    [JsonPropertyName("weight")] public string Weight { get; set; }

    // Total number of line items.
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
}

public class OrderData
{
    // Order ID (auto generated upon order creation)
    [JsonPropertyName("orderId")] public string OrderId { get; set; }

    // ID displayed in the owner's store (auto generated)
    [JsonPropertyName("number")] public string Number { get; set; }
    [JsonPropertyName("dateCreated")] public DateTime DateCreated { get; set; }

    // Customer information
    [JsonPropertyName("buyerInfo")] public BuyerInfoOrder BuyerInfo { get; set; }

    // Currency used for pricing in this store
    [JsonPropertyName("currency")] public string Currency { get; set; }

    // 3 supported values: UNSPECIFIED_WEIGHT_UNIT, KG, LB. Weight unit used in this store
    [JsonPropertyName("weightUnit")] public string WeightUnit { get; set; }

    // Totals for order's line items
    [JsonPropertyName("totals")] public TotalsOrder Totals { get; set; }

    // Whether the order was read by the store owner
    [JsonPropertyName("read")] public bool Read { get; set; }

    // 7 supported values: UNSPECIFIED_PAYMENT_STATUS, PENDING, NOT_PAID, PAID, PARTIALLY_REFUNDED, FULLY_REFUNDED, PARTIALLY_PAID. Order payment status
    [JsonPropertyName("paymentStatus")] public string PaymentStatus { get; set; }

    // 4 supported values: NOT_FULFILLED, FULFILLED, CANCELED, PARTIALLY_FULFILLED. Order fulfillment status
    [JsonPropertyName("fulfillmentStatus")] public string FulfillmentStatus { get; set; }
}
