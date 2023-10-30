using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Customer;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record CustomerDeleteRangeCommand(List<long> Ids) : IRequest<CustomerDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class CustomerDeleteRangeHandler : IRequestHandler<CustomerDeleteRangeCommand, CustomerDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CustomerDeleteRangeHandler(
        ILogger<CustomerDeleteRangeHandler> logger,
        ICustomerService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<CustomerDeleteRangeResponse> Handle(CustomerDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new CustomerDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record CustomerDeleteRangeResponse : CQRSResponse<bool>;