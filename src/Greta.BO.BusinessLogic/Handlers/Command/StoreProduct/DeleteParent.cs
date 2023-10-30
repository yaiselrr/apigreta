using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct
{
    public static class DeleteParent
    {
         public record Command(long entityId) : IRequest<Response>;

        public record Response : CQRSResponse<bool>;

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IMediator _mediator;
            private readonly IStoreProductService _productService;

            public Handler(
                ILogger<Handler> logger,
                IMediator mediator,
                IStoreProductService productService
            )
            {
                _logger = logger;
                _mediator = mediator;
                _productService = productService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var child = await _productService.GetWithParent(request.entityId);
               

                if (child ==  null)
                {
                    throw new BussinessValidationException("Child not found.");
                }
                child.ParentId =null;
                child.SplitCount = 0;
                await _productService.Put(child.Id, child);
                
                return new Response() { Data = true };
            }
        }
    }
}