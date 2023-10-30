using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.StoreProduct
{
    public static class StoreProductFilter
    {
        public record Query
            (long productId, int CurrentPage, int PageSize, StoreProductSearchModel Filter) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"associate_product_store")
            };
        }

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

                var nEnti = _mapper.Map<Api.Entities.StoreProduct>(request.Filter);
                nEnti.ProductId = request.productId;
                var entities = await _service.Filter(
                    request.CurrentPage,
                    request.PageSize,
                    nEnti,
                    request.Filter.Search,
                    request.Filter.Sort);
                return new Response {Data = _mapper.Map<Pager<StoreProductModel>>(entities)};
                // var entities = await this._service.GetAllByProduct(request.productId);
                // return new Response() { Data = this._mapper.Map<List<StoreProductDto>>(entities) };
            }
        }

        public record Response : CQRSResponse<Pager<StoreProductModel>>;
    }
}