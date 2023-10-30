using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListDetail;

/// <summary>
/// 
/// </summary>
/// <param name="Ids"></param>
public record CutListDetailDeleteRangeCommand(List<long> Ids) : IRequest<CutListDetailDeleteRangeResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListDetailDeleteRangeHandler : IRequestHandler<CutListDetailDeleteRangeCommand,
    CutListDetailDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly ICutListDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CutListDetailDeleteRangeHandler(
        ILogger<CutListDetailDeleteRangeHandler> logger,
        ICutListDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<CutListDetailDeleteRangeResponse> Handle(CutListDetailDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new CutListDetailDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record CutListDetailDeleteRangeResponse : CQRSResponse<bool>;