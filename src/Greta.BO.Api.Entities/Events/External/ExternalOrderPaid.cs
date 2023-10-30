using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalOrderPaid : INotification
{
    [JsonPropertyName("firstName")] public string firstName { get; set; }
    [JsonPropertyName("lastName")] public string lastName { get; set; }
}

public class AddressOrderPaid
{
    [JsonPropertyName("fullName")] public FullName fullName { get; set; }
    [JsonPropertyName("country")] public string country { get; set; }
    [JsonPropertyName("city")] public string city { get; set; }
    [JsonPropertyName("zipCode")] public string zipCode { get; set; }
    [JsonPropertyName("phone")] public string phone { get; set; }
    [JsonPropertyName("email")] public string email { get; set; }
}

public class FullName
{
}

public class PriceDataOrderPaid
{
    [JsonPropertyName("taxIncludedInPrice")] public bool taxIncludedInPrice { get; set; }
    [JsonPropertyName("price")] public string price { get; set; }
    [JsonPropertyName("totalPrice")] public string totalPrice { get; set; }
}

public class LineItemOrderPaid
{
    [JsonPropertyName("index")] public int index { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("name")] public string name { get; set; }
    [JsonPropertyName("productId")] public string productId { get; set; }
    [JsonPropertyName("lineItemType")] public string lineItemType { get; set; }
    [JsonPropertyName("options")] public List<object> options { get; set; }
    [JsonPropertyName("customTextFields")] public List<object> customTextFields { get; set; }
    [JsonPropertyName("weight")] public string weight { get; set; }
    [JsonPropertyName("sku")] public string sku { get; set; }
    [JsonPropertyName("discount")] public string discount { get; set; }
    [JsonPropertyName("tax")] public string tax { get; set; }
    [JsonPropertyName("priceData")] public PriceData priceData { get; set; }
}

public class ActivityOrderPaid
{
    [JsonPropertyName("type")] public string type { get; set; }
    [JsonPropertyName("timestamp")] public DateTime timestamp { get; set; }
}

public class DiscountOrderPaid
{
    [JsonPropertyName("value")] public string value { get; set; }
}

public class PaymentStatus
{
    [JsonPropertyName("paymentStatus")] public string paymentStatus { get; set; }
}

public class FulfillmentStatus
{
    [JsonPropertyName("fulfillmentStatus")] public string fulfillmentStatus { get; set; }
}

public class ChannelInfoOrderPaid
{
    [JsonPropertyName("type")] public string type { get; set; }
}

public class EnteredBy
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("identityType")] public string identityType { get; set; }
}

public class Order
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("number")] public int number { get; set; }
    [JsonPropertyName("dateCreated")] public DateTime dateCreated { get; set; }
    [JsonPropertyName("currency")] public string currency { get; set; }
    [JsonPropertyName("weightUnit")] public string weightUnit { get; set; }
    [JsonPropertyName("totals")] public Totals totals { get; set; }
    [JsonPropertyName("billingInfo")] public BillingInfo billingInfo { get; set; }
    [JsonPropertyName("shippingInfo")] public ShippingInfo shippingInfo { get; set; }
    [JsonPropertyName("read")] public bool read { get; set; }
    [JsonPropertyName("archived")] public bool archived { get; set; }
    [JsonPropertyName("paymentStatus")] public PaymentStatus paymentStatus { get; set; }
    [JsonPropertyName("fulfillmentStatus")] public FulfillmentStatus fulfillmentStatus { get; set; }
    [JsonPropertyName("lineItems")] public List<LineItem> lineItems { get; set; }
    [JsonPropertyName("activities")] public List<Activity> activities { get; set; }
    [JsonPropertyName("fulfillments")] public List<object> fulfillments { get; set; }
    [JsonPropertyName("discount")] public Discount discount { get; set; }
    [JsonPropertyName("buyerLanguage")] public string buyerLanguage { get; set; }
    [JsonPropertyName("channelInfo")] public ChannelInfo channelInfo { get; set; }
    [JsonPropertyName("enteredBy")] public EnteredBy enteredBy { get; set; }
    [JsonPropertyName("lastUpdated")] public DateTime lastUpdated { get; set; }
}

public class TotalsOrderPaid
{
    [JsonPropertyName("subtotal")] public string subtotal { get; set; }
    [JsonPropertyName("shipping")] public string shipping { get; set; }
    [JsonPropertyName("tax")] public string tax { get; set; }
    [JsonPropertyName("discount")] public string discount { get; set; }
    [JsonPropertyName("total")] public string total { get; set; }
    [JsonPropertyName("weight")] public string weight { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
}

public class BillingInfoOrderPaid
{
    [JsonPropertyName("paymentMethod")] public string paymentMethod { get; set; }
    [JsonPropertyName("externalTransactionId")] public string externalTransactionId { get; set; }
    [JsonPropertyName("paymentProviderTransactionId")] public string paymentProviderTransactionId { get; set; }
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("paidDate")] public DateTime paidDate { get; set; }
}

public class ShipmentDetails
{
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("discount")] public string discount { get; set; }
    [JsonPropertyName("tax")] public string tax { get; set; }
    [JsonPropertyName("priceData")] public PriceData priceData { get; set; }
}

public class ShippingInfoOrderPaid
{
    [JsonPropertyName("deliveryOption")] public string deliveryOption { get; set; }
    [JsonPropertyName("estimatedDeliveryTime")] public string estimatedDeliveryTime { get; set; }
    [JsonPropertyName("shippingRegion")] public string shippingRegion { get; set; }
    [JsonPropertyName("shipmentDetails")] public ShipmentDetails shipmentDetails { get; set; }
}

public class OrderPaidEventData
{
    [JsonPropertyName("order")] public Order order { get; set; }
}

public class OrderPaidEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
    [JsonPropertyName("eventData")] public OrderPaidEventData eventData { get; set; }
}