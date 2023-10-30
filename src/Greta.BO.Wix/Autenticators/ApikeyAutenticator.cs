using System.Text;
using System.Text.Json;
using Greta.BO.Wix.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace Greta.BO.Wix.Autenticators;

public class ApikeyAutenticator: IAuthenticator
{
    private readonly WixOptions _options;
    private readonly string _baseUrl;
        private readonly string _clientId;
        private readonly string _clientScope;
        private readonly string _clientSecret;
        private string? _refreshToken;
        private DateTime _expirationTime;
        private readonly int _expitationMinutes = 3; //1440;
        protected string? Token { get; set; }

        public ApikeyAutenticator(
            WixOptions options,
            string baseUrl,
            string clientId,
            string clientScope,
            string clientSecret
            )
        {
            _options = options;
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientScope = clientScope;
            _clientSecret = clientSecret;
            _refreshToken = null;
        }

        public async ValueTask Authenticate(IRestClient client, RestRequest request)
        {
            if (Token == null || DateTime.Now > _expirationTime)
            {
                GetToken();
            }
            //request.AddHeader("Authorization", "Bearer " + _accessToken);
            request.AddOrUpdateParameter(await GetAuthenticationParameter(Token).ConfigureAwait(false));
        }
            
        protected async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }
        
        async Task<string> GetToken() {
            
            var httpClient = new HttpClient();
            
            var postData = new
            {
                grant_type= "refresh_token",
                client_id= _options.ClientId,
                client_secret= _options.ClientSecret,
                // refresh_token=  refreshToken
            };
            var json = JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{_options.Url}/oauth/access", content);

            var data = await response.Content.ReadAsStringAsync();

            var access = JsonSerializer.Deserialize<RefreshTokenResponse>(data);

            _expirationTime = DateTime.Now.AddMinutes(_expitationMinutes);
            
            return $"Bearer {access?.AccessToken}";
        }
}