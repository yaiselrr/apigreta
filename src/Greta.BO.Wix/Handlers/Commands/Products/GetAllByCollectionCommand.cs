using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record GetAllWixProductByCollectionCommand(string RefreshToken, bool includeVariants, string collectionId): IRequest<List<OnlineWixProduct>>;

public class GetAllWixProductByCollectionHandler: IRequestHandler<GetAllWixProductByCollectionCommand, List<OnlineWixProduct>>
{
    private readonly WixApiClient _client;

    public GetAllWixProductByCollectionHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<List<OnlineWixProduct>> Handle(GetAllWixProductByCollectionCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.GetAllProductByCollection(request.RefreshToken, request.includeVariants, request.collectionId);
    }
}