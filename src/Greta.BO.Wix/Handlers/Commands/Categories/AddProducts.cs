using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record AddWixProductsCommand(string RefreshToken, string Category, List<string> ProductsIds) : IRequest<bool>;

public class AddWixProductsHandler : IRequestHandler<AddWixProductsCommand, bool>
{
    private readonly WixApiClient _client;

    public AddWixProductsHandler(
        WixApiClient client
    )
    {
        _client = client;
    }

    public async Task<bool> Handle(AddWixProductsCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.AddProductsToCollection(request.RefreshToken, request.Category,
            request.ProductsIds);
    }
}