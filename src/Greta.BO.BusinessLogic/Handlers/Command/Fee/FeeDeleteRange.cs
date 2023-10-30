using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Fee;

/// <summary>
/// Command for delete a range of Fees by ids
/// </summary>
/// <param name="Ids"></param>
public record FeeDeleteRangeCommand(List<long> Ids) : IRequest<FeeDeleteRangeResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Fee).ToLower()}")
    };
}

///<inheritdoc/>
public class FeeDeleteRangeHandler : IRequestHandler<FeeDeleteRangeCommand, FeeDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FeeDeleteRangeHandler(ILogger<FeeDeleteRangeHandler> logger, IFeeService service)
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
    public async Task<FeeDeleteRangeResponse> Handle(FeeDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new FeeDeleteRangeResponse { Data = result};
    }
}

///<inheritdoc/>
public record FeeDeleteRangeResponse : CQRSResponse<bool>;
