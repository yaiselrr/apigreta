using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class OnlineWixCatCreateResponse
{
    [JsonPropertyName("collection")]
    public OnlineWixCategory OnlineWixCategory { get; set; }
}

public class OnlineWixCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("slug")]
    public string Slug { get; set; }
    [JsonPropertyName("visible")]
    public bool Visible { get; set; }
    [JsonPropertyName("numberOfProducts")]
    public int NumberOfProducts { get; set; }
    [JsonPropertyName("media")]
    public WixMedia Media { get; set; }
}

/// <summary>
/// Importan define the type of Items on media
/// </summary>
public class WixMedia
{
    [JsonPropertyName("items")]
    public object[] Items { get; set; }
}

public class OnlineWixGetResponse
{
    [JsonPropertyName("collection")]
    public List<OnlineWixCategory> OnlineWixCategories { get; set; }
}