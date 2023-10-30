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

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record ScaleLabelTypeCreateCommand(ScaleLabelTypeModel Entity) : IRequest<ScaleLabelTypeCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_scale_label_type")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<ScaleLabelTypeCreateCommand>
{
    private readonly IScaleLabelTypeService _service;

    /// <inheritdoc />
    public Validator(IScaleLabelTypeService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("ScaleLabelType name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.ScaleLabelType>(name), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class ScaleLabelTypeCreateHandler : IRequestHandler<ScaleLabelTypeCreateCommand, ScaleLabelTypeCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeCreateHandler(
        ILogger<ScaleLabelTypeCreateHandler> logger,
        IScaleLabelTypeService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeCreateResponse> Handle(ScaleLabelTypeCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.ScaleLabelType>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create ScaleLabelType {ScaleLabelTypeName} for user {UserId}", result.Name, result.UserCreatorId);
        return new ScaleLabelTypeCreateResponse { Data = _mapper.Map<ScaleLabelTypeModel>(result) };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeCreateResponse : CQRSResponse<ScaleLabelTypeModel>;