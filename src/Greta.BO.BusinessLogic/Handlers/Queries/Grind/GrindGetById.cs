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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Grind;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record GrindGetByIdQuery(long Id) : IRequest<GrindGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Grind).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"GrindGetById{Id}";
}

/// <inheritdoc />
public class GrindGetByIdHandler : IRequestHandler<GrindGetByIdQuery, GrindGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public GrindGetByIdHandler(IGrindService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GrindGetByIdResponse> Handle(GrindGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.Grind>(request.Id), cancellationToken);
        var data = _mapper.Map<GrindModel>(entity);
        return new GrindGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record GrindGetByIdResponse : CQRSResponse<GrindModel>;