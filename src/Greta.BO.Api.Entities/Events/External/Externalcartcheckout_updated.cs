using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalcheckout_updated : INotification
{
    [JsonPropertyName("sku")] public string sku { get; set; }
    [JsonPropertyName("shippable")] public bool shippable { get; set; }
}

public class Url_checkout_updated
{
    [JsonPropertyName("relativePath")] public string relativePath { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
}

public class Price_checkout_updated
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Availability_checkout_updated
{
    [JsonPropertyName("status")] public string status { get; set; }
    [JsonPropertyName("quantityAvailable")] public int? quantityAvailable { get; set; }
}

public class PriceBeforeDiscounts_checkout_updated
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Media
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
    [JsonPropertyName("height")] public int? height { get; set; }
    [JsonPropertyName("width")] public int? width { get; set; }
}

public class Original
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class PlainText_checkout_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class Name_checkout_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class ColorInfo_checkout_updated
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
}

public class DescriptionLine_checkout_updated
{
    [JsonPropertyName("name")] public Name name { get; set; }
    [JsonPropertyName("colorInfo")] public ColorInfo colorInfo { get; set; }
    [JsonPropertyName("lineType")] public string lineType { get; set; }
}

public class Options_checkout_updated
{
    [JsonPropertyName("options")] public Dictionary<string, string> options { get; set; }
    [JsonPropertyName("variantId")] public string variantId { get; set; }
}

public class CatalogReference_checkout_updated
{
    [JsonPropertyName("catalogItemId")] public string catalogItemId { get; set; }
    [JsonPropertyName("appId")] public string appId { get; set; }
    [JsonPropertyName("options")] public Options options { get; set; }
}

public class LineItem_checkout_updated
{
    [JsonPropertyName("physicalProperties")] public PhysicalProperties physicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string paymentOption { get; set; }
    [JsonPropertyName("couponScopes")] public List<object> couponScopes { get; set; }
    [JsonPropertyName("url")] public Url url { get; set; }
    [JsonPropertyName("price")] public Price price { get; set; }
    [JsonPropertyName("availability")] public Availability availability { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public PriceBeforeDiscounts priceBeforeDiscounts { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("fullPrice")] public Price fullPrice { get; set; }
    [JsonPropertyName("itemType")] public Dictionary<string, object> itemType { get; set; }
    [JsonPropertyName("productName")] public Original productName { get; set; }
    [JsonPropertyName("descriptionLines")] public List<DescriptionLine> descriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference catalogReference { get; set; }
}

public class BuyerInfo_checkout_updated
{
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class Subtotal_checkout_updated
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Entity_checkout_updated
{
    [JsonPropertyName("lineItems")] public List<LineItem> lineItems { get; set; }
    [JsonPropertyName("siteLanguage")] public string siteLanguage { get; set; }
    [JsonPropertyName("appliedDiscounts")] public List<object> appliedDiscounts { get; set; }
    [JsonPropertyName("taxIncludedInPrices")] public bool taxIncludedInPrices { get; set; }
    [JsonPropertyName("weightUnit")] public string weightUnit { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo buyerInfo { get; set; }
    [JsonPropertyName("currency")] public string currency { get; set; }
    [JsonPropertyName("subtotal")] public Subtotal subtotal { get; set; }
    [JsonPropertyName("updatedDate")] public DateTime updatedDate { get; set; }
    [JsonPropertyName("conversionCurrency")] public string conversionCurrency { get; set; }
    [JsonPropertyName("buyerLanguage")] public string buyerLanguage { get; set; }
    [JsonPropertyName("createdDate")] public DateTime createdDate { get; set; }
}

public class CreatedEvent_checkout_updated
{
    [JsonPropertyName("entity")] public Entity entity { get; set; }
}

public class Root_checkout_updated
{
    [JsonPropertyName("entityId")] public string entityId { get; set; }
    [JsonPropertyName("entityEventSequence")] public string entityEventSequence { get; set; }
    [JsonPropertyName("slug")] public string slug { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("createdEvent")] public CreatedEvent createdEvent { get; set; }
    [JsonPropertyName("entityFqdn")] public string entityFqdn { get; set; }
    [JsonPropertyName("eventTime")] public DateTime eventTime { get; set; }
    [JsonPropertyName("triggeredByAnonymizeRequest")] public bool triggeredByAnonymizeRequest { get; set; }
}

public class CartCreatedEventData
{
    [JsonPropertyName("root")] public Root root { get; set; }
}

public class CartCreatedEvent
{
    [JsonPropertyName("data")] public string data { get; set; }
    [JsonPropertyName("instanceId")] public string instanceId { get; set; }
    [JsonPropertyName("eventType")] public string eventType { get; set; }
    [JsonPropertyName("eventData")] public CartCreatedEventData eventData { get; set; }
}