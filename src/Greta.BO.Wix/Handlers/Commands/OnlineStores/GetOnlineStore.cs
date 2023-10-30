using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.OnlineStores;

public record GetWixOnlineStoreCommand(string RefreshToken): IRequest<OnlineWixStoreResponse>;

public class GetWixOnlineStoreHandler: IRequestHandler<GetWixOnlineStoreCommand, OnlineWixStoreResponse>
{
    private readonly WixApiClient _client;

    public GetWixOnlineStoreHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<OnlineWixStoreResponse> Handle(GetWixOnlineStoreCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.OnlineStore!.GetStore(request.RefreshToken);
    }
}