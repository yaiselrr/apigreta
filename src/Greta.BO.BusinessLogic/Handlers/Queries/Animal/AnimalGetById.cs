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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Animal;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record AnimalGetByIdQuery(long Id) : IRequest<AnimalGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"AnimalGetById{Id}";
}

/// <inheritdoc />
public class AnimalGetByIdHandler : IRequestHandler<AnimalGetByIdQuery, AnimalGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalGetByIdHandler(IAnimalService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalGetByIdResponse> Handle(AnimalGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<AnimalModel>(entity);
        return new AnimalGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record AnimalGetByIdResponse : CQRSResponse<AnimalModel>;