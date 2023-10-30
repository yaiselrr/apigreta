using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;

namespace Greta.BO.Wix.Interfaces;

public interface ICategoryClient: IBaseClient
{
    Task<string> Create(string refreshToken, LiteCategory category);
    Task<bool> Update(string refreshToken, string id,  LiteCategory category);
    
    Task<bool> Delete(string refreshToken, string id);
    
    Task<List<OnlineWixCategory>> Get(string refreshToken, bool includeNumberOfProducts, bool includeDescription, bool includeMedia);
    
    Task<OnlineWixCategory> GetCollectionById(string refreshToken, string id);
    
    Task<bool> AddProductsToCollection(string refreshToken, string id, List<string> productIds);
    Task<bool> RemoveProductsFromCollection(string refreshToken, string id, List<string> productIds);
}


// {
// "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
// "title": "One or more validation errors occurred.",
// "status": 400,
// "traceId": "00-430a0c9b3e4626cf4da6866310e53142-c5235e684fe4bc98-00",
// "errors": {
//     "ProductsIds": [
//     "The ProductsIds field is required."
//         ],
//     "$.productsIds": [
//     "The JSON value could not be converted to GWix.Shared.DTO.ProductsIds. Path: $.productsIds | LineNumber: 1 | BytePositionInLine: 20."
//         ]
// }
// }