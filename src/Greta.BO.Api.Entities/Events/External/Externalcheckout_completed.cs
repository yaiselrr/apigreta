using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalcheckout_completed : INotification
{
    [JsonPropertyName("physicalProperties")] public PhysicalProperties physicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string paymentOption { get; set; }
    [JsonPropertyName("couponScopes")] public List<CouponScope> couponScopes { get; set; }
    [JsonPropertyName("lineItemPrice")] public LineItemPrice lineItemPrice { get; set; }
    [JsonPropertyName("url")] public Url url { get; set; }
    [JsonPropertyName("price")] public Price price { get; set; }
    [JsonPropertyName("totalPriceBeforeTax")] public Price totalPriceBeforeTax { get; set; }
    [JsonPropertyName("availability")] public Availability availability { get; set; }
    [JsonPropertyName("totalPriceAfterTax")] public Price totalPriceAfterTax { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public Price priceBeforeDiscounts { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("fullPrice")] public Price fullPrice { get; set; }
    [JsonPropertyName("itemType")] public ItemType itemType { get; set; }
    [JsonPropertyName("rootCatalogItemId")] public string rootCatalogItemId { get; set; }
    [JsonPropertyName("taxDetails")] public TaxDetails taxDetails { get; set; }
    [JsonPropertyName("media")] public Media media { get; set; }
    [JsonPropertyName("productName")] public ProductName productName { get; set; }
    [JsonPropertyName("descriptionLines")] public List<DescriptionLine> descriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference catalogReference { get; set; }
    [JsonPropertyName("discount")] public Discount discount { get; set; }
}

public class PhysicalProperties_checkout_completed
{
    [JsonPropertyName("sku")] public string sku { get; set; }
    [JsonPropertyName("shippable")] public bool shippable { get; set; }
}

public class CouponScope_checkout_completed
{
    [JsonPropertyName("namespace")] public string @namespace { get; set; }
    [JsonPropertyName("group")] public Group group { get; set; }
}

public class Group_checkout_completed
{
    [JsonPropertyName("name")] public string name { get; set; }
    [JsonPropertyName("entityId")] public string entityId { get; set; }
}

public class LineItemPrice
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Url_checkout_completed
{
    [JsonPropertyName("relativePath")] public string relativePath { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
}

public class Price_checkout_completed
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Availability_checkout_completed
{
    [JsonPropertyName("status")] public string status { get; set; }
    [JsonPropertyName("quantityAvailable")] public int? quantityAvailable { get; set; }
}

public class TaxDetails_checkout_completed
{
    [JsonPropertyName("taxableAmount")] public Price taxableAmount { get; set; }
    [JsonPropertyName("taxRate")] public string taxRate { get; set; }
    [JsonPropertyName("totalTax")] public Price totalTax { get; set; }
    [JsonPropertyName("rateBreakdown")] public List<object> rateBreakdown { get; set; }
}

public class Media_checkout_completed
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
    [JsonPropertyName("height")] public int height { get; set; }
    [JsonPropertyName("width")] public int width { get; set; }
}

public class ProductName_checkout_completed
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class DescriptionLine_checkout_completed
{
    [JsonPropertyName("name")] public Name name { get; set; }
    [JsonPropertyName("plainText")] public PlainText plainText { get; set; }
    [JsonPropertyName("lineType")] public string lineType { get; set; }
    [JsonPropertyName("plainTextValue")] public PlainTextValue plainTextValue { get; set; }
    [JsonPropertyName("colorInfo")] public ColorInfo colorInfo { get; set; }
    [JsonPropertyName("color")] public string color { get; set; }
}

public class Name_checkout_completed
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainText_checkout_completed
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainTextValue
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class ColorInfo_checkout_completed
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
}

public class CatalogReference_checkout_completed
{
    [JsonPropertyName("catalogItemId")] public string catalogItemId { get; set; }
    [JsonPropertyName("appId")] public string appId { get; set; }
    [JsonPropertyName("options")] public Options options { get; set; }
}

public class Options_checkout_completed
{
    [JsonPropertyName("options")] public Dictionary<string, string> options { get; set; }
    [JsonPropertyName("variantId")] public string variantId { get; set; }
}

public class Discount_checkout_completed
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class ShippingDestination_checkout_completed
{
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails contactDetails { get; set; }
}

public class Address_checkout_completed
{
    [JsonPropertyName("city")] public string city { get; set; }
    [JsonPropertyName("countryFullname")] public string countryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string subdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string addressLine { get; set; }
    [JsonPropertyName("country")] public string country { get; set; }
    [JsonPropertyName("postalCode")] public string postalCode { get; set; }
    [JsonPropertyName("subdivision")] public string subdivision { get; set; }
}

public class ContactDetails_checkout_completed
{
    [JsonPropertyName("firstName")] public string firstName { get; set; }
    [JsonPropertyName("lastName")] public string lastName { get; set; }
    [JsonPropertyName("phone")] public string phone { get; set; }
}

