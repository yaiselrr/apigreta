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

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record ScaleLabelTypeDeleteRangeCommand(List<long> Ids) : IRequest<ScaleLabelTypeDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_scale_label_type")
    };
}

/// <inheritdoc />
public class ScaleLabelTypeDeleteRangeHandler : IRequestHandler<ScaleLabelTypeDeleteRangeCommand, ScaleLabelTypeDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeDeleteRangeHandler(
        ILogger<ScaleLabelTypeDeleteRangeHandler> logger,
        IScaleLabelTypeService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeDeleteRangeResponse> Handle(ScaleLabelTypeDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation($"Entities with ids = {request.Ids} Deleted successfully.");
        return new ScaleLabelTypeDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeDeleteRangeResponse : CQRSResponse<bool>;