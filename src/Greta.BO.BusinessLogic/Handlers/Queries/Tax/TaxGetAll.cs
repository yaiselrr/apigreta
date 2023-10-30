using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Tax;

/// <summary>
/// Get all entities
/// </summary>
public record TaxGetAllQuery : IRequest<TaxGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxGetAllHandler : IRequestHandler<TaxGetAllQuery, TaxGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public TaxGetAllHandler(ITaxService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TaxGetAllResponse> Handle(TaxGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        var data = _mapper.Map<List<TaxModel>>(entities);
        return data == null ? null : new TaxGetAllResponse { Data = data };
    }
}

/// <inheritdoc />
public record TaxGetAllResponse : CQRSResponse<List<TaxModel>>;