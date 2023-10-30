using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Discount;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record DiscountDeleteCommand(long Id) : IRequest<DiscountDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountDeleteHandler : IRequestHandler<DiscountDeleteCommand, DiscountDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DiscountDeleteHandler(
        ILogger<DiscountDeleteHandler> logger,
        IDiscountService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DiscountDeleteResponse> Handle(DiscountDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new DiscountDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record DiscountDeleteResponse : CQRSResponse<bool>;