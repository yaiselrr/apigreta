using System.Text.Json.Serialization;

namespace Greta.BO.Wix.Models;

public class RefreshTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

}