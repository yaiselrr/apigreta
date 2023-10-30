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

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleCategory;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">Entity to update</param>
public record ScaleCategoryUpdateCommand
    (long Id, ScaleCategoryModel Entity) : IRequest<ScaleCategoryUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_scale_category")
    };
}

/// <inheritdoc />
public class ScaleCategoryUpdateValidator : AbstractValidator<ScaleCategoryUpdateCommand>
{
    private readonly IScaleCategoryService _service;

    /// <inheritdoc />
    public ScaleCategoryUpdateValidator(IScaleCategoryService service)
    {
        _service = service;

        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Scale Category name already exists.");
    }

    private async Task<bool> NameUnique(ScaleCategoryUpdateCommand command, string name,
        CancellationToken cancellationToken)
    {
        var upcExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.ScaleCategory>(name, command.Id),
            cancellationToken);
        return upcExist == null;
    }
}

/// <inheritdoc />
public class ScaleCategoryUpdateHandler : IRequestHandler<ScaleCategoryUpdateCommand, ScaleCategoryUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleCategoryUpdateHandler(
        ILogger<ScaleCategoryUpdateHandler> logger,
        IScaleCategoryService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryUpdateResponse> Handle(ScaleCategoryUpdateCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Entity.ParentId == -1)
            request.Entity.ParentId = null;
        var entity = _mapper.Map<Api.Entities.ScaleCategory>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Scale Category {ScaleCategoryId} update successfully", request.Id);
        return new ScaleCategoryUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record ScaleCategoryUpdateResponse : CQRSResponse<bool>;