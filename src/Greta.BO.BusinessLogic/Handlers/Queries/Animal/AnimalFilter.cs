using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.AnimalSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Animal;

/// <summary>
/// Query for filter the Animal entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record AnimalFilterQuery
    (int CurrentPage, int PageSize, AnimalSearchModel Filter) : IRequest<AnimalFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalFilterValidator : AbstractValidator<AnimalFilterQuery>
{
    /// <inheritdoc />
    public AnimalFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class AnimalFilterHandler : IRequestHandler<AnimalFilterQuery, AnimalFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalFilterHandler(IAnimalService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalFilterResponse> Handle(AnimalFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        return new AnimalFilterResponse
        {
            Data = _mapper.Map<Pager<AnimalModel>>(
                await _service.FilterSpec(
                    request.CurrentPage,
                    request.PageSize,
                    new AnimalFilterSpec(request.Filter),
                    cancellationToken)
            )
        };
    }
}

/// <inheritdoc />
public record AnimalFilterResponse : CQRSResponse<Pager<AnimalModel>>;