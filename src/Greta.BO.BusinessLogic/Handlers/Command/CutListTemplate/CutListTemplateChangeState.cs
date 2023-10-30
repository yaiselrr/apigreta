using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;

/// <summary>
/// Command for change state to CutListTemplate entity
/// </summary>
/// <param name="Id"></param>
/// <param name="State"></param>
public record CutListTemplateChangeStateCommand(long Id, bool State) : IRequest<CutListTemplateChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc/>
public class CutListTemplateChangeStateHandler : IRequestHandler<CutListTemplateChangeStateCommand, CutListTemplateChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly ICutListTemplateService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CutListTemplateChangeStateHandler(ILogger<CutListTemplateChangeStateHandler> logger, ICutListTemplateService service)
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
    public async Task<CutListTemplateChangeStateResponse> Handle(CutListTemplateChangeStateCommand request, CancellationToken cancellationToken=default)
    {
        if (request.Id < 1)
        {
            _logger.LogInformation("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
            
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new CutListTemplateChangeStateResponse { Data = result};
    }
}

/// <inheritdoc/>
public record CutListTemplateChangeStateResponse : CQRSResponse<bool>;
