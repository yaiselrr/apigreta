using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.StoreProductSpecs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Update Category with TargetGrossProfit and update price and grossProfit of products
/// </summary>
/// <param name="Id"></param>
/// <param name="CategoryGrossProfitModel"></param>
public record CategorySetTargetGrossProfitCommand
    (long Id, CategoryTargetGrossProfitModel CategoryGrossProfitModel) : IRequest<CategorySetTargetGrossProfitResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Category).ToLower()}")
    };
}
/*
public class CategorySetTargetGrossProfitValidator : AbstractValidator<CategorySetTargetGrossProfitCommand>
{
    private readonly ICategoryService _service;    
    public CategorySetTargetGrossProfitValidator(ICategoryService service)
    {        
        _service = service;
        RuleFor(x => x.CategoryGrossProfitModel.AllStores)
            .SetAsyncValidator(x => x.CategoryGrossProfitModel.AllStores)
            .WithMessage("You must select All stores, a region or a specific store.");
    }
}
*/
        
/// <inheritdoc />
public class CategorySetTargetGrossProfitHandler : IRequestHandler<CategorySetTargetGrossProfitCommand, CategorySetTargetGrossProfitResponse>
{
    private readonly ILogger _logger;
    private readonly ICategoryService _serviceCategory;
    private readonly IStoreProductService _serviceStoreProduct;

    /// <summary>
    /// Set gross profit to products
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceCategory"></param>
    /// <param name="serviceStoreProduct"></param>
    public CategorySetTargetGrossProfitHandler(
        ILogger<CategorySetTargetGrossProfitHandler> logger,
        ICategoryService serviceCategory,
        IStoreProductService serviceStoreProduct)
    {
        _logger = logger;
        _serviceCategory = serviceCategory;
        _serviceStoreProduct = serviceStoreProduct;
    }

    /// <inheritdoc />
    public async Task<CategorySetTargetGrossProfitResponse> Handle(CategorySetTargetGrossProfitCommand request, CancellationToken cancellationToken)
    {
        if ( request.Id < 1 )
        {
            throw new BusinessLogicException("Parameters out of bounds");
        }
        
        //Update
        var entity = await _serviceCategory.Get(request.Id);
        entity.TargetGrossProfit = request.CategoryGrossProfitModel.TargetGrossProfit;
        var success = await _serviceCategory.Put(request.Id, entity);
        
        var spec = new StoreProductsByCategoryAndStoreSpec(request.Id, request.CategoryGrossProfitModel.AllStores,
            request.CategoryGrossProfitModel.RegionId,  request.CategoryGrossProfitModel.StoreId);
        var storeProductEntities = await _serviceStoreProduct.Get(spec, cancellationToken);

        foreach (var sp in storeProductEntities)
        {
            if(!sp.NoCategoryChange)
                _serviceStoreProduct.UpdateValuesByTargetGrossProfit(sp, entity.TargetGrossProfit);
            sp.TargetGrossProfit = entity.TargetGrossProfit;
            await _serviceStoreProduct.Put(sp.Id, sp);
        }

        _logger.LogInformation("Category {RequestId} update successfully", request.Id);
        return new CategorySetTargetGrossProfitResponse { Data = success };
    }
}

/// <inheritdoc />
public record CategorySetTargetGrossProfitResponse : CQRSResponse<bool>;