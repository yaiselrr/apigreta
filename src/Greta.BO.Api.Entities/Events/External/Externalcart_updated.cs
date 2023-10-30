using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class Externalcart_updated : INotification
{
    [JsonPropertyName("data")] public string Data { get; set; }
    // [JsonPropertyName("iat")] public long Iat { get; set; }
    // [JsonPropertyName("exp")] public long Exp { get; set; }
}

public class CurrentEntity
{
    [JsonPropertyName("lineItems")] public LineItem[] LineItems { get; set; }

    // Site language in which original values are displayed.
    [JsonPropertyName("siteLanguage")] public string SiteLanguage { get; set; }

    // Cart discounts.
    [JsonPropertyName("appliedDiscounts")] public AppliedDiscount[] AppliedDiscounts { get; set; }

    // Whether tax is included in line item prices.
    [JsonPropertyName("taxIncludedInPrices")] public bool TaxIncludedInPrices { get; set; }

    // Weight measurement unit - defaults to site's weight unit.
    [JsonPropertyName("weightUnit")] public string WeightUnit { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("buyerInfo")] public BuyerInfo BuyerInfo { get; set; }

    // Currency used for pricing.
    [JsonPropertyName("currency")] public string Currency { get; set; }
    [JsonPropertyName("subtotal")] public Subtotal Subtotal { get; set; }

    // Date and time the cart was updated.
    [JsonPropertyName("updatedDate")] public DateTime UpdatedDate { get; set; }

    // Currency code used for all the converted prices that are returned.
    // For a site that supports multiple currencies, this is the currency the buyer selected.   
    [JsonPropertyName("conversionCurrency")] public string ConversionCurrency { get; set; }

    // Language for communication with the buyer. Defaults to the site language.
    // For a site that supports multiple languages, this is the language the buyer selected.
    [JsonPropertyName("buyerLanguage")] public string BuyerLanguage { get; set; }

    // Date and time the cart was created.
    [JsonPropertyName("createdDate")] public DateTime CreatedDate { get; set; }
}

public class LineItem_cart_updated
{
    // Physical properties of the item. When relevant, contains information such as SKU, item weight, and shippability.
    [JsonPropertyName("physicalProperties")] public PhysicalProperties PhysicalProperties { get; set; }

    // Item quantity.
    [JsonPropertyName("quantity")] public int Quantity { get; set; }

    // 5 supported values: FULL_PAYMENT_ONLINE, FULL_PAYMENT_OFFLINE, MEMBERSHIP, DEPOSIT_ONLINE, MEMBERSHIP_OFFLINE,
    [JsonPropertyName("paymentOption")] public string PaymentOption { get; set; }
    [JsonPropertyName("couponScopes")] public CouponScope[] CouponScopes { get; set; }

    // URL to the item's page on the site.
    [JsonPropertyName("url")] public Url Url { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("price")] public Price Price { get; set; }
    [JsonPropertyName("availability")] public Availability Availability { get; set; }

    // Item price before line item discounts and after catalog-defined discount. Defaults to price when not provided.
    [JsonPropertyName("priceBeforeDiscounts")] public PriceBeforeDiscounts PriceBeforeDiscounts { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("fullPrice")] public FullPrice FullPrice { get; set; }
    [JsonPropertyName("itemType")] public ItemType ItemType { get; set; }
    [JsonPropertyName("productName")] public ProductName ProductName { get; set; }

    // Line item description lines. Used for displaying the cart, checkout and order.
    [JsonPropertyName("descriptionLines")] public DescriptionLine[] DescriptionLines { get; set; }

    // Catalog and item reference. Holds IDs for the item and the catalog it came from, as well as further optional info.
    [JsonPropertyName("catalogReference")] public CatalogReference CatalogReference { get; set; }
}

// Physical properties of the item. When relevant, contains information such as SKU, item weight, and shippability.
public class PhysicalProperties_cart_updated
{
    // Stock-keeping unit. Learn more about SKUs.
    [JsonPropertyName("sku")] public string Sku { get; set; }

    // Whether this line item is shippable.
    [JsonPropertyName("shippable")] public bool Shippable { get; set; }
}

public class CouponScope
{
    [JsonPropertyName("namespace")] public string Namespace { get; set; }
    [JsonPropertyName("group")] public Group Group { get; set; }
}

public class Group
{
    [JsonPropertyName("name")] public string Name { get; set; }

    // Assuming that all messages including Actions have id
    // Example: The id of the specific order, the id of a specific campaign
    [JsonPropertyName("entityId")] public string EntityId { get; set; }
}

public class Url
{
    // The path to that page - e.g /product-page/a-product
    [JsonPropertyName("relativePath")] public string RelativePath { get; set; }
    // [JsonPropertyName("url")] public string Url { get; set; }
}

// Line item image details.
public class Image_cart_updated
{
    // WixMedia image ID.
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
}

