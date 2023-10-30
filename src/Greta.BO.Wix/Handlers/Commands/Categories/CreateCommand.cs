using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record CreateWixCategoryCommand(string RefreshToken, LiteCategory Category) : IRequest<string>;

public class CreateWixCategoryHandler : IRequestHandler<CreateWixCategoryCommand, string>
{
    private readonly WixApiClient _client;

    public CreateWixCategoryHandler(
        WixApiClient client
        )
    {
        _client = client;
    }

    public async Task<string> Handle(CreateWixCategoryCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.Create(request.RefreshToken, request.Category);
    }
}