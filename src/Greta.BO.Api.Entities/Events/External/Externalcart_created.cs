using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalcart_created : INotification
{
    [JsonPropertyName("instansceId")] public string InstansceId { get; set; }
    [JsonPropertyName("physicalProperties")] public PhysicalProperties physicalProperties { get; set; }
    [JsonPropertyName("quantity")] public int quantity { get; set; }
    [JsonPropertyName("paymentOption")] public string paymentOption { get; set; }
    [JsonPropertyName("couponScopes")] public List<CouponScope> couponScopes { get; set; }
    [JsonPropertyName("url")] public Url url { get; set; }
    [JsonPropertyName("image")] public Image image { get; set; }
    [JsonPropertyName("price")] public Price price { get; set; }
    [JsonPropertyName("availability")] public Availability availability { get; set; }
    [JsonPropertyName("priceBeforeDiscounts")] public PriceBeforeDiscounts priceBeforeDiscounts { get; set; }
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("fullPrice")] public FullPrice fullPrice { get; set; }
    [JsonPropertyName("itemType")] public ItemType itemType { get; set; }
    [JsonPropertyName("productName")] public ProductName productName { get; set; }
    [JsonPropertyName("descriptionLines")] public List<DescriptionLine> descriptionLines { get; set; }
    [JsonPropertyName("catalogReference")] public CatalogReference catalogReference { get; set; }
}

public class PhysicalProperties_cart_created
{
    [JsonPropertyName("sku")] public string sku { get; set; }
    [JsonPropertyName("shippable")] public bool shippable { get; set; }
}

public class CouponScope_cart_created
{
    [JsonPropertyName("namespace")] public string @namespace { get; set; }
    [JsonPropertyName("group")] public Group group { get; set; }
}

public class Group_cart_created
{
    [JsonPropertyName("name")] public string name { get; set; }
    [JsonPropertyName("entityId")] public string entityId { get; set; }
}

public class Url_cart_created
{
    [JsonPropertyName("relativePath")] public string relativePath { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
}

public class Image_cart_created
{
    [JsonPropertyName("id")] public string id { get; set; }
    [JsonPropertyName("url")] public string url { get; set; }
    [JsonPropertyName("height")] public int height { get; set; }
    [JsonPropertyName("width")] public int width { get; set; }
}

public class Price_cart_created
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Availability_cart_created
{
    [JsonPropertyName("status")] public string status { get; set; }
    [JsonPropertyName("quantityAvailable")] public int quantityAvailable { get; set; }
}

public class PriceBeforeDiscounts_cart_created
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class FullPrice_cart_created
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class ItemType_cart_created
{
    [JsonPropertyName("preset")] public string preset { get; set; }
}

public class ProductName_cart_created
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class DescriptionLine_cart_created
{
    [JsonPropertyName("name")] public Name name { get; set; }
    [JsonPropertyName("colorInfo")] public ColorInfo colorInfo { get; set; }
    [JsonPropertyName("lineType")] public string lineType { get; set; }
}

public class Name_cart_created
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
}

public class ColorInfo_cart_created
{
    [JsonPropertyName("original")] public string original { get; set; }
    [JsonPropertyName("translated")] public string translated { get; set; }
    [JsonPropertyName("code")] public string code { get; set; }
}

public class CatalogReference_cart_created
{
    [JsonPropertyName("catalogItemId")] public string catalogItemId { get; set; }
    [JsonPropertyName("appId")] public string appId { get; set; }
    [JsonPropertyName("options")] public Options options { get; set; }
}

public class Options_cart_created
{
    [JsonPropertyName("variantId")] public string variantId { get; set; }
}

public class BuyerInfo_cart_created
{
    [JsonPropertyName("visitorId")] public string visitorId { get; set; }
}

public class Subtotal_cart_created
{
    [JsonPropertyName("amount")] public string amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string convertedAmount { get; set; }
    [JsonPropertyName("formattedAmount")] public string formattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string formattedConvertedAmount { get; set; }
}

public class Entity
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

public class CreatedEvent
{
    [JsonPropertyName("entity")] public Entity entity { get; set; }
}

public class Root_cart_created
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