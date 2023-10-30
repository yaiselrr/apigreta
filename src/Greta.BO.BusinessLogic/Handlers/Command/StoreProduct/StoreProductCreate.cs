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

public record StoreProductCreateCommand(StoreProductCreateModel storeproduct) : IRequest<StoreProductCreateResponse>,
    IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"associate_product_store")
    };
}

public class StoreProductCreateValidator : AbstractValidator<StoreProductCreateCommand>
{
    private readonly IStoreProductService _service;

    public StoreProductCreateValidator(IStoreProductService service)
    {
        _service = service;
        RuleFor(x => x.storeproduct.ProductId).GreaterThan(0).WithMessage("Product id must be greater than 0.");
        //RuleFor(x => x.storeproduct.StoreId).GreaterThan(0).WithMessage("Store id most be more than 0.");
    }
}

public class StoreProductCreateHandler : IRequestHandler<StoreProductCreateCommand, StoreProductCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IStoreProductService _service;
    private readonly IStoreService _serviceStore;

    public StoreProductCreateHandler(
        ILogger<StoreProductCreateHandler> logger,
        IStoreProductService service,
        IStoreService serviceStore,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _serviceStore = serviceStore;
        _mapper = mapper;
    }

    public async Task<StoreProductCreateResponse> Handle(StoreProductCreateCommand request,
        CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Api.Entities.StoreProduct>(request.storeproduct);


        if (request.storeproduct.AllStores)
        {
            var stores = await _serviceStore.GetAllIds();
            var result = await _service.CreateOnMultipleStores(stores, entity);
            _logger.LogInformation("Create Multiples StoreProduct");
            return new StoreProductCreateResponse { Data = request.storeproduct };
        }

        if (request.storeproduct.RegionId > 0)
        {
            var stores = await _serviceStore.GetByRegion(request.storeproduct.RegionId);
            var result = await _service.CreateOnMultipleStores(stores, entity);
            _logger.LogInformation("Create Multiples StoreProduct");
            return new StoreProductCreateResponse { Data = request.storeproduct };
        }
        else
        {
            var result = await _service.Post(entity);
            _logger.LogInformation("Create StoreProduct for user {UserCreatorId}", result.UserCreatorId);
            return new StoreProductCreateResponse { Data = _mapper.Map<StoreProductCreateModel>(result) };
        }
    }
}

public record StoreProductCreateResponse : CQRSResponse<StoreProductCreateModel>;