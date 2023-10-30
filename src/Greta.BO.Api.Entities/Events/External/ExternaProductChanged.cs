using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalProductChanged : INotification
{
    [JsonPropertyName("productId")] public string ProductId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("priceData")] public PriceDataUpdate PriceData { get; set; }
    [JsonPropertyName("visible")] public bool Visible { get; set; }
    [JsonPropertyName("media")] public MediaDataProdUpdate Media { get; set; }
    [JsonPropertyName("sku")] public string Sku { get; set; }
    [JsonPropertyName("productPageUrl")] public ProductPageUrlUpdate ProductPageUrl { get; set; }
}

public class PriceDataUpdate
{
    [JsonPropertyName("currency")] public string Currency { get; set; }
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("discountedPrice")] public decimal DiscountedPrice { get; set; }
    [JsonPropertyName("formatted")] public FormattedDataUpdate Formatted { get; set; }
}

public class FormattedDataUpdate
{
    [JsonPropertyName("price")] public string Price { get; set; }
    [JsonPropertyName("discountedPrice")] public string DiscountedPrice { get; set; }
}

public class MediaDataProdUpdate
{
    [JsonPropertyName("mainMedia")] public MainMediaProUpdate MainMedia { get; set; }
    [JsonPropertyName("items")] public ItemProUpdate[] Items { get; set; }
}

public class MainMediaProUpdate
{
    [JsonPropertyName("thumbnail")] public ThumbnailProUpdate Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public ImageProUpdate Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class ThumbnailProUpdate
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ImageProUpdate
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ItemProUpdate
{
    [JsonPropertyName("thumbnail")] public Thumbnail Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class ProductPageUrlUpdate
{
    [JsonPropertyName("base")] public string Base { get; set; }
    [JsonPropertyName("path")] public string Path { get; set; }
}