// Item price after catalog-defined discount and line item discounts.
public class Price_cart_updated
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string ConvertedAmount { get; set; }

    // Amount formatted with currency symbol.
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }

    // Converted amount formatted with currency symbol.
    [JsonPropertyName("formattedConvertedAmount")] public string FormattedConvertedAmount { get; set; }
}

// Item availability details.
public class Availability
{
    // 4 supported values: AVAILABLE, NOT_FOUND, NOT_AVAILABLE, PARTIALLY_AVAILABLE,
    [JsonPropertyName("status")] public string Status { get; set; }

    // Quantity available.
    [JsonPropertyName("quantityAvailable")] public int? QuantityAvailable { get; set; }
}

// Item price before line item discounts and after catalog-defined discount. Defaults to price when not provided.
public class PriceBeforeDiscounts_cart_updated
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string ConvertedAmount { get; set; }

    // Converted amount formatted with currency symbol.
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }

    // Converted amount formatted with currency symbol.
    [JsonPropertyName("formattedConvertedAmount")] public string FormattedConvertedAmount { get; set; }
}

// Item price before catalog-defined discount. Defaults to when not provided.
public class FullPrice
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string ConvertedAmount { get; set; }

    // Amount formatted with currency symbol.
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }

    // Converted amount formatted with currency symbol.
    [JsonPropertyName("formattedConvertedAmount")] public string FormattedConvertedAmount { get; set; }
}

// Item type. Either a preset type or custom.
public class ItemType_cart_updated
{
    // 5 supported values: UNRECOGNISED, PHYSICAL, DIGITAL, GIFT_CARD, SERVICE,
    [JsonPropertyName("preset")] public string Preset { get; set; }
}

public class ProductName_cart_updated
{

    // Required. Original item name in site's default language.
    [JsonPropertyName("original")] public string Original { get; set; }

    // Optional. Translated item name according to buyer language. Defaults to original when not provided.
    [JsonPropertyName("translated")] public string Translated { get; set; }
}

// Line item description lines. Used for displaying the cart, checkout and order.
public class DescriptionLine
{
    // Description line name.
    [JsonPropertyName("name")] public Name Name { get; set; }
    [JsonPropertyName("plainText")] public PlainText PlainText { get; set; }
    [JsonPropertyName("lineType")] public string LineType { get; set; }
    [JsonPropertyName("colorInfo")] public ColorInfo ColorInfo { get; set; }
}

// Description line name.
public class Name
{
    // Description line name in site's default language.
    [JsonPropertyName("original")] public string Original { get; set; }

    // Translated description line item according to buyer language. Defaults to original when not provided.
    [JsonPropertyName("translated")] public string Translated { get; set; }
}

// Description line plain text value.
public class PlainText
{
    // Description line plain text value in site's default language.
    [JsonPropertyName("original")] public string Original { get; set; }

    // Translated description line plain text value according to buyer language. Defaults to original when not provided.
    [JsonPropertyName("translated")] public string Translated { get; set; }
}

// Description line color value.
public class ColorInfo
{
    // Description line color name in site's default language.
    [JsonPropertyName("original")] public string Original { get; set; }

    // Translated description line color name according to buyer language. Defaults to original when not provided.
    [JsonPropertyName("translated")] public string Translated { get; set; }

    // HEX or RGB color code for display.
    [JsonPropertyName("code")] public string Code { get; set; }
}

public class CatalogReference_cart_updated
{

    // ID of the item within its Wix or 3rd-party catalog. For example, productId for Wix Stores or bookingId for Wix Bookings.
    [JsonPropertyName("catalogItemId")] public string CatalogItemId { get; set; }

    // ID of the app providing the catalog. For items from Wix apps, the following values always apply:
    // Wix Stores: "215238eb-22a5-4c36-9e7b-e7c08025e04e"
    // Wix Bookings: "13d21c63-b5ec-5912-8397-c3a5ddb27a97"
    [JsonPropertyName("appId")] public string AppId { get; set; }

    // Additional info in key:value pairs. For example, to specify Wix Stores product options or variants:
    // {"options": {"options": {"Size": "M", "Color": "Red"}}}
    // {"options": {"variantId": "<VARIANT_ID>"}}
    [JsonPropertyName("options")] public Options Options { get; set; }
}

public class Options_cart_updated
{
    [JsonPropertyName("options")] public OptionsOptions Options { get; set; }
    [JsonPropertyName("variantId")] public string VariantId { get; set; }
}

public class OptionsOptions
{
}

// Buyer information.
public class BuyerInfo_cart_updated
{
    // Visitor ID - if the buyer is not a site member.
    [JsonPropertyName("visitorId")] public string VisitorId { get; set; }
}

public class Subtotal_cart_updated
{
    [JsonPropertyName("amount")] public string Amount { get; set; }
    [JsonPropertyName("convertedAmount")] public string ConvertedAmount { get; set; }

    // Converted amount formatted with currency symbol.
    [JsonPropertyName("formattedAmount")] public string FormattedAmount { get; set; }
    [JsonPropertyName("formattedConvertedAmount")] public string FormattedConvertedAmount { get; set; }
}
