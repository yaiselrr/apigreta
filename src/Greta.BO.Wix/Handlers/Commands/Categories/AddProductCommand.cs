using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record AddProductCategoryCommand(string RefreshToken, string id, List<string> productIds) : IRequest<bool>;

public class AddProductCategoryHandler : IRequestHandler<AddProductCategoryCommand, bool>
{
    private readonly WixApiClient _client;

    public AddProductCategoryHandler(
        WixApiClient client
        )
    {
        _client = client;
    }

    public async Task<bool> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.AddProductsToCollection(request.RefreshToken, request.id, request.productIds);
    }
}