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

public record VendorOrderDetailChangeStateCommand(long Id, bool State) : IRequest<VendorOrderDetailChangeStateResponse>,
    IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

public class VendorOrderDetailChangeStateHandler : IRequestHandler<VendorOrderDetailChangeStateCommand,
    VendorOrderDetailChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;

    public VendorOrderDetailChangeStateHandler(ILogger<VendorOrderDetailChangeStateHandler> logger, IVendorOrderDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<VendorOrderDetailChangeStateResponse> Handle(VendorOrderDetailChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {State}", request.Id, request.State);
        return new VendorOrderDetailChangeStateResponse { Data = result };
    }
}

public record VendorOrderDetailChangeStateResponse : CQRSResponse<bool>;