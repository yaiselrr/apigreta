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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record GrindUpdateCommand
    (long Id, GrindModel Entity) : IRequest<GrindUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindUpdateValidator : AbstractValidator<GrindUpdateCommand>
{
    private readonly IGrindService _service;

    /// <inheritdoc />
    public GrindUpdateValidator(IGrindService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Grind name already exists");
    }

    private async Task<bool> NameUnique(GrindUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var grindExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Grind>(name, command.Id), cancellationToken);
        return grindExist == null;
    }
}

/// <inheritdoc />
public class GrindUpdateHandler : IRequestHandler<GrindUpdateCommand, GrindUpdateResponse>
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
    public GrindUpdateHandler(
        ILogger<GrindUpdateHandler> logger,
        IGrindService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GrindUpdateResponse> Handle(GrindUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Grind>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Grind {GrindId} update successfully", request.Id);
        return new GrindUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record GrindUpdateResponse : CQRSResponse<bool>;