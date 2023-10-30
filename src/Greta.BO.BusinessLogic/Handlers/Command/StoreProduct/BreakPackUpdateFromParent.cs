using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct
{
    public static class BreakPackUpdateFromParent
    {
        public record Command(Api.Entities.StoreProduct parent) : IRequest<bool>;
        public class Handler: IRequestHandler<Command, bool>
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
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                await UpdateCostFromTop(request.parent.Child, request.parent.Cost);
                return true;
            }


            private async Task UpdateCostFromTop(Api.Entities.StoreProduct child, decimal parentPrice)
            {
                if (child == null) return;
                child.Cost = child.SplitCount == 0 ? 0 : parentPrice / child.SplitCount;
                await _productService.Put(child.Id, child);
                if (child.ChildId == null) return;
                var nChild = await _productService.GetWithParent(child.ChildId.Value);
                await UpdateCostFromTop(nChild, child.Cost);
            }
        }
    }
}