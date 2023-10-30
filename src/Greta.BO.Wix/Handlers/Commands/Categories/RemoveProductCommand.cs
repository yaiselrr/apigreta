using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record RemoveProductsFromCollectionCommand(string RefreshToken, string Id, List<string> ProductIds) : IRequest<bool>;

public class RemoveProductsFromCollectionHandler : IRequestHandler<RemoveProductsFromCollectionCommand, bool>
{
    private readonly WixApiClient _client;

    public RemoveProductsFromCollectionHandler(
        WixApiClient client
        )
    {
        _client = client;
    }

    public async Task<bool> Handle(RemoveProductsFromCollectionCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.RemoveProductsFromCollection(request.RefreshToken, request.Id, request.ProductIds);
    }
}