using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class Result
{
    [JsonPropertyName("itemMetadata")]
    public ItemMetadata ItemMetadata { get; set; }
}
public class ItemMetadata
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("originalIndex")]
    public string OriginalIndex { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("error")]
    public Error Error { get; set; }
}

public class Error
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Root
{
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; }

    [JsonPropertyName("bulkActionMetadata")]
    public BulkActionMetadata BulkActionMetadata { get; set; }
}



public class BulkActionMetadata
{
    [JsonPropertyName("totalSuccesses")]
    public int TotalSuccesses { get; set; }

    [JsonPropertyName("totalFailures")]
    public int TotalFailures { get; set; }
}