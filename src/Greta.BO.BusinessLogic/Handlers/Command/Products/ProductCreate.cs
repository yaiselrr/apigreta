using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Products
{
    public static class ProductCreate
    {
        public record ProductCommand(ProductModel Product) : IRequest<ProductResult>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public record ScaleProductCommand(ScaleProductModel Product) : IRequest<ScaleProductResult>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public record KitProductCommand(KitProductModel Product) : IRequest<KitProductResult>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public class ProductValidator : AbstractValidator<ProductCommand>
        {
            private readonly IProductService _productService;

            public ProductValidator(IProductService productService)
            {
                _productService = productService;
                RuleFor(x => x.Product.Name).NotEmpty().Length(3, 64);
                RuleFor(x => x.Product.DepartmentId).GreaterThan(0).WithMessage("Department id most be more than 0.");
                RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("Category id most be more than 0.");
                //RuleFor(x => x.Product.FamilyId).GreaterThan(0).WithMessage("Family id most be more than 0.");
                RuleFor(x => x.Product.UPC)
                    .Must(UPCConditionalLength).WithMessage("In scale products the UPC must have exactly 5 digits.")
                    .MustAsync(UPCUnique).WithMessage("UPC already exists.");
                //RuleFor(x => x.Product.Name).MustAsync(NameUnique).WithMessage("Product name already exists.");
            }

            private bool UPCConditionalLength(ProductCommand comand, string upc)
            {
                if (comand.Product.ProductType == ProductType.SLP) return comand.Product.UPC.Length == 5;
                return true;
            }

            private async Task<bool> UPCUnique(string upc, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByUPC(upc);
                return upcExist == null;
            }

            private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByName(name);
                return upcExist == null;
            }
        }

        public class ScaleProductValidator : AbstractValidator<ScaleProductCommand>
        {
            private readonly IProductService _productService;

            public ScaleProductValidator(IProductService productService)
            {
                _productService = productService;
                RuleFor(x => x.Product.Name).NotEmpty().Length(3, 64);
                RuleFor(x => x.Product.PLUNumber)
                    .GreaterThanOrEqualTo(1)
                    .LessThanOrEqualTo(99999);
                RuleFor(x => x.Product.DepartmentId).GreaterThan(0).WithMessage("Department id most be more than 0.");
                RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("Category id most be more than 0.");
                RuleFor(x => x.Product.ScaleCategoryId).GreaterThan(0)
                    .WithMessage("Scale Category id most be more than 0.");
                //RuleFor(x => x.Product.FamilyId).GreaterThan(0).WithMessage("Category id most be more than 0.");
                RuleFor(x => x.Product.UPC)
                    .Must(UPCConditionalLength).WithMessage("In scale products the UPC must have exactly 5 digits.")
                    .MustAsync(UPCUnique).WithMessage("UPC already exists.");
                RuleFor(x => x.Product.PLUNumber).MustAsync(PLUNumberUnique).WithMessage("PLU number already exists.");
                //RuleFor(x => x.Product.Name).MustAsync(NameUnique).WithMessage("Product name already exists.");
                // RuleFor(x => x.Product.ScaleLabelTypesId).Must(list => list.Count < 4).WithMessage("The product can have a maximum of 4 scale label type.");
            }

            private bool UPCConditionalLength(ScaleProductCommand comand, string upc)
            {
                if (comand.Product.ProductType == ProductType.SLP) return comand.Product.UPC.Length == 5;
                return true;
            }

            private async Task<bool> PLUNumberUnique(int plu, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByPLUNumber(plu);
                return upcExist == null;
            }

            private async Task<bool> UPCUnique(string upc, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByUPC(upc);
                return upcExist == null;
            }

            private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByName(name);
                return upcExist == null;
            }
        }

        public class ProductHandler : IRequestHandler<ProductCommand, ProductResult>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IProductService _service;

            public ProductHandler(
                ILogger<ProductHandler> logger,
                IProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<ProductResult> Handle(ProductCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.Product>(request.Product);
                // we dont add this yet we need add the image on separate controller
                //entity.ProductImages = await UploadPhotoOnCreateProduct(request.Files);
                var result = await _service.CreateProduct(entity);
                _logger.LogInformation($"Create Standard product {result.Name} for user {result.UserCreatorId}");
                return new ProductResult {Data = _mapper.Map<ProductModel>(result)};
            }
        }

        public class ScaleProductHandler : IRequestHandler<ScaleProductCommand, ScaleProductResult>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IProductService _service;

            public ScaleProductHandler(
                ILogger<ScaleProductHandler> logger,
                IProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<ScaleProductResult> Handle(ScaleProductCommand request,
                CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<ScaleProduct>(request.Product);
                var result = await _service.CreateScaleProduct(entity);
                _logger.LogInformation($"Create Scale product {result.Name} for user {result.UserCreatorId}");
                return new ScaleProductResult {Data = _mapper.Map<ScaleProductModel>(result)};
            }
        }


        public class KitProductHandler : IRequestHandler<KitProductCommand, KitProductResult>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IProductService _service;

            public KitProductHandler(
                ILogger<KitProductHandler> logger,
                IProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<KitProductResult> Handle(KitProductCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.Product>(request.Product);
                var result = await _service.CreateProduct(entity);
                _logger.LogInformation($"Create Kit product {result.Name} for user {result.UserCreatorId}");
                return new KitProductResult {Data = _mapper.Map<KitProductModel>(result)};
            }
        }


        public record ProductResult : CQRSResponse<ProductModel>;

        public record ScaleProductResult : CQRSResponse<ScaleProductModel>;

        public record KitProductResult : CQRSResponse<KitProductModel>;
    }
}