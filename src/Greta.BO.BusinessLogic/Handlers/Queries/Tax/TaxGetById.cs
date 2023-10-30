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
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record TaxGetByIdQuery(long Id) : IRequest<TaxGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Tax).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"TaxGetById{Id}";
}

/// <inheritdoc />
public class TaxGetByIdHandler : IRequestHandler<TaxGetByIdQuery, TaxGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public TaxGetByIdHandler(ITaxService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TaxGetByIdResponse> Handle(TaxGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<TaxModel>(entity);
        return data == null ? null : new TaxGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record TaxGetByIdResponse : CQRSResponse<TaxModel>;