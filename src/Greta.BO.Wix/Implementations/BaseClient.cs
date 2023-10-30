using System.Text;
using System.Text.Json;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Implementations;

public class BaseClient: IBaseClient
{
    protected readonly WixOptions options;
    
    private DateTime _expirationTime;
    private readonly int _expitationMinutes = 3; //1440;
    protected string? Token { get; set; }

    public BaseClient(WixOptions options)
    {
        this.options = options;
    }
    
    public async ValueTask<Parameter> RefreshAccess(string refreshToken)
    {

        if (Token == null || DateTime.Now > _expirationTime)
        {
            await GetToken(refreshToken);
        }
        //request.AddHeader("Authorization", "Bearer " + _accessToken);
        return await GetAuthenticationParameter(refreshToken).ConfigureAwait(false);

    }
    public async Task<string?> GetRefreshTokenByCode(string token)
    {
        return await GetRefreshToken(token);
    }

    private async ValueTask<Parameter> GetAuthenticationParameter(string refreshToken)
    {
        Token = string.IsNullOrEmpty(Token) ? await GetToken(refreshToken) : Token;
        return new HeaderParameter(KnownHeaders.Authorization, Token);
    }
    
    private async Task<string?> GetToken(string refreshToken)
    {
        var httpClient = new HttpClient();
            
        var postData = new
        {
            grant_type= "refresh_token",
            client_id= options.ClientId,
            client_secret= options.ClientSecret,
            refresh_token=  refreshToken
        };
        var json = JsonSerializer.Serialize(postData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{options.Url}/oauth/access", content);

        var data = await response.Content.ReadAsStringAsync();

        var access = JsonSerializer.Deserialize<RefreshTokenResponse>(data);

        return access?.AccessToken;
    }

    private async Task<string?> GetRefreshToken(string token)
    {
        var httpClient = new HttpClient();
            
        var postData = new
        {
            grant_type= "authorization_code",
            client_id= options.ClientId,
            client_secret= options.ClientSecret,
            code=  token
        };
        var json = JsonSerializer.Serialize(postData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{options.Url}/oauth/access", content);

        var data = await response.Content.ReadAsStringAsync();

        var access = JsonSerializer.Deserialize<RefreshTokenResponse>(data);

        return access?.RefreshToken;
    }
}