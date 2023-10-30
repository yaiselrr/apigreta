using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.InventoryQueries;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.BinLocation
{
    public static class BinLocationProducts
    {
        public record Query
            (int CurrentPage, int PageSize, InventorySearchModel Filter) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_bin_location")
            };
        }

        public class Validator : AbstractValidator<InventoryFilterQuery>
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

                var entities = await _service.FilteBinlocation(
                    request.CurrentPage,
                    request.PageSize,
                    request.Filter.Search,
                    request.Filter.Sort,
                    request.Filter.BinLocationId
                );
                return new Response() { Data = this._mapper.Map<Pager<BinLocationResponseModel>>(entities) };
            }
        }

        public record Response : CQRSResponse<Pager<BinLocationResponseModel>>;
        
    }
}