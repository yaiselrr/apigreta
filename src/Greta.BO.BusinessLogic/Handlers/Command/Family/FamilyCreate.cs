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

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record FamilyCreateCommand(FamilyModel Entity) : IRequest<FamilyCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<FamilyCreateCommand>
{
    private readonly IFamilyService _service;

    /// <inheritdoc />
    public Validator(IFamilyService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Family name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var familyExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Family>(name), cancellationToken);
        return familyExist == null;
    }
}

/// <inheritdoc />
public class FamilyCreateHandler : IRequestHandler<FamilyCreateCommand, FamilyCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyCreateHandler(
        ILogger<FamilyCreateHandler> logger,
        IFamilyService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<FamilyCreateResponse> Handle(FamilyCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Family>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Family {FamilyName} for user {UserId}", result.Name, result.UserCreatorId);
        return new FamilyCreateResponse { Data = _mapper.Map<FamilyModel>(result) };
    }
}

/// <inheritdoc />
public record FamilyCreateResponse : CQRSResponse<FamilyModel>;