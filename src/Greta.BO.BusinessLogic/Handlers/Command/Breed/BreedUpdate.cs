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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record BreedUpdateCommand
    (long Id, BreedModel Entity) : IRequest<BreedUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedUpdateValidator : AbstractValidator<BreedUpdateCommand>
{
    private readonly IBreedService _service;

    /// <inheritdoc />
    public BreedUpdateValidator(IBreedService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Breed name already exists");
    }

    private async Task<bool> NameUnique(BreedUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Breed>(name, command.Id), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class BreedUpdateHandler : IRequestHandler<BreedUpdateCommand, BreedUpdateResponse>
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
    public BreedUpdateHandler(
        ILogger<BreedUpdateHandler> logger,
        IBreedService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BreedUpdateResponse> Handle(BreedUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Breed>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Breed {BreedId} update successfully", request.Id);
        return new BreedUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record BreedUpdateResponse : CQRSResponse<bool>;