using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class OnlineWixMediaResponse
{
    [JsonPropertyName("file")]
    public OnlineWixMedia OnlineWixMedia { get; set; }
}

public class OnlineWixMedia
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("parentFolderId")]
    public string ParentFolderId { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    // [JsonPropertyName("sizeInBytes")]
    // [JsonConverter(typeof(ParseStringConverter))]
    // public long SizeInBytes { get; set; }

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("media")]
    public object Media { get; set; }

    [JsonPropertyName("operationStatus")]
    public string OperationStatus { get; set; }

    [JsonPropertyName("sourceUrl")]
    public Uri SourceUrl { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public Uri ThumbnailUrl { get; set; }

    [JsonPropertyName("labels")]
    public object[] Labels { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTimeOffset CreatedDate { get; set; }

    [JsonPropertyName("updatedDate")]
    public DateTimeOffset UpdatedDate { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}


public class OnlineWixGetMediaResponse
{
    [JsonPropertyName("media")]
    public List<OnlineWixMedia> OnlineWixMedia { get; set; }
}