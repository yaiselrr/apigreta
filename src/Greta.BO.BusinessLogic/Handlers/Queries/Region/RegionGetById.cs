using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Region;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record RegionGetByIdQuery(long Id) : IRequest<RegionGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Region).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"RegionGetById{Id}";
}

/// <inheritdoc />
public class RegionGetByIdHandler : IRequestHandler<RegionGetByIdQuery, RegionGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RegionGetByIdHandler(IRegionService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RegionGetByIdResponse> Handle(RegionGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.Region>(request.Id), cancellationToken);
        var data = _mapper.Map<RegionModel>(entity);
        return new RegionGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record RegionGetByIdResponse : CQRSResponse<RegionModel>;