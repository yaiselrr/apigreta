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

namespace Greta.BO.BusinessLogic.Handlers.Command.Region;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record RegionCreateCommand(RegionModel Entity) : IRequest<RegionCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionCreateValidator : AbstractValidator<RegionCreateCommand>
{
    private readonly IRegionService _service;

    /// <inheritdoc />
    public RegionCreateValidator(IRegionService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Region name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var regionExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Region>(name), cancellationToken);
        return regionExist == null;
    }
}

/// <inheritdoc />
public class RegionCreateHandler : IRequestHandler<RegionCreateCommand, RegionCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RegionCreateHandler(
        ILogger<RegionCreateHandler> logger,
        IRegionService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RegionCreateResponse> Handle(RegionCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Region>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Region {RegionName} for user {UserId}", result.Name, result.UserCreatorId);
        return new RegionCreateResponse { Data = _mapper.Map<RegionModel>(result) };
    }
}

/// <inheritdoc />
public record RegionCreateResponse : CQRSResponse<RegionModel>;