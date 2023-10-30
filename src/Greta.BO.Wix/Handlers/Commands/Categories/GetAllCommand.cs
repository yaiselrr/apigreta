using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Models;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record GetAllWixCategoryCommand(string RefreshToken, bool includeNumberOfProducts, bool includeDescription, bool includeMedia): IRequest<List<OnlineWixCategory>>;

public class GetAllWixCategoryHandler: IRequestHandler<GetAllWixCategoryCommand, List<OnlineWixCategory>>
{
    private readonly WixApiClient _client;

    public GetAllWixCategoryHandler(
        WixApiClient client
        )
    {
        _client = client;
    }
    
    public async Task<List<OnlineWixCategory>> Handle(GetAllWixCategoryCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.Get(request.RefreshToken, request.includeNumberOfProducts, request.includeDescription, request.includeMedia);
    }
}