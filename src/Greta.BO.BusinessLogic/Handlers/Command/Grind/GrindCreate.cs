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

namespace Greta.BO.BusinessLogic.Handlers.Command.Grind;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record GrindCreateCommand(GrindModel Entity) : IRequest<GrindCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindCreateValidator : AbstractValidator<GrindCreateCommand>
{
    private readonly IGrindService _service;

    /// <inheritdoc />
    public GrindCreateValidator(IGrindService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Grind name already exists");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var grindExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Grind>(name), cancellationToken);
        return grindExist == null;
    }
}

/// <inheritdoc />
public class GrindCreateHandler : IRequestHandler<GrindCreateCommand, GrindCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public GrindCreateHandler(
        ILogger<GrindCreateHandler> logger,
        IGrindService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GrindCreateResponse> Handle(GrindCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Grind>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Grind {GrindName} for user {UserId}", result.Name, result.UserCreatorId);
        return new GrindCreateResponse { Data = _mapper.Map<GrindModel>(result) };
    }
}

/// <inheritdoc />
public record GrindCreateResponse : CQRSResponse<GrindModel>;