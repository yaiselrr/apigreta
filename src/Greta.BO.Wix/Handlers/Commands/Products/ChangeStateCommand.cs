using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record ChangeStateWixProductCommand(string RefreshToken, string Id, LiteProduct Product, bool State):IRequest<bool>;

public class ChangeStateWixProductHandler: IRequestHandler<ChangeStateWixProductCommand, bool>
{
    private readonly WixApiClient _client;

    public ChangeStateWixProductHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<bool> Handle(ChangeStateWixProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.ChangeState(request.RefreshToken, request.Id, request.Product, request.State);
    }
}