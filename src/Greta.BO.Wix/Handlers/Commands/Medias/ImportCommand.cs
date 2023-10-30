// using Greta.BO.Api.Entities.Lite;
// namespace Greta.BO.Wix.Models;
// using Greta.BO.Wix.Apis;
// using MediatR;

// namespace Greta.BO.Wix.Handlers.Commands.Medias;

// public record ImportWixMediaCommand(string RefreshToken, LiteMedia Media) : IRequest<string>;

// public class ImportWixMediaHandler : IRequestHandler<ImportWixMediaCommand, string>
// {
//     private readonly WixApiClient _client;

//     public ImportWixMediaHandler(
//         WixApiClient client
//         )
//     {
//         _client = client;
//     }

//     public async Task<string> Handle(ImportWixMediaCommand request, CancellationToken cancellationToken = default)
//     {
//         return await _client.Media!.Import(request.RefreshToken, request.Media);
//     }
// }