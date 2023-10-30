using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.OnlineStores;

public record DeleteWixOnlineStoreCommand(string RefreshToken, List<string> siteInstance): IRequest<Result>;

public class DeleteWixOnlineStoreHandler: IRequestHandler<DeleteWixOnlineStoreCommand, Result>
{
    private readonly WixApiClient _client;

    public DeleteWixOnlineStoreHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<Result> Handle(DeleteWixOnlineStoreCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.OnlineStore!.DeleteOnlineStoreWix(request.RefreshToken, request.siteInstance);
    }
}