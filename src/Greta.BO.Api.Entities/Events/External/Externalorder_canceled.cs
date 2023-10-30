using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalorder_canceled : INotification
{
    [JsonPropertyName("sku")] public string Sku { get; set; }
    [JsonPropertyName("shippable")] public bool Shippable { get; set; }
}

// public class Image
// {
// [JsonPropertyName("id")] public string Id { get; set; }
// [JsonPropertyName("url")] public string Url { get; set; }
// [JsonPropertyName("height")] public int Height { get; set; }
// [JsonPropertyName("width")] public int Width { get; set; }
// }

public class PriceOrder
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalPriceBeforeTax
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalPriceAfterTax
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class PriceBeforeDiscounts
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalDiscount
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class ItemTypeOrder
{
    [JsonPropertyName("preset")] public string Preset { get; set; }
}

public class TaxableAmount
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalTax
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class CatalogReferenceOrder
{
    [JsonPropertyName("catalogItemId")] public string CatalogItemId { get; set; }
    [JsonPropertyName("appId")] public string AppId { get; set; }
    [JsonPropertyName("options")] public Dictionary<string, object> Options { get; set; }
}

public class LineItemOrder
{
    [JsonPropertyName("physicalProperties")] public PhysicalProperties PhysicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string PaymentOption { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("price")] public Price Price { get; set; }
    [JsonPropertyName("totalPriceBeforeTax")] public TotalPriceBeforeTax TotalPriceBeforeTax { get; set; }
    [JsonPropertyName("totalPriceAfterTax")] public TotalPriceAfterTax TotalPriceAfterTax { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public PriceBeforeDiscounts PriceBeforeDiscounts { get; set; }
    [JsonPropertyName("totalDiscount")] public TotalDiscount TotalDiscount { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("itemType")] public ItemType ItemType { get; set; }
    [JsonPropertyName("taxDetails")] public Dictionary<string, object> TaxDetails { get; set; }
    [JsonPropertyName("productName")] public Dictionary<string, object> ProductName { get; set; }
    [JsonPropertyName("descriptionLines")] public List<object> DescriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference CatalogReference { get; set; }
    [JsonPropertyName("refundQuantity")] public int RefundQuantity { get; set; }
}

public class Cost
{
    [JsonPropertyName("price")] public Price Price { get; set; }
    [JsonPropertyName("totalPriceBeforeTax")] public TotalPriceBeforeTax TotalPriceBeforeTax { get; set; }
    [JsonPropertyName("totalPriceAfterTax")] public TotalPriceAfterTax TotalPriceAfterTax { get; set; }
    [JsonPropertyName("taxDetails")] public Dictionary<string, object> TaxDetails { get; set; }
    [JsonPropertyName("discount")] public Dictionary<string, object> Discount { get; set; }
}

public class AddressOrder
{
    [JsonPropertyName("city")] public string City { get; set; }
    [JsonPropertyName("countryFullname")] public string CountryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string SubdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string AddressLine { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; }
    [JsonPropertyName("postalCode")] public string PostalCode { get; set; }
    [JsonPropertyName("subdivision")] public string Subdivision { get; set; }
}

public class ContactDetailsOrder
{
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
}

public class LogisticsOrder
{
    [JsonPropertyName("deliveryTime")] public string DeliveryTime { get; set; }
    [JsonPropertyName("shippingDestination")] public Address ShippingDestination { get; set; }
}

public class ShippingInfoOrder
{
    [JsonPropertyName("code")] public string Code { get; set; }
    [JsonPropertyName("cost")] public Cost Cost { get; set; }
    [JsonPropertyName("logistics")] public Logistics Logistics { get; set; }
    [JsonPropertyName("carrierId")] public string CarrierId { get; set; }
    [JsonPropertyName("region")] public Region Region { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
}

public class ActivityOrder
{
    [JsonPropertyName("createdDate")] public DateTime CreatedDate { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class Discount
{
    [JsonPropertyName("discountType")] public string DiscountType { get; set; }
    [JsonPropertyName("lineItemIds")] public List<string> LineItemIds { get; set; }
    [JsonPropertyName("coupon")] public Dictionary<string, object> Coupon { get; set; }
}

public class TotalWithGiftCard
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Tax
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Total
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalPrice
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Subtotal
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalWithoutGiftCard
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Shipping
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class PriceSummary
{
    [JsonPropertyName("totalWithGiftCard")] public TotalWithGiftCard TotalWithGiftCard { get; set; }
    [JsonPropertyName("totalAdditionalFees")] public Price TotalAdditionalFees { get; set; }
    [JsonPropertyName("tax")] public Tax Tax { get; set; }
    [JsonPropertyName("total")] public Total Total { get; set; }
    [JsonPropertyName("totalPrice")] public TotalPrice TotalPrice { get; set; }
    [JsonPropertyName("subtotal")] public Subtotal Subtotal { get; set; }
    [JsonPropertyName("totalWithoutGiftCard")] public TotalWithoutGiftCard TotalWithoutGiftCard { get; set; }
    [JsonPropertyName("discount")] public TotalDiscount Discount { get; set; }
    [JsonPropertyName("shipping")] public Shipping Shipping { get; set; }
}

public class Address2
{
    [JsonPropertyName("city")] public string City { get; set; }
    [JsonPropertyName("countryFullname")] public string CountryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string SubdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string AddressLine { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; }
    [JsonPropertyName("postalCode")] public string PostalCode { get; set; }
    [JsonPropertyName("subdivision")] public string Subdivision { get; set; }
}

public class ContactDetails2
{
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
}

public class BillingInfoOrder
{
    [JsonPropertyName("address")] public Address2 Address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails2 ContactDetails { get; set; }
}

public class ContactDetails3
{
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
}

public class BuyerInfoOrderCacelled
{
    [JsonPropertyName("contactId")] public string ContactId { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("visitorId")] public string VisitorId { get; set; }
}

public class TotalWithGiftCard2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Tax2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Total2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalPrice2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Subtotal2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class TotalWithoutGiftCard2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class Shipping2
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class PayNowOrder
{
    [JsonPropertyName("totalWithGiftCard")] public TotalWithGiftCard2 TotalWithGiftCard { get; set; }
    [JsonPropertyName("tax")] public Tax2 Tax { get; set; }
    [JsonPropertyName("total")] public Total2 Total { get; set; }
    [JsonPropertyName("totalPrice")] public TotalPrice2 TotalPrice { get; set; }
    [JsonPropertyName("subtotal")] public Subtotal2 Subtotal { get; set; }
    [JsonPropertyName("totalWithoutGiftCard")] public TotalWithoutGiftCard2 TotalWithoutGiftCard { get; set; }
    [JsonPropertyName("discount")] public TotalDiscount Discount { get; set; }
    [JsonPropertyName("shipping")] public Shipping2 Shipping { get; set; }
}

public class VisitorId
{
    [JsonPropertyName("visitorId")] public string VisitorId2 { get; set; }
}

public class CreatedBy
{
    [JsonPropertyName("visitorId")] public VisitorId VisitorId { get; set; }
}

public class TotalTaxOrder
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class RegionOrder
{
    [JsonPropertyName("name")] public string Name { get; set; }
}

public class Balance
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class BalanceSummaryOrder
{
    [JsonPropertyName("balance")] public Balance Balance { get; set; }
}

public class ChannelInfoOrder
{
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class OrderCanceledData
{
    [JsonPropertyName("order")] public Dictionary<string, object> Order { get; set; }
    [JsonPropertyName("restockAllItems")] public bool RestockAllItems { get; set; }
    [JsonPropertyName("sendOrderCanceledEmail")] public bool SendOrderCanceledEmail { get; set; }
    [JsonPropertyName("customMessage")] public string CustomMessage { get; set; }
}

public class OrderCanceledEvent
{
    [JsonPropertyName("data")] public OrderCanceledData Data { get; set; }
    [JsonPropertyName("instanceId")] public string InstanceId { get; set; }
    [JsonPropertyName("eventType")] public string EventType { get; set; }
}

public class RootObjectOrder
{
    [JsonPropertyName("data")] public OrderCanceledEvent Data { get; set; }
    [JsonPropertyName("iat")] public int Iat { get; set; }
    [JsonPropertyName("exp")] public int Exp { get; set; }
}