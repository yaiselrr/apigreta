using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;

/// <summary>
/// Command for delete range of MixAndMatch entities
/// </summary>
/// <param name="Ids">List of long ids</param>
public record MixAndMatchDeleteRangeCommand(List<long> Ids) : IRequest<MixAndMatchDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("delete_mix_and_match")
    };
}

/// <inheritdoc/>
public class MixAndMatchDeleteRangeHandler : IRequestHandler<MixAndMatchDeleteRangeCommand, MixAndMatchDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IMixAndMatchService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public MixAndMatchDeleteRangeHandler(
        ILogger<MixAndMatchDeleteRangeHandler> logger,
        IMixAndMatchService service)
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
    public async Task<MixAndMatchDeleteRangeResponse> Handle(MixAndMatchDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new MixAndMatchDeleteRangeResponse { Data = result};
    }
}

/// <inheritdoc/>
public record MixAndMatchDeleteRangeResponse : CQRSResponse<bool>;
