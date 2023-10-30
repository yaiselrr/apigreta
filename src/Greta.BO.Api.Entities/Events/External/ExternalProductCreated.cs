using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalProductCreated : INotification
{
    [JsonPropertyName("productId")] public string ProductId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("priceData")] public PriceData PriceData { get; set; }
    [JsonPropertyName("visible")] public bool Visible { get; set; }
    [JsonPropertyName("media")] public MediaDataProd Media { get; set; }
    [JsonPropertyName("sku")] public string Sku { get; set; }
    [JsonPropertyName("productPageUrl")] public ProductPageUrl ProductPageUrl { get; set; }
}

public class PriceData
{
    [JsonPropertyName("currency")] public string Currency { get; set; }
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("discountedPrice")] public decimal DiscountedPrice { get; set; }
    [JsonPropertyName("formatted")] public FormattedData Formatted { get; set; }
}

public class FormattedData
{
    [JsonPropertyName("price")] public string Price { get; set; }
    [JsonPropertyName("discountedPrice")] public string DiscountedPrice { get; set; }
}

public class MediaDataProd
{
    [JsonPropertyName("mainMedia")] public MainMediaPro MainMedia { get; set; }
    [JsonPropertyName("items")] public ItemPro[] Items { get; set; }
}

public class MainMediaPro
{
    [JsonPropertyName("thumbnail")] public ThumbnailPro Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public ImagePro Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class ThumbnailPro
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ImagePro
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ItemPro
{
    [JsonPropertyName("thumbnail")] public Thumbnail Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class ProductPageUrl
{
    [JsonPropertyName("base")] public string Base { get; set; }
    [JsonPropertyName("path")] public string Path { get; set; }
}
