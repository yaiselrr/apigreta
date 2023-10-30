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

namespace Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;

/// <summary>
/// Command for change state to MixAndMatch entity
/// </summary>
/// <param name="Id"></param>
/// <param name="State"></param>
public record MixAndMatchChangeStateCommand(long Id, bool State) : IRequest<MixAndMatchChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_mix_and_match")
    };
}

/// <inheritdoc/>
public class MixAndMatchChangeStateHandler : IRequestHandler<MixAndMatchChangeStateCommand, MixAndMatchChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IMixAndMatchService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public MixAndMatchChangeStateHandler(ILogger<MixAndMatchChangeStateHandler> logger, IMixAndMatchService service)
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
    public async Task<MixAndMatchChangeStateResponse> Handle(MixAndMatchChangeStateCommand request, CancellationToken cancellationToken=default)
    {
        if (request.Id < 1)
        {
            _logger.LogInformation("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
            
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new MixAndMatchChangeStateResponse { Data = result};
    }
}

/// <inheritdoc/>
public record MixAndMatchChangeStateResponse : CQRSResponse<bool>;
