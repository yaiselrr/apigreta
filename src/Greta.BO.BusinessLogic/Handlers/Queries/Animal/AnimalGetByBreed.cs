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
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Animal;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="breedId"></param>
public record AnimalGetByBreedQuery(long breedId) : IRequest<AnimalGetByBreedResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalGetByBreedHandler : IRequestHandler<AnimalGetByBreedQuery, AnimalGetByBreedResponse>
{
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;
    private readonly ILogger _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalGetByBreedHandler(ILogger<AnimalGetByBreedHandler> logger, IAnimalService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalGetByBreedResponse> Handle(AnimalGetByBreedQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetAnimalByRancher(request.breedId);
        var data = _mapper.Map<AnimalModel>(entity);
        return new AnimalGetByBreedResponse { Data = data };
    }
}

/// <inheritdoc />
public record AnimalGetByBreedResponse : CQRSResponse<AnimalModel>;