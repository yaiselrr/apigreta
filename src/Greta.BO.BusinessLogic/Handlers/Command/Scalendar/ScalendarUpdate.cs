using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Scalendar;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record ScalendarUpdateCommand
    (long Id, ScalendarModel Entity) : IRequest<ScalendarUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Scalendar).ToLower()}")
    };
}

/// <inheritdoc />
public class ScalendarUpdateHandler : IRequestHandler<ScalendarUpdateCommand, ScalendarUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScalendarService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScalendarUpdateHandler(
        ILogger<ScalendarUpdateHandler> logger,
        IScalendarService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScalendarUpdateResponse> Handle(ScalendarUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Scalendar>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Scalendar {ScalendarId} update successfully", request.Id);
        return new ScalendarUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record ScalendarUpdateResponse : CQRSResponse<bool>;