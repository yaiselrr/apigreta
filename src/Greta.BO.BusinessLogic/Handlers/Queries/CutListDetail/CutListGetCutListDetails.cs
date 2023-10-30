using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutListDetail;

/// <summary>
/// Get CutListDetails by cutList
/// </summary>
/// <param name="CutListId">CutList id</param>
public record CutListDetailGetCutListDetailsQuery(long CutListId) : IRequest<CutListDetailGetCutListDetailsResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CutListDetailGetCutListDetails{CutListId}";
}

/// <inheritdoc />
public class CutListDetailGetCutListDetailsHandler : IRequestHandler<CutListDetailGetCutListDetailsQuery, CutListDetailGetCutListDetailsResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListDetailGetCutListDetailsHandler(ICutListService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListDetailGetCutListDetailsResponse> Handle(CutListDetailGetCutListDetailsQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetCutListDetails(request.CutListId);
        var data = _mapper.Map<CutListDetailModel>(entity);
        return new CutListDetailGetCutListDetailsResponse { Data = data };
    }
}

/// <inheritdoc />
public record CutListDetailGetCutListDetailsResponse : CQRSResponse<CutListDetailModel>;