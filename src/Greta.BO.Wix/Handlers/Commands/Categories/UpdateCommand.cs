using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record UpdateWixCategoryCommand(string RefreshToken, string id, LiteCategory Category):IRequest<bool>;

public class UpdateWixCategoryHandler: IRequestHandler<UpdateWixCategoryCommand, bool>
{
    private readonly WixApiClient _client;

    public UpdateWixCategoryHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<bool> Handle(UpdateWixCategoryCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.Update(request.RefreshToken, request.id, request.Category);
    }
}