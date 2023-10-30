using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;

public record StoreProductUpdateCommand(long Id, StoreProductModel storeproduct) : IRequest<StoreProductUpdateResponse>,
    IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"associate_product_store")
    };
}

public class StoreProductUpdateValidator : AbstractValidator<StoreProductUpdateCommand>
{
    private readonly IStoreProductService _service;

    public StoreProductUpdateValidator(IStoreProductService service)
    {
        _service = service;
        RuleFor(x => x.storeproduct.ProductId).GreaterThan(0).WithMessage("Product id most be more than 0.");
        RuleFor(x => x.storeproduct.StoreId).GreaterThan(0).WithMessage("Store id most be more than 0.");
    }
}

public class StoreProductUpdateHandler : IRequestHandler<StoreProductUpdateCommand, StoreProductUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IStoreProductService _service;

    public StoreProductUpdateHandler(
        ILogger<StoreProductUpdateHandler> logger,
        IStoreProductService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<StoreProductUpdateResponse> Handle(StoreProductUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.StoreProduct>(request.storeproduct);
        var entityDb = await _service.Get(request.Id);

        entityDb.Price = entity.Price;
        entityDb.Price2 = entity.Price2;
        entityDb.WebPrice = entity.WebPrice;
        entityDb.Cost = entity.Cost;
        entityDb.GrossProfit = entity.GrossProfit;
        entityDb.GrossProfit2 = entity.GrossProfit2;
        entityDb.WebGrossProfit = entity.WebGrossProfit;
        entityDb.NoCategoryChange = entity.NoCategoryChange;
        entityDb.Taxs = entity.Taxs;
        if(entity.NoCategoryChange)
            entityDb.TargetGrossProfit = entity.TargetGrossProfit;

        var success = await _service.Put(request.Id, entityDb);
        _logger.LogInformation("Store Product {Id} update successfully", request.Id);
        return new StoreProductUpdateResponse { Data = success };
    }
}

public record StoreProductUpdateResponse : CQRSResponse<bool>;