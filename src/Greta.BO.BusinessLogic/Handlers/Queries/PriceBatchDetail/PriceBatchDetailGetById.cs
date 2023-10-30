using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record PriceBatchDetailGetByIdQuery(long Id) : IRequest<PriceBatchDetailGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_price_batch")
    };

    /// <inheritdoc />
    public string CacheKey => $"PriceBatchDetailGetById{Id}";
}

/// <inheritdoc />
public class PriceBatchDetailGetByIdHandler : IRequestHandler<PriceBatchDetailGetByIdQuery, PriceBatchDetailGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public PriceBatchDetailGetByIdHandler(IPriceBatchDetailService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailGetByIdResponse> Handle(PriceBatchDetailGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.PriceBatchDetail>(request.Id), cancellationToken);
        var data = _mapper.Map<PriceBatchDetailModel>(entity);
        return new PriceBatchDetailGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record PriceBatchDetailGetByIdResponse : CQRSResponse<PriceBatchDetailModel>;