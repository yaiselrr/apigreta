using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Region;

/// <summary>
/// Get all entities
/// </summary>
public record RegionGetAllQuery : IRequest<RegionGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionGetAllHandler : IRequestHandler<RegionGetAllQuery, RegionGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RegionGetAllHandler(IRegionService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RegionGetAllResponse> Handle(RegionGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new RegionGetAllResponse {Data = _mapper.Map<List<RegionModel>>(entities)};
    }
}

/// <inheritdoc />
public record RegionGetAllResponse : CQRSResponse<List<RegionModel>>;