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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record ScalendarGetByIdQuery(long Id) : IRequest<ScalendarGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Scalendar).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"ScalendarGetById{Id}";
}

/// <inheritdoc />
public class ScalendarGetByIdHandler : IRequestHandler<ScalendarGetByIdQuery, ScalendarGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IScalendarService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScalendarGetByIdHandler(IScalendarService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScalendarGetByIdResponse> Handle(ScalendarGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<ScalendarModel>(entity);
        return new ScalendarGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record ScalendarGetByIdResponse : CQRSResponse<ScalendarModel>;