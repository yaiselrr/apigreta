using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetailCredit;

public record VendorOrderDetailCreditFilterQuery
    (VendorOrderDetailCreditSearchModel Filter) : IRequest<VendorOrderDetailCreditFilterResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

public class VendorOrderDetailCreditFilterHandler : IRequestHandler<VendorOrderDetailCreditFilterQuery,
    VendorOrderDetailCreditFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailCreditService _service;

    public VendorOrderDetailCreditFilterHandler(IVendorOrderDetailCreditService service,
        IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<VendorOrderDetailCreditFilterResponse> Handle(VendorOrderDetailCreditFilterQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _service.FilterCustom(
            request.Filter);

        return new VendorOrderDetailCreditFilterResponse
            { Data = _mapper.Map<List<VendorOrderDetailCreditModel>>(entities) };
    }
}

public record VendorOrderDetailCreditFilterResponse : CQRSResponse<List<VendorOrderDetailCreditModel>>;