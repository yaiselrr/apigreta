using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;

/// <summary>
/// Get all entities
/// </summary>
public record ScalendarGetAllQuery : IRequest<ScalendarGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Scalendar).ToLower()}")
    };
}

/// <inheritdoc />
public class ScalendarGetAllHandler : IRequestHandler<ScalendarGetAllQuery, ScalendarGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IScalendarService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScalendarGetAllHandler(IScalendarService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScalendarGetAllResponse> Handle(ScalendarGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new ScalendarGetAllResponse {Data = _mapper.Map<List<ScalendarModel>>(entities)};
    }
}

/// <inheritdoc />
public record ScalendarGetAllResponse : CQRSResponse<List<ScalendarModel>>;