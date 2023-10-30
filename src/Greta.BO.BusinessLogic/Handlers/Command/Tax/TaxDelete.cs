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
/// Delete entity by entity id
/// </summary>
/// <param name="Id">Entity id</param>
public record TaxDeleteCommand(long Id) : IRequest<TaxDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxDeleteHandler : IRequestHandler<TaxDeleteCommand, TaxDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TaxDeleteHandler(
        ILogger<TaxDeleteHandler> logger,
        ITaxService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<TaxDeleteResponse> Handle(TaxDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
                _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
                return new TaxDeleteResponse {Data = result};
    }
}

/// <inheritdoc />
public record TaxDeleteResponse : CQRSResponse<bool>;