using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Products
{
    public static class ProductChangeState
    {
        public record Command(long Id, bool State) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ILogger _logger;
            private readonly IProductService _service;

            public Handler(ILogger<Handler> logger, IProductService service)
            {
                _logger = logger;
                _service = service;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _service.ChangeState(request.Id, request.State);
                _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
                return new Response {Data = result};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}