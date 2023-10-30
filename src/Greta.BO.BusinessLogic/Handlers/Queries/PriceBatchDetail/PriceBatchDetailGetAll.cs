using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;

/// <summary>
/// Get all entities
/// </summary>
public record PriceBatchDetailGetAllQuery : IRequest<PriceBatchDetailGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_price_batch")
    };
}

/// <inheritdoc />
public class PriceBatchDetailGetAllHandler : IRequestHandler<PriceBatchDetailGetAllQuery, PriceBatchDetailGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public PriceBatchDetailGetAllHandler(IPriceBatchDetailService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailGetAllResponse> Handle(PriceBatchDetailGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new PriceBatchDetailGetAllResponse { Data = _mapper.Map<List<PriceBatchDetailModel>>(entities) };
    }
}

/// <inheritdoc />
public record PriceBatchDetailGetAllResponse : CQRSResponse<List<PriceBatchDetailModel>>;