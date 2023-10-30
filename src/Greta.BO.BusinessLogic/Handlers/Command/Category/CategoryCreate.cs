using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record CategoryCreateCommand(CategoryModel Entity) : IRequest<CategoryCreateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Category).ToLower()}")
    };
}

///<inheritdoc/>
public class Validator : AbstractValidator<CategoryCreateCommand>
{
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public Validator(ICategoryService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Category name already exists");
        RuleFor(x => x.Entity)
                    .MustAsync(NameCategoryIdUnique).WithMessage("Category id already exists for this category");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Category>(name), cancellationToken);
        return nameExist == null;
    }

    private async Task<bool> NameCategoryIdUnique(CategoryModel entity, CancellationToken cancellationToken)
    {
        var categoryIds = await _service.Get();
        bool res = !(categoryIds.Any(x => x.CategoryId == entity.CategoryId));
        return res;
    }
}

/// <inheritdoc />
public class CategoryCreateHandler : IRequestHandler<CategoryCreateCommand, CategoryCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;
    private readonly IOnlineStoreService _storeService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CategoryCreateHandler(
        ILogger<CategoryCreateHandler> logger,
        ICategoryService service,
        IOnlineStoreService storeService,
        IMediator mediator,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _storeService = storeService;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CategoryCreateResponse> Handle(CategoryCreateCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Api.Entities.Category>(request.Entity);
        var result = await _service.Post(entity);

        if (result.AddOnlineStore == false)
        {
            return new CategoryCreateResponse { Data = _mapper.Map<CategoryModel>(result) };
        }

        try
        {
            var wixCtegory = LiteCategory.Convert(result, new List<long>());
            await _mediator.Publish(new CategoryCreated(wixCtegory), cancellationToken);
            _logger.LogInformation("Create Category {CategoryName} for user {UserId}", result.Name, result.UserCreatorId);
            return new CategoryCreateResponse { Data = _mapper.Map<CategoryModel>(result) };
        }
        catch (Exception e)
        {
            await _service.Delete(result.Id);
            _logger.LogError(e, "Error creating category {CategoryName}", result.Name);
            throw new BussinessValidationException(new List<string>() { e.Message });
        }
    }
}

/// <inheritdoc />
public record CategoryCreateResponse : CQRSResponse<CategoryModel>;