using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="ChangeAllProducts">Bool Value</param>
/// <param name="Entity">New entity</param>
public record CategoryUpdateCommand
    (long Id, bool ChangeAllProducts, CategoryModel Entity) : IRequest<CategoryUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Category).ToLower()}")
    };
}

///<inheritdoc/>
public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateCommand>
{
    private readonly ICategoryService _service;

    /// <inheritdoc />
    public CategoryUpdateValidator(ICategoryService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Category name already exists");

        RuleFor(x => x.Entity.Description)
            .NotEmpty()
            .Length(3, 254);
        RuleFor(x => x.Entity)
    .MustAsync(NameCategoryIdUnique).WithMessage("Category id already exists for this category");
    }

    private async Task<bool> NameUnique(CategoryUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var upcExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Category>(name, command.Id), cancellationToken);
        return upcExist == null;
    }
    private async Task<bool> NameCategoryIdUnique(CategoryUpdateCommand command, CategoryModel entity, CancellationToken cancellationToken)
    {
        var categoryIds = await _service.Get();
        bool res = !(categoryIds.Any(x => x.CategoryId == entity.CategoryId && x.Id != command.Id));
        return res;
    }
}

/// <inheritdoc />
public class CategoryUpdateHandler : IRequestHandler<CategoryUpdateCommand, CategoryUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;
    private readonly IMediator _mediator;
    private readonly ISynchroService _synchroService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="synchroService"></param>
    /// <param name="mapper"></param>
    public CategoryUpdateHandler(
        ILogger<CategoryUpdateHandler> logger,
        ICategoryService service,
        IMediator mediator,
        ISynchroService synchroService,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
        _synchroService = synchroService;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CategoryUpdateResponse> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Category>(request.Entity);

        var originalCategory = await _service.GetCategoryWithProduct(request.Id);
        var oldDepartment = originalCategory.DepartmentId;


        if (request.ChangeAllProducts &&
            originalCategory.Products != null
            && (
        entity.DisplayStockOnPosButton != originalCategory.DisplayStockOnPosButton ||
        entity.Modifier != originalCategory.Modifier ||
        entity.AddOnlineStore != originalCategory.AddOnlineStore ||
        entity.NoDiscountAllowed != originalCategory.NoDiscountAllowed ||
        entity.MinimumAge != originalCategory.MinimumAge ||
        entity.AllowZeroStock != originalCategory.AllowZeroStock ||
        entity.NoPriceOnShelfTag != originalCategory.NoPriceOnShelfTag ||
        entity.PrintShelfTag != originalCategory.PrintShelfTag ||
        entity.SnapEBT != originalCategory.SnapEBT ||
        entity.DefaulShelfTagId != originalCategory.DefaulShelfTagId ||
        entity.PromptPriceAtPOS != originalCategory.PromptPriceAtPOS ||
        entity.VisibleOnPos != originalCategory.VisibleOnPos
        )
        )
        {
            _logger.LogInformation("Updating child products");
            foreach (var product in originalCategory.Products)
            {
                product.Modifier = entity.Modifier;
                product.PrintShelfTag = entity.PrintShelfTag;
                product.PosVisible = entity.VisibleOnPos;
                product.PromptPriceAtPOS = entity.PromptPriceAtPOS;
                product.MinimumAge = entity.MinimumAge ?? null;
                product.DefaulShelfTagId = entity.DefaulShelfTagId;
                product.SnapEBT = entity.SnapEBT;
                product.NoPriceOnShelfTag = entity.NoPriceOnShelfTag;
                product.AllowZeroStock = entity.AllowZeroStock;
                product.NoDiscountAllowed = entity.NoDiscountAllowed;
                product.AddOnlineStore = entity.AddOnlineStore;
                product.DisplayStockOnPosButton = entity.DisplayStockOnPosButton;

                foreach (var s in product.StoreProducts)
                {
                    switch (product.ProductType)
                    {
                        case ProductType.SLP:
                            await _synchroService.AddSynchroToStore(
                s.StoreId,
                LiteScaleProduct.Convert1((ScaleProduct)product, s),
                SynchroType.UPDATE
                            );
                            break;
                        case ProductType.KPT:
                            await _synchroService.AddSynchroToStore(
                s.StoreId,
                LiteKitProduct.Convert2((KitProduct)product, s),
                SynchroType.UPDATE
                            );
                            break;
                        default:
                            await _synchroService.AddSynchroToStore(
                s.StoreId,
                LiteProduct.Convert(product, s),
                SynchroType.UPDATE
                            );
                            break;
                    }
                }
            }

            entity.Products = originalCategory.Products;
        }

        var success = await _service.Put(request.Id, entity);

        var wixCategory = LiteCategory.Convert(entity, new List<long>());
        await _mediator.Publish(new CategoryUpdated(wixCategory, oldDepartment, originalCategory.Products), cancellationToken);

        _logger.LogInformation("Category {RequestId} update successfully", request.Id);
        return new CategoryUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record CategoryUpdateResponse : CQRSResponse<bool>;