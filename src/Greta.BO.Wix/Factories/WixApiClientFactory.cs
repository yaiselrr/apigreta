using Greta.BO.Wix.Autenticators;
using Greta.BO.Wix.Implementations;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Factories;

public class WixApiClientFactory
{
    private readonly WixOptions _wixOptions;
    private readonly RestClient _client;

    public WixApiClientFactory(WixOptions wixOptions)
    {
        _wixOptions = wixOptions;
        var options = new RestClientOptions(wixOptions.Url){
            // Authenticator = new ApikeyAutenticator(
            //     apiKey
            // ),
            MaxTimeout = TimeSpan.FromMinutes(3).Milliseconds
        };
        _client = new RestClient(options) ;
        
    }
            
    public T? CreateClient<T>() where T : class
    {
        var type = typeof(T);
        
        if (type == typeof(ICategoryClient))
        {
            return new CategoryClient(_client, _wixOptions) as T;
        }
        
        if (type == typeof(IProductClient))
        {
            return new ProductClient(_client, _wixOptions) as T;
        }

        if (type == typeof(IMediaClient))
        {
            return new MediaClient(_client, _wixOptions) as T;
        }

        if (type == typeof(IOnlineStoreClient))
        {
            return new OnlineStoreClient(_client, _wixOptions) as T;
        }
        
        throw new ArgumentException($"Invalid client type: {type.Name}");
    }
}