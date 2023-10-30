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
/// Command for delete Fee
/// </summary>
/// <param name="Id"></param>
public record FeeDeleteCommand(long Id) : IRequest<FeeDeleteResponse>, IAuthorizable
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
public class FeeDeleteHandler : IRequestHandler<FeeDeleteCommand, FeeDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FeeDeleteHandler(ILogger<FeeDeleteHandler> logger, IFeeService service)
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
    public async Task<FeeDeleteResponse> Handle(FeeDeleteCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new FeeDeleteResponse { Data = result};
    }
}

///<inheritdoc/>
public record FeeDeleteResponse : CQRSResponse<bool>;
