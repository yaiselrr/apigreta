using System.Net;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Implementations;

public class OnlineStoreClient : BaseClient, IOnlineStoreClient
{
    private readonly RestClient _client;

    public OnlineStoreClient(RestClient client, WixOptions options) : base(options)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<string> Create(string token)
    {
        var response = await GetRefreshTokenByCode(token);

        return response == null ? "" : response;
    }

    public async Task<bool> FinishesInstallation(string refreshToken)
    {
        var request = new RestRequest($"/apps/v1/bi-event").AddJsonBody(new
        {
            eventName = "APP_FINISHED_CONFIGURATION"
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync(request);

        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<OnlineWixStoreResponse> GetStore(string refreshToken)
    {
        var request = new RestRequest($"/apps/v1/instance");

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.GetAsync<OnlineWixStoreResponse>(request);

        return response!;
    }


    public async Task<Result> DeleteOnlineStoreWix(string refreshToken, List<string> siteInstance)
    {
        var request = new RestRequest($"/site-actions/v1/bulk/sites/delete").AddJsonBody(new
        {
            ids = siteInstance
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<Result>(request);

        return response;
    }

}
