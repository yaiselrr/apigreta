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
public record VendorOrderDetailUpdateCommand
    (long Id, VendorOrderDetailModel Entity) : IRequest<VendorOrderDetailUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailUpdateHandler : IRequestHandler<VendorOrderDetailUpdateCommand, VendorOrderDetailUpdateResponse>
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
    public VendorOrderDetailUpdateHandler(
        ILogger<VendorOrderDetailUpdateHandler> logger,
        IVendorOrderDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDetailUpdateResponse> Handle(VendorOrderDetailUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.VendorOrderDetail>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        return new VendorOrderDetailUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record VendorOrderDetailUpdateResponse : CQRSResponse<bool>;