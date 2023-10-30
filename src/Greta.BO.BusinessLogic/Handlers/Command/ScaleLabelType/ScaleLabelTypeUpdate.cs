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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record ScaleLabelTypeUpdateCommand
    (long Id, ScaleLabelTypeModel Entity) : IRequest<ScaleLabelTypeUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_scale_label_type")
    };
}

/// <inheritdoc />
public class ScaleLabelTypeUpdateValidator : AbstractValidator<ScaleLabelTypeUpdateCommand>
{
    private readonly IScaleLabelTypeService _service;

    /// <inheritdoc />
    public ScaleLabelTypeUpdateValidator(IScaleLabelTypeService service)
    {
        _service = service;

        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("ScaleLabelType name already exists.");
    }

    private async Task<bool> NameUnique(ScaleLabelTypeUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.ScaleLabelType>(name, command.Id), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class ScaleLabelTypeUpdateHandler : IRequestHandler<ScaleLabelTypeUpdateCommand, ScaleLabelTypeUpdateResponse>
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
    public ScaleLabelTypeUpdateHandler(
        ILogger<ScaleLabelTypeUpdateHandler> logger,
        IScaleLabelTypeService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeUpdateResponse> Handle(ScaleLabelTypeUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.ScaleLabelType>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("ScaleLabelType {Id} update successfully", request.Id);
        return new ScaleLabelTypeUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeUpdateResponse : CQRSResponse<bool>;