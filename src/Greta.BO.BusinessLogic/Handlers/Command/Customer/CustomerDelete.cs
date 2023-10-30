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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record CustomerDeleteCommand(long Id) : IRequest<CustomerDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class CustomerDeleteHandler : IRequestHandler<CustomerDeleteCommand, CustomerDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CustomerDeleteHandler(
        ILogger<CustomerDeleteHandler> logger,
        ICustomerService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<CustomerDeleteResponse> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new CustomerDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record CustomerDeleteResponse : CQRSResponse<bool>;