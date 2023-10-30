using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutList;

/// <summary>
/// Get CutList by animal and customer
/// </summary>
/// <param name="AnimalId">Animal id</param>
/// <param name="CustomerId">Customer id</param>
public record CutListGetCutListQuery(long AnimalId, long CustomerId, bool FullDetails) : IRequest<CutListGetCutListResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CutListGetCutList{AnimalId}{CustomerId}{FullDetails}";
}

/// <inheritdoc />
public class CutListGetCutListHandler : IRequestHandler<CutListGetCutListQuery, CutListGetCutListResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListGetCutListHandler(ICutListService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListGetCutListResponse> Handle(CutListGetCutListQuery request, CancellationToken cancellationToken = default)
    {
        var spec = new CutListDetailGetSingleSpec(request.AnimalId, request.CustomerId, request.FullDetails);
        var entity = await _service.Get(spec, cancellationToken);
        var data = _mapper.Map<CutListModel>(entity);
        return new CutListGetCutListResponse { Data = data };
    }
}

/// <inheritdoc />
public record CutListGetCutListResponse : CQRSResponse<CutListModel>;