using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class WixWebHookData
{
    [JsonPropertyName("data")]
    public string Data { get; set; }
    [JsonPropertyName("instanceId")]
    public string InstanceId { get; set; }
    [JsonPropertyName("eventType")]
    public string EventType { get; set; }
}