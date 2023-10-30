using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record UpdateWixProductCommand(string RefreshToken, string id, LiteProduct Product):IRequest<bool>;

public class UpdateWixProductHandler: IRequestHandler<UpdateWixProductCommand, bool>
{
    private readonly WixApiClient _client;

    public UpdateWixProductHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<bool> Handle(UpdateWixProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.Update(request.RefreshToken, request.id, request.Product);
    }
}