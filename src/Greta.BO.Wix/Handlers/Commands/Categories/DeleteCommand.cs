using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Categories;

public record DeleteCommand(string RefreshToken, string Id):IRequest<bool>;

public class DeleteCommandHandler: IRequestHandler<DeleteCommand, bool>
{
    private readonly WixApiClient _client;

    public DeleteCommandHandler(
        WixApiClient client
    )
    {
        _client = client;
    }
    
    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Category!.Delete(request.RefreshToken, request.Id);
    }
}