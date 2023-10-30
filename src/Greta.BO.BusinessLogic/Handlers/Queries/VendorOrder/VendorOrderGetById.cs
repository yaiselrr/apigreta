using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;

/// <summary>
/// Return vendor order by id, throw exception if exists more than 1
/// </summary>
/// <param name="Id">Vendor Order Id</param>
public record VendorOrderGetByIdQuery(long Id) : IRequest<VendorOrderGetByIdResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderGetByIdHandler : IRequestHandler<VendorOrderGetByIdQuery, VendorOrderGetByIdResponse>
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
    public VendorOrderGetByIdHandler(ILogger<VendorOrderGetByIdHandler> logger, IVendorOrderService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderGetByIdResponse> Handle(VendorOrderGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.VendorOrder>(request.Id), cancellationToken);
        var data = _mapper.Map<VendorOrderModel>(entity);
        return new VendorOrderGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record VendorOrderGetByIdResponse : CQRSResponse<VendorOrderModel>;