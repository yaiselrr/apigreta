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

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutList;

/// <summary>
/// Get customers by animal
/// </summary>
/// <param name="AnimalId">Animal id</param>
public record CutListGetCustomerByAnimalQuery(long AnimalId) : IRequest<CutListGetCustomerByAnimalResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CutListGetCustomerByAnimal{AnimalId}";
}

/// <inheritdoc />
public class CutListGetCustomerByAnimalHandler : IRequestHandler<CutListGetCustomerByAnimalQuery, CutListGetCustomerByAnimalResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListGetCustomerByAnimalHandler(ICutListService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListGetCustomerByAnimalResponse> Handle(CutListGetCustomerByAnimalQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetCustomerByAnimal(request.AnimalId);
        var data = _mapper.Map<List<CustomerModel>>(entity);
        return new CutListGetCustomerByAnimalResponse { Data = data };
    }
}

/// <inheritdoc />
public record CutListGetCustomerByAnimalResponse : CQRSResponse<List<CustomerModel>>;