using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.StoreProduct
{
    public static class StoreProductParentGetById
    {
        public record Query(long Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ILogger _logger;
            private readonly IMapper _mapper;
            private readonly IStoreProductService _service;

            public Handler(ILogger<Handler> logger, IStoreProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.GetWithParent(request.Id);
                if (entity.ParentId == null)
                {
                    return new Response {Data = null};
                }
                var entity1 = await _service.GetWithParent(entity.ParentId.Value);
                entity1.SplitCount = entity.SplitCount;
                var data = _mapper.Map<StoreProductSetParentResponse>(entity1);
                return data == null ? null : new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<StoreProductSetParentResponse>;
    }
}