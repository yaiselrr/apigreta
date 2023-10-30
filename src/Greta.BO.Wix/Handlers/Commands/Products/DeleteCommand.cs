using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;

public record DeleteProductCommand(string RefreshToken, string Id):IRequest<bool>;

public class DeleteProductCommandHandler: IRequestHandler<DeleteProductCommand, bool>
{
    private readonly WixApiClient _client;

    public DeleteProductCommandHandler(
        WixApiClient client
    )
    {
        _client = client;
    }
    
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.Delete(request.RefreshToken, request.Id);
    }
}