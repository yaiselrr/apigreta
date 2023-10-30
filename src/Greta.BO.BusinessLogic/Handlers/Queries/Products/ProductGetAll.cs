using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Products
{
    public static class ProductGetAll
    {
        public record Query : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_product")
            };
        }


        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IProductService _service;
            protected readonly IStoreProductService _storeProductservice;

            public Handler(ILogger<Handler> logger, IProductService service, IStoreProductService storeProductservice, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _storeProductservice = storeProductservice;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _service.Get();
                foreach (var item in entities)
                {
                    var storeProduct = await _storeProductservice.GetAllByProduct(item.Id);
                    item.StoreProducts = storeProduct;

                }
                return new Response {Data = _mapper.Map<List<ProductModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<List<ProductModel>>;
    }
}