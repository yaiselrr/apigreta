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

namespace Greta.BO.BusinessLogic.Handlers.Command.Breed;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record BreedCreateCommand(BreedModel Entity) : IRequest<BreedCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedCreateValidator : AbstractValidator<BreedCreateCommand>
{
    private readonly IBreedService _service;

    /// <inheritdoc />
    public BreedCreateValidator(IBreedService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Breed name already exists");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Breed>(name), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class BreedCreateHandler : IRequestHandler<BreedCreateCommand, BreedCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public BreedCreateHandler(
        ILogger<BreedCreateHandler> logger,
        IBreedService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BreedCreateResponse> Handle(BreedCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Breed>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Breed {BreedName} for user {UserId}", result.Name, result.UserCreatorId);
        return new BreedCreateResponse { Data = _mapper.Map<BreedModel>(result) };
    }
}

/// <inheritdoc />
public record BreedCreateResponse : CQRSResponse<BreedModel>;