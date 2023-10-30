using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.Core.Models.Pager;
using FluentValidation;
using AutoMapper;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleReasonCodes
{
    public static class ScaleReasonCodesFilter
    {
        public record Query(int CurrentPage, int PageSize, ScaleReasonCodesSearchModel Filter) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_reason_codes")
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
            protected readonly IScaleReasonCodesService _service;

            public Handler(ILogger<Handler> logger, IScaleReasonCodesService service, IMapper mapper)
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
                    _mapper.Map<Api.Entities.ScaleReasonCodes>(request.Filter),
                    request.Filter.Search,
                    request.Filter.Sort);
                return new Response { Data = _mapper.Map<Pager<ScaleReasonCodesModel>>(entities) };
            }
        }

        public record Response : CQRSResponse<Pager<ScaleReasonCodesModel>>;
    }
}
