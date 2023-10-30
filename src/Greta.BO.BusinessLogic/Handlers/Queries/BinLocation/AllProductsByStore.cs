
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Queries.BinLocation
{
    public static class AllProductsByStore
    {
        public record Query(
            int CurrentPage, 
            int PageSize, 
            AllProductsByStoreRequestModel Filter) : IRequest<Response>;
        public record Response : CQRSResponse<Pager<ProductInventoryModel>>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage).GreaterThan(0);
                RuleFor(x => x.PageSize).GreaterThan(0);
            }

        }
        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IStoreProductService _service;

            public Handler(ILogger<Handler> logger, IStoreProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.CurrentPage < 1 || request.PageSize < 1)
                {
                    _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                    throw new BusinessLogicException("Page parameter out of bounds.");
                }
                var data = await _service.FilterByStore(
                        request.CurrentPage,
                        request.PageSize,
                        request.Filter.Search,
                        request.Filter.Sort,
                        request.Filter.UPC,
                        request.Filter.Name,
                        request.Filter.StoreId
                    );
                return new Response() { Data = this._mapper.Map<Pager<ProductInventoryModel>>(data) };
            }
        }
    }
}
