using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record GetAllWixProductCommand(string RefreshToken, bool includeVariants): IRequest<List<OnlineWixProduct>>;

public class GetAllWixProductHandler: IRequestHandler<GetAllWixProductCommand, List<OnlineWixProduct>>
{
    private readonly WixApiClient _client;

    public GetAllWixProductHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<List<OnlineWixProduct>> Handle(GetAllWixProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.GetAllProduct(request.RefreshToken, request.includeVariants);
    }
}