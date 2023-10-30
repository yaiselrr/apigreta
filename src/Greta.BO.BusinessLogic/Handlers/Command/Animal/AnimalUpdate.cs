using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Animal;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record AnimalUpdateCommand
    (long Id, AnimalModel Entity) : IRequest<AnimalUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalUpdateValidator : AbstractValidator<AnimalUpdateCommand>
{
    private readonly IAnimalService _service;

    /// <inheritdoc />
    public AnimalUpdateValidator(IAnimalService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Tag)
                    .NotEmpty();
                
                RuleFor(x => x.Entity.RancherId).GreaterThan(0).WithMessage("Rancher must be selected");
                RuleFor(x => x.Entity.BreedId).GreaterThan(0).WithMessage("Breed must be selected");
    }
}

/// <inheritdoc />
public class AnimalUpdateHandler : IRequestHandler<AnimalUpdateCommand, AnimalUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public AnimalUpdateHandler(
        ILogger<AnimalUpdateHandler> logger,
        IAnimalService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalUpdateResponse> Handle(AnimalUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Animal>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Animal {AnimalId} update successfully", request.Id);
        return new AnimalUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record AnimalUpdateResponse : CQRSResponse<bool>;