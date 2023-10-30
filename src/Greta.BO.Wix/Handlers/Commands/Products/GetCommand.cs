using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record GetWixProductCommand(string RefreshToken, string id): IRequest<OnlineWixProduct>;

public class GetWixProductHandler: IRequestHandler<GetWixProductCommand, OnlineWixProduct>
{
    private readonly WixApiClient _client;

    public GetWixProductHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<OnlineWixProduct> Handle(GetWixProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.GetProductById(request.RefreshToken, request.id);
    }
}