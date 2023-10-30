using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Tax;

/// <summary>
/// Get Tax entity by store
/// </summary>
/// <param name="Id">Store id</param>
public record TaxGetByStoreQuery(long Id) : IRequest<TaxGetByStoreResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Tax).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"TaxGetByStore{Id}";
}

/// <inheritdoc />
public class TaxGetByStoreHandler : IRequestHandler<TaxGetByStoreQuery, TaxGetByStoreResponse>
{
    private readonly IMapper _mapper;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public TaxGetByStoreHandler(ITaxService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TaxGetByStoreResponse> Handle(TaxGetByStoreQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetTaxByStore(request.Id);
        var data = _mapper.Map<List<TaxModel>>(entity);
        return data.Count == 0 ? null : new TaxGetByStoreResponse { Data = data };
    }
}

/// <inheritdoc />
public record TaxGetByStoreResponse : CQRSResponse<List<TaxModel>>;