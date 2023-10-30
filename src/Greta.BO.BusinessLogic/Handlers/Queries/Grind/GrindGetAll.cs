using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Grind;

/// <summary>
/// Get all entities
/// </summary>
public record GrindGetAllQuery : IRequest<GrindGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindGetAllHandler : IRequestHandler<GrindGetAllQuery, GrindGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public GrindGetAllHandler(IGrindService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GrindGetAllResponse> Handle(GrindGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new GrindGetAllResponse {Data = _mapper.Map<List<GrindModel>>(entities)};
    }
}

/// <inheritdoc />
public record GrindGetAllResponse : CQRSResponse<List<GrindModel>>;