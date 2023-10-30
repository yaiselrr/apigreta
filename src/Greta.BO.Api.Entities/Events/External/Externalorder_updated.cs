using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalorder_updated : INotification
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("entityFqdn")] public string entityFqdn { get; set; }
    [JsonPropertyName("slug")] public string slug { get; set; }
    [JsonPropertyName("entityId")] public string entityId { get; set; }
    [JsonPropertyName("eventTime")] public DateTime eventTime { get; set; }
    [JsonPropertyName("updatedEvent")] public UpdatedEvent updatedEvent { get; set; }
    [JsonPropertyName("triggeredByAnonymizeRequest")] public bool triggeredByAnonymizeRequest { get; set; }
}

public class UpdatedEvent
{
    [JsonPropertyName("currentEntity")] public CurrentEntity currentEntity { get; set; }
}

public class CurrentEntityorder_updated
{
    [JsonPropertyName("number")] public string number { get; set; }
    [JsonPropertyName("additionalFees")] public List<object> additionalFees { get; set; }
    [JsonPropertyName("cartId")] public string cartId { get; set; }
    [JsonPropertyName("lineItems")] public List<LineItem> lineItems { get; set; }
    [JsonPropertyName("paymentStatus")] public string paymentStatus { get; set; }
    [JsonPropertyName("archived")] public bool archived { get; set; }
    [JsonPropertyName("seenByAHuman")] public bool seenByAHuman { get; set; }
    [JsonPropertyName("activities")] public List<Activity> activities { get; set; }
    [JsonPropertyName("appliedDiscounts")] public List<object> appliedDiscounts { get; set; }
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

public class LineItemorder_updated
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

public class PhysicalPropertiesorder_updated
{
    [JsonPropertyName("shippable")] public bool shippable { get; set; }
}

public class Imageorder_updated
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
    [JsonPropertyName("height")] public int height { get; set; }
    [JsonPropertyName("width")] public int width { get; set; }
}

public class Priceorder_updated
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
}

public class ItemTypeorder_updated
{
    [JsonPropertyName("preset")] public string preset { get; set; }
}

public class TaxDetailsorder_updated
{
    [JsonPropertyName("taxableAmount")] public Price taxableAmount { get; set; }
    [JsonPropertyName("taxRate")] public string taxRate { get; set; }
    [JsonPropertyName("totalTax")] public Price totalTax { get; set; }
}

public class ProductNameorder_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class DescriptionLineorder_updated
{
    [JsonPropertyName("name")] public Name name { get; set; }
    [JsonPropertyName("plainText")] public PlainText plainText { get; set; }
    [JsonPropertyName("lineType")] public string lineType { get; set; }
    [JsonPropertyName("plainTextValue")] public PlainTextValue plainTextValue { get; set; }
}

public class Nameorder_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainTextorder_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainTextValueorder_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class CatalogReferenceorder_updated
{
    [JsonPropertyName("catalogItemId")] public string catalogItemId { get; set; }
    [JsonPropertyName("appId")] public string appId { get; set; }
    [JsonPropertyName("options")] public Options options { get; set; }
}

public class Optionsorder_updated
{
    [JsonPropertyName("currency")] public string currency { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("variantId")] public string variantId { get; set; }
}

public class Activityorder_updated
{
    [JsonPropertyName("createdDate")] public DateTime createdDate { get; set; }
    [JsonPropertyName("type")] public string type { get; set; }
}

public class PriceSummaryorder_updated
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

public class BillingInfoorder_updated
{
    [JsonPropertyName("address")] public Address address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails contactDetails { get; set; }
}

public class Addressorder_updated
{
    [JsonPropertyName("city")] public string city { get; set; }
    [JsonPropertyName("countryFullname")] public string countryFullname { get; set; }
    [JsonPropertyName("subdivisionFullname")] public string subdivisionFullname { get; set; }
    [JsonPropertyName("addressLine")] public string addressLine { get; set; }
    [JsonPropertyName("country")] public string country { get; set; }
    [JsonPropertyName("postalCode")] public string postalCode { get; set; }
    [JsonPropertyName("subdivision")] public string subdivision { get; set; }
}

public class ContactDetailsorder_updated
{
    [JsonPropertyName("firstName")] public string firstName { get; set; }
    [JsonPropertyName("lastName")] public string lastName { get; set; }
    [JsonPropertyName("phone")] public string phone { get; set; }
}

public class BuyerInfoorder_updated
{
    [JsonPropertyName("contactId")] public string contactId { get; set; }
    [JsonPropertyName("email")] public string email { get; set; }
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class PayNoworder_updated
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