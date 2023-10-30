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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;

public record VendorOrderDetailDeleteRangeCommand(List<long> Ids) : IRequest<VendorOrderDetailDeleteRangeResponse>,
    IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_vendor_order")
    };
}

public class VendorOrderDetailDeleteRangeHandler : IRequestHandler<VendorOrderDetailDeleteRangeCommand,
    VendorOrderDetailDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;

    public VendorOrderDetailDeleteRangeHandler(
        ILogger<VendorOrderDetailDeleteRangeHandler> logger,
        IVendorOrderDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<VendorOrderDetailDeleteRangeResponse> Handle(VendorOrderDetailDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new VendorOrderDetailDeleteRangeResponse { Data = result };
    }
}

public record VendorOrderDetailDeleteRangeResponse : CQRSResponse<bool>;