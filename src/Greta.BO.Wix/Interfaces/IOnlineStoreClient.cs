using Greta.BO.Wix.Models;

namespace Greta.BO.Wix.Interfaces;

public interface IOnlineStoreClient: IBaseClient
{
    Task<string> Create(string token);
    Task<bool> FinishesInstallation (string refreshToken);
    Task<OnlineWixStoreResponse> GetStore (string refreshToken);
    Task<Result> DeleteOnlineStoreWix (string refreshToken, List<string> siteInstance);
}