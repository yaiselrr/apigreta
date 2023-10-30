using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCollectionChanged : INotification
{
    [JsonPropertyName("collectionId")] public string CollectionId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("media")] public MediaDataUpdate Media { get; set; }
}

public class MediaDataUpdate
{
    [JsonPropertyName("mainMedia")] public MainMediaUpdate MainMedia { get; set; }
    [JsonPropertyName("items")] public ItemUpdate[] Items { get; set; }
}

public class MainMediaUpdate
{
    [JsonPropertyName("thumbnail")] public ThumbnailUpdate Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class ThumbnailUpdate
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ImageUpdate
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class ItemUpdate
{
    [JsonPropertyName("thumbnail")] public Thumbnail Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public ImageUpdate Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}