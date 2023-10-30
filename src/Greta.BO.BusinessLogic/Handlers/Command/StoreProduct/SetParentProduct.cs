using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct
{
    public static class SetParentProduct
    {
        public record Command(SetParentProductModel model) : IRequest<Response>;

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
                if (request.model.Child <= 0 || request.model.Parent <= 0 || request.model.Count <= 0)
                {
                    throw new BussinessValidationException("All Parameters was required.");
                    //continue;
                }

                var child = await _productService.GetWithProduct(request.model.Child);
                var parent = await _productService.GetWithParent(request.model.Parent);

                if (child ==  null)
                {
                    throw new BussinessValidationException("Child not found.");
                }
                
                if (parent ==  null)
                {
                    throw new BussinessValidationException("Parent not found.");
                }
                
                if (child.StoreId != parent.StoreId)
                {
                    throw new BussinessValidationException("The parent selected for this product must belong to the same store as the child.");
                }
                child.ParentId = request.model.Parent;
                child.SplitCount = request.model.Count;
                child.Product.AllowZeroStock = false;
                await _productService.Put(child.Id, child);
                
                
                
                //finding the parent off this breakpack
                _logger.LogInformation("Start finding parent on this Break Pack.");

                if (parent.ParentId == null)
                {
                    _logger.LogInformation("Parent found calling update method.");
                    //await _mediator.Send(new BreakPackUpdateFromParent.Command(parent));
                    await UpdateCostFromTop(parent.Child, parent.Cost);
                }
                else
                {
                    var rParent = await FindBreakPackParent(parent.ParentId.Value);
                    await _mediator.Send(new BreakPackUpdateFromParent.Command(rParent));
                    //await UpdateCostFromTop(rParent.Child, rParent.Cost);
                }
                return new Response() { Data = true };
            }

            private async Task UpdateCostFromTop(Api.Entities.StoreProduct child, decimal parentCost)
            {
                if (child == null) return;
                child.Cost = child.SplitCount == 0 ? 0 : parentCost / child.SplitCount;
                await _productService.Put(child.Id, child);
                if (child.ChildId == null) return;
                var nChild = await _productService.GetWithParent(child.ChildId.Value);
                await UpdateCostFromTop(nChild, child.Cost);
            }
            
            private async Task<Api.Entities.StoreProduct> FindBreakPackParent(long child)
            {
                var parent = await _productService.GetWithParent(child);
                if (parent.ParentId == null)
                {
                    return parent;
                }
                else
                {
                    return  await FindBreakPackParent(parent.ParentId.Value);
                }
            }
        }
    }
}