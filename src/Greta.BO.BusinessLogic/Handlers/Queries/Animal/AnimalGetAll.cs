using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Animal;

/// <summary>
/// Get all entities
/// </summary>
public record AnimalGetAllQuery : IRequest<AnimalGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalGetAllHandler : IRequestHandler<AnimalGetAllQuery, AnimalGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalGetAllHandler(IAnimalService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalGetAllResponse> Handle(AnimalGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new AnimalGetAllResponse {Data = _mapper.Map<List<AnimalModel>>(entities)};
    }
}

/// <inheritdoc />
public record AnimalGetAllResponse : CQRSResponse<List<AnimalModel>>;