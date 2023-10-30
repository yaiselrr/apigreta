using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalorder_payment_status_updated : INotification
{
    [JsonPropertyName("order")] public Order order { get; set; }
    [JsonPropertyName("previousPaymentStatus")] public string previousPaymentStatus { get; set; }
}

public class Order_payment_status_updated
{
    [JsonPropertyName("number")] public string number { get; set; }
    [JsonPropertyName("additionalFees")] public List<object> additionalFees { get; set; }
    [JsonPropertyName("cartId")] public string cartId { get; set; }
    [JsonPropertyName("lineItems")] public List<LineItem> lineItems { get; set; }
    [JsonPropertyName("paymentStatus")] public string paymentStatus { get; set; }
    [JsonPropertyName("archived")] public bool archived { get; set; }
    [JsonPropertyName("seenByAHuman")] public bool seenByAHuman { get; set; }
    [JsonPropertyName("shippingInfo")] public ShippingInfo shippingInfo { get; set; }
    [JsonPropertyName("activities")] public List<Activity> activities { get; set; }
    [JsonPropertyName("appliedDiscounts")] public List<AppliedDiscount> appliedDiscounts { get; set; }
    [JsonPropertyName("customFields")] public List<object> customFields { get; set; }
    [JsonPropertyName("taxIncludedInPrices")] public bool taxIncludedInPrices { get; set; }
    [JsonPropertyName("attributionSource")] public string attributionSource { get; set; }
    [JsonPropertyName("weightUnit")] public string weightUnit { get; set; }
    [JsonPropertyName("priceSummary")] public PriceSummary priceSummary { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("isInternalOrderCreate")] public bool isInternalOrderCreate { get; set; }
    [JsonPropertyName("status")] public string status { get; set; }
    [JsonPropertyName("billingInfo")] public BillingInfo billingInfo { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo buyerInfo { get; set; }
    [JsonPropertyName("payNow")] public PayNow payNow { get; set; }
    [JsonPropertyName("createdDate")] public DateTime createdDate { get; set; }
}

public class LineItem_payment_status_updated
{
    [JsonPropertyName("physicalProperties")] public PhysicalProperties physicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string paymentOption { get; set; }
    [JsonPropertyName("image")] public Image image { get; set; }
    [JsonPropertyName("price")] public Price price { get; set; }
    [JsonPropertyName("totalPriceBeforeTax")] public Price totalPriceBeforeTax { get; set; }
    [JsonPropertyName("totalPriceAfterTax")] public Price totalPriceAfterTax { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public Price priceBeforeDiscounts { get; set; }
    [JsonPropertyName("totalDiscount")] public Price totalDiscount { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("itemType")] public ItemType itemType { get; set; }
    [JsonPropertyName("taxDetails")] public TaxDetails taxDetails { get; set; }
    [JsonPropertyName("productName")] public ProductName productName { get; set; }
    [JsonPropertyName("descriptionLines")] public List<DescriptionLine> descriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference catalogReference { get; set; }
    [JsonPropertyName("refundQuantity")] public int refundQuantity { get; set; }
}

public class PhysicalProperties_payment_status_updated
{
    [JsonPropertyName("sku")] public string sku { get; set; }
    [JsonPropertyName("shippable")] public bool shippable { get; set; }
}

public class Image_payment_status_updated
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
    [JsonPropertyName("height")] public int height { get; set; }
    [JsonPropertyName("width")] public int width { get; set; }
}

public class Price_payment_status_updated
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
}

public class ItemType_payment_status_updated
{
    [JsonPropertyName("preset")] public string preset { get; set; }
}

public class TaxDetails_payment_status_updated
{
    [JsonPropertyName("taxableAmount")] public Price taxableAmount { get; set; }
    [JsonPropertyName("taxRate")] public string taxRate { get; set; }
    [JsonPropertyName("totalTax")] public Price totalTax { get; set; }
}

public class ProductName_payment_status_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class DescriptionLine_payment_status_updated
{
    [JsonPropertyName("name")] public Name name { get; set; }
    [JsonPropertyName("plainText")] public PlainText plainText { get; set; }
    [JsonPropertyName("lineType")] public string lineType { get; set; }
    [JsonPropertyName("plainTextValue")] public PlainTextValue plainTextValue { get; set; }
}

