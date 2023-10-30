using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record FinishesWixOnlineStoreCommand(string Token) : IRequest<bool>;

public class FinishesWixOnlineStoreHandler : IRequestHandler<FinishesWixOnlineStoreCommand, bool>
{
    private readonly WixApiClient _client;

    public FinishesWixOnlineStoreHandler(
        WixApiClient client
        )
    {
        _client = client;
    }

    public async Task<bool> Handle(FinishesWixOnlineStoreCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.OnlineStore!.FinishesInstallation(request.Token);
    }
}