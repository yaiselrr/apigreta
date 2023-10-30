using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record CreateWixOnlineStoreCommand(string Token) : IRequest<string>;

public class CreateWixOnlineStoreHandler : IRequestHandler<CreateWixOnlineStoreCommand, string>
{
    private readonly WixApiClient _client;

    public CreateWixOnlineStoreHandler(
        WixApiClient client
        )
    {
        _client = client;
    }

    public async Task<string> Handle(CreateWixOnlineStoreCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.OnlineStore!.Create(request.Token);
    }
}