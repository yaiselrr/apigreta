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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Breed;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record BreedGetByIdQuery(long Id) : IRequest<BreedGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Breed).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"BreedGetById{Id}";
}

/// <inheritdoc />
public class BreedGetByIdHandler : IRequestHandler<BreedGetByIdQuery, BreedGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public BreedGetByIdHandler(IBreedService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BreedGetByIdResponse> Handle(BreedGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.Breed>(request.Id), cancellationToken);
        var data = _mapper.Map<BreedModel>(entity);
        return new BreedGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record BreedGetByIdResponse : CQRSResponse<BreedModel>;