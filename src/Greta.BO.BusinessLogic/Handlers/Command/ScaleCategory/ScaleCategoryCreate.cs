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
/// Create entity
/// </summary>
/// <param name="Entity">Entity to create</param>
public record ScaleCategoryCreateCommand(ScaleCategoryModel Entity) : IRequest<ScaleCategoryCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_scale_category")
    };
}

/// <inheritdoc />
public class ScaleCategoryCreateValidator : AbstractValidator<ScaleCategoryCreateCommand>
{
    private readonly IScaleCategoryService _service;

    /// <inheritdoc />
    public ScaleCategoryCreateValidator(IScaleCategoryService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Scale Category name already exists.");
        
        RuleFor(x => x.Entity.CategoryId)
            .NotEmpty()
            .MustAsync(IdUnique).WithMessage("Scale Category id already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var scaleCategoryName = await _service.Get(new CheckUniqueNameSpec<Api.Entities.ScaleCategory>(name), cancellationToken);
        return scaleCategoryName == null;
    }
    
    private async Task<bool> IdUnique(int id, CancellationToken cancellationToken)
    {
        var scaleCategoryId = await _service.GetByScaleCategoryId(id);
        return scaleCategoryId == null;
    }
}

/// <inheritdoc />
public class ScaleCategoryCreateHandler : IRequestHandler<ScaleCategoryCreateCommand, ScaleCategoryCreateResponse>
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
    public ScaleCategoryCreateHandler(
        ILogger<ScaleCategoryCreateHandler> logger,
        IScaleCategoryService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryCreateResponse> Handle(ScaleCategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.ScaleCategory>(request.Entity);
        if (entity.ParentId.HasValue && entity.ParentId.Value == -1)
            entity.ParentId = null;
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Scale Category {ScaleCategoryName} for user {UserId}", result.Name, result.UserCreatorId);
        return new ScaleCategoryCreateResponse { Data = _mapper.Map<ScaleCategoryModel>(result) };
    }
}

/// <inheritdoc />
public record ScaleCategoryCreateResponse : CQRSResponse<ScaleCategoryModel>;