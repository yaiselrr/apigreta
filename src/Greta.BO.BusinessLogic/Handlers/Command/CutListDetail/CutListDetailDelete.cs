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
/// <param name="Id"></param>
public record CutListDetailDeleteCommand(long Id) : IRequest<CutListDetailDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListDetailDeleteHandler : IRequestHandler<CutListDetailDeleteCommand, CutListDetailDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly ICutListDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CutListDetailDeleteHandler(ILogger<CutListDetailDeleteHandler> logger, ICutListDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<CutListDetailDeleteResponse> Handle(CutListDetailDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new CutListDetailDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record CutListDetailDeleteResponse : CQRSResponse<bool>;