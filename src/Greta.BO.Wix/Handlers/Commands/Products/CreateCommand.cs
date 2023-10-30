using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Apis;
using MediatR;

namespace Greta.BO.Wix.Handlers.Commands.Products;


public record CreateWixProductCommand(string RefreshToken, LiteProduct Product, string CategoryId = null):IRequest<string>;

public class CreateWixProductHandler: IRequestHandler<CreateWixProductCommand, string>
{
    private readonly WixApiClient _client;

    public CreateWixProductHandler(
        WixApiClient client
    )
    {
        _client = client;
    }
    
    public async Task<string> Handle(CreateWixProductCommand request, CancellationToken cancellationToken = default)
    {
        return await _client.Product!.Create(request.RefreshToken, request.Product);
    }
}