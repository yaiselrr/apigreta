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
public record AnimalValidateForDayQuery(ValidateADayModel Model) : IRequest<AnimalValidateForDayResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalValidateForDayHandler : IRequestHandler<AnimalValidateForDayQuery, AnimalValidateForDayResponse>
{
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public AnimalValidateForDayHandler(IAnimalService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public async Task<AnimalValidateForDayResponse> Handle(AnimalValidateForDayQuery request,
        CancellationToken cancellationToken = default)
    {
        var data = await _service.ValidateForDay(request.Model.Date);
        return new AnimalValidateForDayResponse() { Data = data };
    }
}

/// <inheritdoc />
public record AnimalValidateForDayResponse : CQRSResponse<bool>;