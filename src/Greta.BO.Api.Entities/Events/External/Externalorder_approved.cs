using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalorder_approved : INotification
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("slug")]public string Slug { get; set; }
    [JsonPropertyName("entityId")]public string EntityId { get; set; }
    [JsonPropertyName("entityFqdn")]public string EntityFqdn { get; set; }
    // public DateTime EventTime { get; set; }
    [JsonPropertyName("actionEvent")]public ActionEvent ActionEvent { get; set; }
}

public class ActionEvent
{
    [JsonPropertyName("body")] public Body Body { get; set; }
}

public class Body
{
    [JsonPropertyName("order")] public OrderData Order { get; set; }
}

public class OrderDataApproved
{
    [JsonPropertyName("number")] public string Number { get; set; }
    [JsonPropertyName("cartId")] public string CartId { get; set; }
    [JsonPropertyName("lineItems")] public LineItem[] LineItems { get; set; }
    [JsonPropertyName("paymentStatus")] public string PaymentStatus { get; set; }
    [JsonPropertyName("archived")] public bool Archived { get; set; }
    [JsonPropertyName("seenByAHuman")] public bool SeenByAHuman { get; set; }
    [JsonPropertyName("shippingInfo")] public ShippingInfo ShippingInfo { get; set; }
    [JsonPropertyName("activities")] public Activity[] Activities { get; set; }
    [JsonPropertyName("appliedDiscounts")] public AppliedDiscount[] AppliedDiscounts { get; set; }
    [JsonPropertyName("billingInfo")] public BillingInfo BillingInfo { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo BuyerInfo { get; set; }
    [JsonPropertyName("payNow")] public PayNow PayNow { get; set; }
    [JsonPropertyName("isInternalOrderCreate")] public bool IsInternalOrderCreate { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
    // public PriceSummaryApproved PriceSummary { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("taxSummary")] public TaxSummary TaxSummary { get; set; }
    [JsonPropertyName("currency")] public string Currency { get; set; }
    [JsonPropertyName("balanceSummary")] public BalanceSummary BalanceSummary { get; set; }
     // public DateTimeApproved UpdatedDate { get; set; }
    [JsonPropertyName("checkoutId")] public string CheckoutId { get; set; }
    [JsonPropertyName("buyerLanguage")] public string BuyerLanguage { get; set; }
    [JsonPropertyName("channelInfo")] public ChannelInfo ChannelInfo { get; set; }
    [JsonPropertyName("fulfillmentStatus")] public string FulfillmentStatus { get; set; }
    // public DateTime CreatedDate { get; set; }
}

public class LineItem
{
    [JsonPropertyName("physicalProperties")] public PhysicalProperties PhysicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string PaymentOption { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("price")] public Price Price { get; set; }
    [JsonPropertyName("totalPriceBeforeTax")] public Price TotalPriceBeforeTax { get; set; }
    [JsonPropertyName("totalPriceAfterTax")] public Price TotalPriceAfterTax { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public Price PriceBeforeDiscounts { get; set; }
    [JsonPropertyName("totalDiscount")] public Price TotalDiscount { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("itemType")] public ItemType ItemType { get; set; }
    [JsonPropertyName("taxDetails")] public TaxDetails TaxDetails { get; set; }
    [JsonPropertyName("productName")] public ProductName ProductName { get; set; }
    [JsonPropertyName("descriptionLines")] public object[] DescriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference CatalogReference { get; set; }
    [JsonPropertyName("refundQuantity")] public int RefundQuantity { get; set; }
}

public class PhysicalProperties
{
    [JsonPropertyName("sku")] public string Sku { get; set; }
    [JsonPropertyName("shippable")] public bool Shippable { get; set; }
}

public class ImageApproved
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
}

public class Price
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
}

public class ItemType
{
   [JsonPropertyName("preset")] public string Preset { get; set; }
}

public class TaxDetails
{
    [JsonPropertyName("taxableAmount")] public Price TaxableAmount { get; set; }
    [JsonPropertyName("taxRate")] public string TaxRate { get; set; }
    [JsonPropertyName("totalTax")] public Price TotalTax { get; set; }
}

public class ProductName
{
    [JsonPropertyName("original")] public string Original { get; set; }
    [JsonPropertyName("translated")] public string Translated { get; set; }
}

public class CatalogReference
{
    [JsonPropertyName("catalogItemId")] public string CatalogItemId { get; set; }
    [JsonPropertyName("appId")] public string AppId { get; set; }
    [JsonPropertyName("options")] public Options Options { get; set; }
}

public class Options
{
    // public object Options { get; set; }
   [JsonPropertyName("variantId")] public string VariantId { get; set; }
}

public class TaxSummary
{
   [JsonPropertyName("totalTax")] public Price TotalTax { get; set; }
}

public class BalanceSummary
{
   [JsonPropertyName("balance")] public Price Balance { get; set; }
}

public class Activity
{
    // public DateTime CreatedDate { get; set; }
   [JsonPropertyName("type")] public string Type { get; set; }
}

public class AppliedDiscount
{
    [JsonPropertyName("discountType")] public string DiscountType { get; set; }
    [JsonPropertyName("lineItemIds")] public string[] LineItemIds { get; set; }
    [JsonPropertyName("coupon")] public Coupon Coupon { get; set; }
}

public class Coupon
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("code")] public string Code { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("amount")] public Price Amount { get; set; }
}