public class SelectedCarrierServiceOption
{
    [JsonPropertyName("requestedShippingOption")] public bool requestedShippingOption { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
    [JsonPropertyName("cost")] public Price cost { get; set; }
    [JsonPropertyName("logistics")] public Logistics logistics { get; set; }
    [JsonPropertyName("carrierId")] public string carrierId { get; set; }
    [JsonPropertyName("otherCharges")] public List<object> otherCharges { get; set; }
    [JsonPropertyName("title")] public string title { get; set; }
}

public class Logistics_checkout_completed
{
    [JsonPropertyName("deliveryTime")] public string deliveryTime { get; set; }
}

public class Region_checkout_completed
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("name")] public string name { get; set; }
}

public class CarrierServiceOption
{
    [JsonPropertyName("carrierId")] public string carrierId { get; set; }
    [JsonPropertyName("shippingOptions")] public List<ShippingOption> shippingOptions { get; set; }
}

public class ShippingOption
{
    [JsonPropertyName("code")] public string code { get; set; }
    [JsonPropertyName("title")] public string title { get; set; }
    [JsonPropertyName("logistics")] public Logistics logistics { get; set; }
    [JsonPropertyName("cost")] public Price cost { get; set; }
}

public class AppliedDiscount_checkout_completed
{
    [JsonPropertyName("discountType")] public string discountType { get; set; }
    [JsonPropertyName("lineItemIds")] public List<object> lineItemIds { get; set; }
    [JsonPropertyName("coupon")] public Coupon coupon { get; set; }
}

public class Coupon_checkout_completed
{
    [JsonPropertyName("name")] public string name { get; set; }
    [JsonPropertyName("amount")] public Price amount { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("couponType")] public string couponType { get; set; }
}

public class BillingInfo_checkout_completed
{
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails contactDetails { get; set; }
}

public class BuyerInfo_checkout_completed
{
    [JsonPropertyName("contactId")] public string contactId { get; set; }
    [JsonPropertyName("email")] public string email { get; set; }
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class PriceSummary_checkout_completed
{
    [JsonPropertyName("additionalFees")] public Price additionalFees { get; set; }
    [JsonPropertyName("tax")] public Price tax { get; set; }
    [JsonPropertyName("total")] public Price total { get; set; }
    [JsonPropertyName("subtotal")] public Price subtotal { get; set; }
    [JsonPropertyName("discount")] public Price discount { get; set; }
    [JsonPropertyName("shipping")] public Price shipping { get; set; }
}

public class PayNowTotalAfterGiftCard
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class PayLater
{
    [JsonPropertyName("additionalFees")] public Price additionalFees { get; set; }
    [JsonPropertyName("tax")] public Price tax { get; set; }
    [JsonPropertyName("total")] public Price total { get; set; }
    [JsonPropertyName("subtotal")] public Price subtotal { get; set; }
    [JsonPropertyName("discount")] public Price discount { get; set; }
    [JsonPropertyName("shipping")] public Price shipping { get; set; }
}

public class CreatedBy_checkout_completed
{
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class CalculationErrors
{
    [JsonPropertyName("orderValidationErrors")] public List<object> orderValidationErrors { get; set; }
}

public class CheckoutCompletedEventData
{
    [JsonPropertyName("lineItems")] public List<LineItem> lineItems { get; set; }
    [JsonPropertyName("siteLanguage")] public string siteLanguage { get; set; }
    [JsonPropertyName("calculationErrors")] public CalculationErrors calculationErrors { get; set; }
    [JsonPropertyName("shippingInfo")] public ShippingInfo shippingInfo { get; set; }
    [JsonPropertyName("channelType")] public string channelType { get; set; }
    [JsonPropertyName("appliedDiscounts")] public List<AppliedDiscount> appliedDiscounts { get; set; }
    [JsonPropertyName("customFields")] public List<object> customFields { get; set; }
    [JsonPropertyName("weightUnit")] public string weightUnit { get; set; }
    [JsonPropertyName("priceSummary")] public PriceSummary priceSummary { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("completed")] public bool completed { get; set; }
    [JsonPropertyName("billingInfo")] public BillingInfo billingInfo { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo buyerInfo { get; set; }
    [JsonPropertyName("payNowTotalAfterGiftCard")] public PayNowTotalAfterGiftCard payNowTotalAfterGiftCard { get; set; }
    [JsonPropertyName("payLater")] public PayLater payLater { get; set; }
    [JsonPropertyName("updatedDate")] public DateTime updatedDate { get; set; }
    [JsonPropertyName("conversionCurrency")] public string conversionCurrency { get; set; }
    [JsonPropertyName("buyerLanguage")] public string buyerLanguage { get; set; }
    [JsonPropertyName("createdDate")] public DateTime createdDate { get; set; }
}

public class CheckoutCompletedEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
    [JsonPropertyName("eventData")] public CheckoutCompletedEventData eventData { get; set; }
}