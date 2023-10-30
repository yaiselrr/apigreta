using Greta.BO.Wix.Factories;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;

namespace Greta.BO.Wix.Apis;

public class WixApiClient
{
    private readonly WixApiClientFactory _factory;

    public WixApiClient(WixOptions wixOptions)
    {
        _factory = new WixApiClientFactory(
            wixOptions
        );
    }
    
    public ICategoryClient? Category => _factory.CreateClient<ICategoryClient>();
    public IProductClient? Product => _factory.CreateClient<IProductClient>();
    public IMediaClient? Media => _factory.CreateClient<IMediaClient>();
    public IOnlineStoreClient? OnlineStore => _factory.CreateClient<IOnlineStoreClient>();
}