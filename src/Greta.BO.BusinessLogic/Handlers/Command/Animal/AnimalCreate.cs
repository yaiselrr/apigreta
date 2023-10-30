using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Animal;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record AnimalCreateCommand(AnimalModel Entity) : IRequest<AnimalCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalCreateValidator : AbstractValidator<AnimalCreateCommand>
{
    private readonly IAnimalService _service;

    /// <inheritdoc />
    public AnimalCreateValidator(IAnimalService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Tag)
                    .NotEmpty();
                
                RuleFor(x => x.Entity.RancherId).GreaterThan(0).WithMessage("Rancher must be selected");
                RuleFor(x => x.Entity.BreedId).GreaterThan(0).WithMessage("Breed must be selected");
    }
}

/// <inheritdoc />
public class AnimalCreateHandler : IRequestHandler<AnimalCreateCommand, AnimalCreateResponse>
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
    public AnimalCreateHandler(
        ILogger<AnimalCreateHandler> logger,
        IAnimalService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalCreateResponse> Handle(AnimalCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Animal>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Animal {AnimalTag} for user {UserId}", result.Tag, result.UserCreatorId);
        return new AnimalCreateResponse { Data = _mapper.Map<AnimalModel>(result) };
    }
}

/// <inheritdoc />
public record AnimalCreateResponse : CQRSResponse<AnimalModel>;