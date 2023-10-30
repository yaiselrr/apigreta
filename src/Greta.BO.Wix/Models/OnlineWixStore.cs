using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class OnlineWixStoreResponse
{
    [JsonPropertyName("instance")]
    public InstanceData Instance { get; set; }

    [JsonPropertyName("site")]
    public SiteData Site { get; set; }
}

public class InstanceData
{
    [JsonPropertyName("appName")]
    public string AppName { get; set; }

    [JsonPropertyName("appVersion")]
    public string AppVersion { get; set; }

    [JsonPropertyName("billing")]
    public BillingData Billing { get; set; }

    [JsonPropertyName("instanceId")]
    public string InstanceId { get; set; }

    [JsonPropertyName("isFree")]
    public bool IsFree { get; set; }

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; }
}

public class BillingData
{
    [JsonPropertyName("billingCycle")]
    public string BillingCycle { get; set; }

    [JsonPropertyName("packageName")]
    public string PackageName { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }
}

public class SiteData
{
    [JsonPropertyName("locale")]
    public string Locale { get; set; }

    [JsonPropertyName("multilingual")]
    public MultilingualData Multilingual { get; set; }

    [JsonPropertyName("paymentCurrency")]
    public string PaymentCurrency { get; set; }

    [JsonPropertyName("siteDisplayName")]
    public string SiteDisplayName { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("ownerEmail")]
    public string OwnerEmail { get; set; }

    [JsonPropertyName("ownerInfo")]
    public OwnerInfoData OwnerInfo { get; set; }

    [JsonPropertyName("siteId")]
    public string SiteId { get; set; }
}

public class MultilingualData
{
    [JsonPropertyName("isMultiLingual")]
    public bool IsMultiLingual { get; set; }

    [JsonPropertyName("supportedLanguages")]
    public List<string> SupportedLanguages { get; set; }
}

public class OwnerInfoData
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("emailStatus")]
    public string EmailStatus { get; set; }
}