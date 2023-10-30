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
/// Get entity by rancherId
/// </summary>
/// <param name="rancherId"></param>
public record AnimalGetByRancherQuery(long rancherId) : IRequest<AnimalGetByRancherResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalGetByRancherHandler : IRequestHandler<AnimalGetByRancherQuery, AnimalGetByRancherResponse>
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
    public AnimalGetByRancherHandler(ILogger<AnimalGetByRancherHandler> logger, IAnimalService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalGetByRancherResponse> Handle(AnimalGetByRancherQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetAnimalByRancher(request.rancherId);
        var data = _mapper.Map<AnimalModel>(entity);
        return new AnimalGetByRancherResponse { Data = data };
    }
}

/// <inheritdoc />
public record AnimalGetByRancherResponse : CQRSResponse<AnimalModel>;