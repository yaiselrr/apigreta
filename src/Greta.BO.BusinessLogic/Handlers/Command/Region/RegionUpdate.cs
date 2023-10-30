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

namespace Greta.BO.BusinessLogic.Handlers.Command.Region;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record RegionUpdateCommand
    (long Id, RegionModel Entity) : IRequest<RegionUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionUpdateValidator : AbstractValidator<RegionUpdateCommand>
{
    private readonly IRegionService _service;

    /// <inheritdoc />
    public RegionUpdateValidator(IRegionService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Region name already exists.");
    }

    private async Task<bool> NameUnique(RegionUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var regionExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Region>(name, command.Id), cancellationToken);
        return regionExist == null;
    }
}

/// <inheritdoc />
public class RegionUpdateHandler : IRequestHandler<RegionUpdateCommand, RegionUpdateResponse>
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
    public RegionUpdateHandler(
        ILogger<RegionUpdateHandler> logger,
        IRegionService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RegionUpdateResponse> Handle(RegionUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Region>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Region {RegionId} update successfully", request.Id);
        return new RegionUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record RegionUpdateResponse : CQRSResponse<bool>;