public class BillingInfo
{
    [JsonPropertyName("address")] public Address Address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails ContactDetails { get; set; }
}

public class AddressApproved
{
    [JsonPropertyName("city")] public string City { get; set; }
    [JsonPropertyName("countryFullname")] public string CountryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string SubdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string AddressLine { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; }
    [JsonPropertyName("postalCode")] public string PostalCode { get; set; }
    [JsonPropertyName("subdivision")] public string Subdivision { get; set; }
}

public class ContactDetailsApproved
{
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
}

public class BuyerInfoApproved
{
    [JsonPropertyName("contactId")] public string ContactId { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("visitorId")] public string VisitorId { get; set; }
}

public class PayNow
{
    [JsonPropertyName("totalWithGiftCard")] public Price TotalWithGiftCard { get; set; }
    [JsonPropertyName("tax")] public Price Tax { get; set; }
    [JsonPropertyName("totalPrice")] public Price TotalPrice { get; set; }
    [JsonPropertyName("subtotal")] public Price Subtotal { get; set; }
    [JsonPropertyName("totalWithoutGiftCard")] public Price TotalWithoutGiftCard { get; set; }
    [JsonPropertyName("discount")] public Price Discount { get; set; }
    [JsonPropertyName("shipping")] public Price Shipping { get; set; }
}

public class ShippingInfoApproved
{
     [JsonPropertyName("code")] public string Code { get; set; }
     [JsonPropertyName("cost")] public Price Cost { get; set; }
     [JsonPropertyName("logistics")] public Logistics Logistics { get; set; }
     [JsonPropertyName("carrierId")] public string CarrierId { get; set; }
     [JsonPropertyName("region")] public Region Region { get; set; }
     [JsonPropertyName("title")] public string Title { get; set; }
}

public class Logistics
{
    [JsonPropertyName("deliveryTime")] public string DeliveryTime { get; set; }
    [JsonPropertyName("shippingDestination")] public ShippingDestination ShippingDestination { get; set; }
}

public class ShippingDestination
{
    [JsonPropertyName("address")] public Address Address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails ContactDetails { get; set; }
}

public class Region
{
    [JsonPropertyName("name")] public string Name { get; set; }
}

public class ChannelInfo
{
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class Root
{
    [JsonPropertyName("data")] public string Data { get; set; }
    [JsonPropertyName("iat")] public long Iat { get; set; }
    [JsonPropertyName("exp")] public long Exp { get; set; }
    [JsonPropertyName("instanceId")] public string InstanceId { get; set; }
    [JsonPropertyName("eventType")] public string EventType { get; set; }
}