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
using Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Profiles;

/// <summary>
/// Query for filter and paginate profiles
/// </summary>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record ProfilesFilterQuery(int CurrentPage, int PageSize, ProfilesSearchModel Filter) : IRequest<ProfilesFilterResponse>, IAuthorizable
{
    ///<inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Profiles).ToLower()}")
    };
}

/// <inheritdoc />
public class ProfilesFilterValidator : AbstractValidator<ProfilesFilterQuery>
{
    /// <inheritdoc />
    public ProfilesFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ProfilesFilterHandler : IRequestHandler<ProfilesFilterQuery, ProfilesFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProfilesService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ProfilesFilterHandler(ILogger<ProfilesFilterHandler> logger, IProfilesService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ProfilesFilterResponse> Handle(ProfilesFilterQuery request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }

        var spec = new ProfilesFilterSpec(request.Filter);
        var result = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);               
        return new ProfilesFilterResponse { Data = _mapper.Map<Pager<ProfilesListModel>>(result)};
    }
}

/// <inheritdoc />
public record ProfilesFilterResponse : CQRSResponse<Pager<ProfilesListModel>>;
