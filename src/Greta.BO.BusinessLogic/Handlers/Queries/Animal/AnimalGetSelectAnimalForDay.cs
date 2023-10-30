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
/// Get entity by Model
/// </summary>
/// <param name="Model"></param>
public record AnimalGetSelectAnimalForDayQuery(ValidateADayModel Model) : IRequest<AnimalGetSelectAnimalForDayResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalGetSelectAnimalForDayHandler : IRequestHandler<AnimalGetSelectAnimalForDayQuery, AnimalGetSelectAnimalForDayResponse>
{
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalGetSelectAnimalForDayHandler(IAnimalService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalGetSelectAnimalForDayResponse> Handle(AnimalGetSelectAnimalForDayQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetSelectScheduleForDay(request.Model.Date);
        var data = _mapper.Map<BreedModel>(entity);
        return new AnimalGetSelectAnimalForDayResponse { Data = data };
    }
}

/// <inheritdoc />
public record AnimalGetSelectAnimalForDayResponse : CQRSResponse<BreedModel>;