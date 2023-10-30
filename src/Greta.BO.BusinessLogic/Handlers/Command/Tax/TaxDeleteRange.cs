using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Tax;

/// <summary>
/// Delete entities by list of entity ids
/// </summary>
/// <param name="Ids">List of entity ids</param>
public record TaxDeleteRangeCommand(List<long> Ids) : IRequest<TaxDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxDeleteRangeHandler : IRequestHandler<TaxDeleteRangeCommand, TaxDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TaxDeleteRangeHandler(
        ILogger<TaxDeleteRangeHandler> logger,
        ITaxService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<TaxDeleteRangeResponse> Handle(TaxDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new TaxDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record TaxDeleteRangeResponse : CQRSResponse<bool>;