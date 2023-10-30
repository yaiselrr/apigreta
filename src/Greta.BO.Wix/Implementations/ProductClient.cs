using System.Net;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Extentions;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Implementations;

public class ProductClient : BaseClient, IProductClient
{
    private readonly RestClient _client;

    public ProductClient(RestClient client, WixOptions options) : base(options)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<string> Create(string refreshToken, LiteProduct product)
    {
        var request = new RestRequest($"/stores/v1/products").AddJsonBody(new
        {
            product = new
            {
                name = product.Name,
                productType = "physical",
                priceData = new
                {
                    price = product.Price,
                },
                costAndProfitData = new
                {
                    itemCost = product.Cost
                },
                description = "Complete description of this product",
                visible = true,
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixProdCreateResponse>(request);

        return response == null ? "" : response.OnlineWixProduct.Id;
    }

    public async Task<bool> Update(string refreshToken, string id, LiteProduct product)
    {
        var request = new RestRequest($"/stores/v1/products/{id}").AddJsonBody(new
        {
            product = new
            {
                name = product.Name,
                productType = "physical",
                priceData = new
                {
                    price = product.Price,
                },
                costAndProfitData = new
                {
                    itemCost = product.Cost
                },
                description = "Complete description of this product",
                visible = product.AddOnlineStore,
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PatchAsync<OnlineWixProdCreateResponse>(request);

        return response != null;
    }
    public async Task<bool> ChangeState(string refreshToken, string id, LiteProduct product, bool state)
    {
        var request = new RestRequest($"/stores/v1/products/{id}").AddJsonBody(new
        {
            product = new
            {
                name = product.Name,
                productType = "physical",
                priceData = new
                {
                    price = product.Price,
                },
                costAndProfitData = new
                {
                    itemCost = product.Cost
                },
                description = "Complete description of this product",
                visible = state,
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PatchAsync<OnlineWixProdCreateResponse>(request);

        return response != null;
    }


    public async Task<bool> Delete(string refreshToken, string id)
    {
        var request = new RestRequest($"/stores/v1/products/{id}");

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.DeleteAsync(request);

        return response.StatusCode == null;
    }

    public async Task<List<OnlineWixProduct>> GetAllProduct(string refreshToken, bool includeVariants)
    {
        var request = new RestRequest($"/stores/v1/products/query").AddJsonBody(new
        {
            includeVariants = true
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixGetProductResponse>(request);

        return response?.OnlineWixProduct?.ToList() ?? new List<OnlineWixProduct>();
    }

    public async Task<List<OnlineWixProduct>> GetAllProductByCollection(string refreshToken, bool includeVariants, string collectionId)
    {
        var query = new
        {
            filter = $"{{\"collections.id\": {{ \"$hasSome\": [\"{collectionId}\"]}}}}"
        };

        var request = new RestRequest($"/stores/v1/collections/query").AddJsonBody(new
        {
            includeVariants = true,
            query
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixGetProductResponse>(request);
        
        return response?.OnlineWixProduct?.ToList() ?? new List<OnlineWixProduct>();
    }

    public async Task<OnlineWixProduct> GetProductById(string refreshToken, string id)
    {
        var request = new RestRequest($"/stores/v1/products/{id}").AddJsonBody(new{ });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixProdCreateResponse>(request);

        return response.OnlineWixProduct;
    }

    public async Task<bool> AddMediaProductById(string refreshToken, Media media, string ProductId)
    {
        var request = new RestRequest($"/stores/v1/products/{ProductId}/media").AddJsonBody(new
        {
            media = new[]
            {
                new
                {
                     mediaId = media.MediaId
                }
            }
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<bool>(request);
        
        return response;
    }

}
