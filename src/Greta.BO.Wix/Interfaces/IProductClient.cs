using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;

namespace Greta.BO.Wix.Interfaces;

public interface IProductClient : IBaseClient
{
    Task<string> Create(string refreshToken, LiteProduct product);
    Task<bool> Update(string refreshToken, string id, LiteProduct product);
    Task<bool> ChangeState(string refreshToken, string id, LiteProduct product, bool state);
    Task<bool> Delete(string refreshToken, string id);

    Task<List<OnlineWixProduct>> GetAllProduct(string refreshToken, bool includeVariants);
    Task<List<OnlineWixProduct>> GetAllProductByCollection(string refreshToken, bool includeVariants, string collectionId);
    Task<OnlineWixProduct> GetProductById(string refreshToken, string id);

    Task<bool> AddMediaProductById(string refreshToken, Media media, string ProductId);

}