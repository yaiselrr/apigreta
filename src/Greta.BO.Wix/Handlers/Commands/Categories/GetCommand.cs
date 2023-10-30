using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record GetWixCategoryCommand(string RefreshToken, string Id): IRequest<OnlineWixCategory>;

public class GetWixCategoryHandler: IRequestHandler<GetWixCategoryCommand, OnlineWixCategory>
{
    private readonly WixApiClient _client;

    public GetWixCategoryHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<OnlineWixCategory> Handle(GetWixCategoryCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.GetCollectionById(request.RefreshToken, request.Id);
    }
}