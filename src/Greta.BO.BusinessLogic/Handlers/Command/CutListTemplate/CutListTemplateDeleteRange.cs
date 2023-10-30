using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;

/// <summary>
/// Command for delete range of CutListTemplate entities
/// </summary>
/// <param name="Ids">List of long ids</param>
public record CutListTemplateDeleteRangeCommand(List<long> Ids) : IRequest<CutListTemplateDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc/>
public class CutListTemplateDeleteRangeHandler : IRequestHandler<CutListTemplateDeleteRangeCommand, CutListTemplateDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly ICutListTemplateService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CutListTemplateDeleteRangeHandler(
        ILogger<CutListTemplateDeleteRangeHandler> logger,
        ICutListTemplateService service)
    {
        _logger = logger;
        _service = service;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CutListTemplateDeleteRangeResponse> Handle(CutListTemplateDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var idsNotRemove = new List<long>();
        foreach (var id in request.Ids)
        {
            if (!await _service.CanDeleted(id))
            {
                idsNotRemove.Add(id);
            }
        }

        var result = await _service.DeleteRange(request.Ids.Except(idsNotRemove).ToList());
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new CutListTemplateDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc/>
public record CutListTemplateDeleteRangeResponse : CQRSResponse<bool>;
