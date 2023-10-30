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
using Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutListDetail;

/// <summary>
/// Query for filter the CutListDetail entities
/// </summary>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record CutListDetailFilterQuery
    ( CutListDetailSearchModel Filter) : IRequest<CutListDetailFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListDetailFilterHandler : IRequestHandler<CutListDetailFilterQuery, CutListDetailFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListDetailFilterHandler(ICutListDetailService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListDetailFilterResponse> Handle(CutListDetailFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new CutListDetailFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(1, int.MaxValue, spec, cancellationToken);
        return new CutListDetailFilterResponse { Data = _mapper.Map<List<CutListDetailModel>>(entities.Data) };
    }
}

/// <inheritdoc />
public record CutListDetailFilterResponse : CQRSResponse<List<CutListDetailModel>>;