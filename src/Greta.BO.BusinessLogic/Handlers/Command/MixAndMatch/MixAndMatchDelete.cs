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
/// Command for delete MixAndMatch entity
/// </summary>
/// <param name="Id"></param>
public record MixAndMatchDeleteCommand(long Id) : IRequest<MixAndMatchDeleteResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("delete_mix_and_match")
    };
}

/// <inheritdoc/>
public class MixAndMatchDeleteHandler : IRequestHandler<MixAndMatchDeleteCommand, MixAndMatchDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IMixAndMatchService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>   
    public MixAndMatchDeleteHandler(ILogger<MixAndMatchDeleteHandler> logger, IMixAndMatchService service)
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
    public async Task<MixAndMatchDeleteResponse> Handle(MixAndMatchDeleteCommand request, CancellationToken cancellationToken=default)
    {
        if (request.Id < 1)
        {
            _logger.LogInformation("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new MixAndMatchDeleteResponse { Data = result};
    }
}

/// <inheritdoc/>
public record MixAndMatchDeleteResponse : CQRSResponse<bool>;
