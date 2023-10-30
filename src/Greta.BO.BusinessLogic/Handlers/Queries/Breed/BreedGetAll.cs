using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Breed;

/// <summary>
/// Get all entities
/// </summary>
public record BreedGetAllQuery : IRequest<BreedGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedGetAllHandler : IRequestHandler<BreedGetAllQuery, BreedGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public BreedGetAllHandler(IBreedService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BreedGetAllResponse> Handle(BreedGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new BreedGetAllResponse {Data = _mapper.Map<List<BreedModel>>(entities)};
    }
}

/// <inheritdoc />
public record BreedGetAllResponse : CQRSResponse<List<BreedModel>>;