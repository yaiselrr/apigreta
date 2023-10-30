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

public record VendorOrderDeleteRangeCommand(List<long> Ids) : IRequest<VendorOrderDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDeleteRangeHandler : IRequestHandler<VendorOrderDeleteRangeCommand, VendorOrderDeleteRangeResponse>
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
    public VendorOrderDeleteRangeHandler(
        ILogger<VendorOrderDeleteRangeHandler> logger,
        IVendorOrderService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDeleteRangeResponse> Handle(VendorOrderDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new VendorOrderDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record VendorOrderDeleteRangeResponse : CQRSResponse<bool>;