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

/// <inheritdoc />
public record VendorOrderDetailDeleteCommand(long Id) : IRequest<VendorOrderDetailDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailDeleteHandler : IRequestHandler<VendorOrderDetailDeleteCommand, VendorOrderDetailDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderDetailDeleteHandler(ILogger<VendorOrderDetailDeleteHandler> logger, IVendorOrderDetailService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<VendorOrderDetailDeleteResponse> Handle(VendorOrderDetailDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new VendorOrderDetailDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record VendorOrderDetailDeleteResponse : CQRSResponse<bool>;