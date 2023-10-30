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

namespace Greta.BO.BusinessLogic.Handlers.Queries.PriceBatch
{
    public static class PriceBatchFilter
    {
        public record Query(int CurrentPage, int PageSize, PriceBatchSearchModel Filter) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_price_batch")
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
            protected readonly IPriceBatchService _service;

            public Handler(ILogger<Handler> logger, IPriceBatchService service, IMapper mapper)
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

                var entities = await _service.Filter(
                    request.CurrentPage,
                    request.PageSize,
                    _mapper.Map<Api.Entities.PriceBatch>(request.Filter),
                    request.Filter.Search,
                    request.Filter.Sort);
                return new Response {Data = _mapper.Map<Pager<PriceBatchModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<Pager<PriceBatchModel>>;
    }
}