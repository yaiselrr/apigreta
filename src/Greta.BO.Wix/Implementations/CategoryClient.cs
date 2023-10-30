using System.Net;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Extentions;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Implementations;

public class CategoryClient : BaseClient, ICategoryClient
{
    private readonly RestClient _client;

    public CategoryClient(RestClient client, WixOptions options) : base(options)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<string> Create(string refreshToken, LiteCategory category)
    {
        var request = new RestRequest($"/stores/v1/collections").AddJsonBody(new
        {
            collection = new
            {
                name = category.Name,
                description = category.Description,
                slug = category.Name.Slugify(),
                visible = true
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixCatCreateResponse>(request);

        return response == null ? "" : response.OnlineWixCategory.Id;
    }

    public async Task<bool> Update(string refreshToken, string id, LiteCategory category)
    {
        var request = new RestRequest($"/stores/v1/collections/{id}").AddJsonBody(new
        {
            collection = new
            {
                name = category.Name,
                description = category.Name,
                slug = category.Name.Slugify(),
                visible = category.AddOnlineStore
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PatchAsync<OnlineWixCatCreateResponse>(request);

        return response != null;
    }

    public async Task<bool> Delete(string refreshToken, string id)
    {
        var request = new RestRequest($"/stores/v1/collections/{id}");

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.DeleteAsync(request);
        
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<List<OnlineWixCategory>> Get(string refreshToken, bool includeNumberOfProducts, bool includeDescription, bool includeMedia)
    {
        var request = new RestRequest($"/stores/v1/collections").AddJsonBody(new
        {
            // query = new {},
            includeNumberOfProducts = true,
            includeDescription = true
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixGetResponse>(request);

        return response?.OnlineWixCategories?.ToList() ?? new List<OnlineWixCategory>();
    }

    public async Task<OnlineWixCategory> GetCollectionById(string refreshToken, string id)
    {
        var request = new RestRequest($"/stores/v1/collections/{id}").AddJsonBody(new
        {
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixCatCreateResponse>(request);

        return response.OnlineWixCategory;
    }

    public async Task<bool> AddProductsToCollection(string refreshToken, string id, List<string> productIds)
    {
        var request = new RestRequest($"/stores/v1/collections/{id}/productIds").AddJsonBody(new
        {
            productIds = productIds
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync(request);

        return response.StatusCode == HttpStatusCode.OK;
    }
    public async Task<bool> RemoveProductsFromCollection(string refreshToken, string id, List<string> productIds)
    {
        var request = new RestRequest($"/stores/v1/collections/{id}/productIds/delete").AddJsonBody(new
        {
            productIds = productIds
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync(request);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