public class Name_payment_status_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainText_payment_status_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainTextValue_payment_status_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class CatalogReference_payment_status_updated
{
    [JsonPropertyName("catalogItemId")] public string catalogItemId { get; set; }
    [JsonPropertyName("appId")] public string appId { get; set; }
    [JsonPropertyName("options")] public Options options { get; set; }
}

public class Options_payment_status_updated
{
    [JsonPropertyName("options")] public Dictionary<string, string> options { get; set; }
    [JsonPropertyName("variantId")] public string variantId { get; set; }
}

public class AppliedDiscount_payment_status_updated
{
    [JsonPropertyName("discountType")] public string discountType { get; set; }
    [JsonPropertyName("lineItemIds")] public List<string> lineItemIds { get; set; }
    [JsonPropertyName("coupon")] public Coupon coupon { get; set; }
}

public class Coupon_payment_status_updated
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
    [JsonPropertyName("name")] public string name { get; set; }
    [JsonPropertyName("amount")] public Price amount { get; set; }
}

public class ShippingInfo_payment_status_updated
{
    [JsonPropertyName("code")] public string code { get; set; }
    [JsonPropertyName("cost")] public Price cost { get; set; }
    [JsonPropertyName("logistics")] public Logistics logistics { get; set; }
    [JsonPropertyName("carrierId")] public string carrierId { get; set; }
    [JsonPropertyName("region")] public Region region { get; set; }
    [JsonPropertyName("title")] public string title { get; set; }
}

public class Logistics_payment_status_updated
{
    [JsonPropertyName("deliveryTime")] public string deliveryTime { get; set; }
}

public class Region_payment_status_updated
{
    [JsonPropertyName("name")] public string name { get; set; }
}

public class Activity_payment_status_updated
{
    [JsonPropertyName("createdDate")] public DateTime createdDate { get; set; }
    [JsonPropertyName("type")] public string type { get; set; }
}

public class PriceSummary_payment_status_updated
{
    [JsonPropertyName("totalWithGiftCard")] public Price totalWithGiftCard { get; set; }
    [JsonPropertyName("totalAdditionalFees")] public Price totalAdditionalFees { get; set; }
    [JsonPropertyName("tax")] public Price tax { get; set; }
    [JsonPropertyName("total")] public Price total { get; set; }
    [JsonPropertyName("totalPrice")] public Price totalPrice { get; set; }
    [JsonPropertyName("subtotal")] public Price subtotal { get; set; }
    [JsonPropertyName("totalWithoutGiftCard")] public Price totalWithoutGiftCard { get; set; }
    [JsonPropertyName("discount")] public Price discount { get; set; }
    [JsonPropertyName("shipping")] public Price shipping { get; set; }
}

public class BillingInfo_payment_status_updated
{
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails contactDetails { get; set; }
}

public class Address_payment_status_updated
{
    [JsonPropertyName("city")] public string city { get; set; }
    [JsonPropertyName("countryFullname")] public string countryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string subdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string addressLine { get; set; }
    [JsonPropertyName("country")] public string country { get; set; }
    [JsonPropertyName("postalCode")] public string postalCode { get; set; }
    [JsonPropertyName("subdivision")] public string subdivision { get; set; }
}

public class ContactDetails_payment_status_updated
{
    [JsonPropertyName("firstName")] public string firstName { get; set; }
    [JsonPropertyName("lastName")] public string lastName { get; set; }
    [JsonPropertyName("phone")] public string phone { get; set; }
}

public class BuyerInfo_payment_status_updated
{
    [JsonPropertyName("contactId")] public string contactId { get; set; }
    [JsonPropertyName("email")] public string email { get; set; }
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class PayNow_payment_status_updated
{
    [JsonPropertyName("totalWithGiftCard")] public Price totalWithGiftCard { get; set; }
    [JsonPropertyName("tax")] public Price tax { get; set; }
    [JsonPropertyName("total")] public Price total { get; set; }
    [JsonPropertyName("totalPrice")] public Price totalPrice { get; set; }
    [JsonPropertyName("subtotal")] public Price subtotal { get; set; }
    [JsonPropertyName("totalWithoutGiftCard")] public Price totalWithoutGiftCard { get; set; }
    [JsonPropertyName("discount")] public Price discount { get; set; }
    [JsonPropertyName("shipping")] public Price shipping { get; set; }
}