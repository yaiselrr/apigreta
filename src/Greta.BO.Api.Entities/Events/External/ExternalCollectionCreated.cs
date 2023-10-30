using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCollectionCreated : INotification
{
    [JsonPropertyName("collectionId")] public string CollectionId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("media")] public MediaData Media { get; set; }
}

public class MediaData
{
    [JsonPropertyName("mainMedia")] public MainMedia MainMedia { get; set; }
    [JsonPropertyName("items")] public Item[] Items { get; set; }
}

public class MainMedia
{
    [JsonPropertyName("thumbnail")] public Thumbnail Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}

public class Thumbnail
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class Image
{
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}

public class Item
{
    [JsonPropertyName("thumbnail")] public Thumbnail Thumbnail { get; set; }
    [JsonPropertyName("mediaType")] public string MediaType { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("image")] public Image Image { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}