using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

/// <inheritdoc />
public record VendorOrderDeleteCommand(long Id) : IRequest<VendorOrderDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDeleteHandler : IRequestHandler<VendorOrderDeleteCommand, VendorOrderDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderDeleteHandler(ILogger<VendorOrderDeleteHandler> logger, IVendorOrderService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDeleteResponse> Handle(VendorOrderDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new VendorOrderDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record VendorOrderDeleteResponse : CQRSResponse<bool>;