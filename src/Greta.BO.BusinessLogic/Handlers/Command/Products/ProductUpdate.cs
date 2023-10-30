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
    public static class ProductUpdate
    {
        public record ProductCommand(long Id, ProductModel Product) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public record ScaleProductCommand(long Id, ScaleProductModel Product) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
            };
        }

        public record KitProductCommand(long Id, KitProductModel Product) : IRequest<Response>, IAuthorizable
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
                RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("Category id most be more than 0.");
                RuleFor(x => x.Product.FamilyId).GreaterThan(0).WithMessage("Family id most be more than 0.");
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

            private async Task<bool> UPCUnique(ProductCommand comand, string upc, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByUPC(upc, comand.Id);
                return upcExist == null;
            }

            private async Task<bool> NameUnique(ProductCommand comand, string name, CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByName(name, comand.Id);
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
                RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("Category id most be more than 0.");
                RuleFor(x => x.Product.FamilyId).GreaterThan(0).WithMessage("Family id most be more than 0.");
                RuleFor(x => x.Product.UPC)
                    .Must(UPCConditionalLength).WithMessage("In scale products the UPC must have exactly 5 digits.")
                    .MustAsync(UPCUnique).WithMessage("UPC already exists.");
                RuleFor(x => x.Product.PLUNumber).MustAsync(PLUNumberUnique).WithMessage("PLU number already exists.");
                //RuleFor(x => x.Product.Name).MustAsync(NameUnique).WithMessage("Product name already exists.");
                //RuleFor(x => x.Product.ScaleLabelTypesId).Must(list => list.Count <= 4).WithMessage("The product can have a maximum of 4 scale label type.");
            }

            private bool UPCConditionalLength(ScaleProductCommand comand, string upc)
            {
                if (comand.Product.ProductType == ProductType.SLP) return comand.Product.UPC.Length == 5;
                return true;
            }

            private async Task<bool> PLUNumberUnique(ScaleProductCommand comand, int plu,
                CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByPLUNumber(plu, comand.Id);
                return upcExist == null;
            }

            private async Task<bool> UPCUnique(ScaleProductCommand comand, string upc,
                CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByUPC(upc, comand.Id);
                return upcExist == null;
            }

            private async Task<bool> NameUnique(ScaleProductCommand comand, string name,
                CancellationToken cancellationToken)
            {
                var upcExist = await _productService.GetProductByName(name, comand.Id);
                return upcExist == null;
            }
        }

        public class ProductHandler : IRequestHandler<ProductCommand, Response>
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

            public async Task<Response> Handle(ProductCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Product>(request.Product);
                var success = await _service.UpdateProduct(request.Id, entity);
                _logger.LogInformation($"Product {request.Id} update successfully");
                return new Response { Data = success };
            }
        }

        public class ScaleProductHandler : IRequestHandler<ScaleProductCommand, Response>
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

            public async Task<Response> Handle(ScaleProductCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<ScaleProduct>(request.Product);
                var success = await _service.UpdateScaleProduct(request.Id, entity);
                _logger.LogInformation($"Scale Product {request.Id} update successfully");
                return new Response { Data = success };
            }
        }

        public class KitProductHandler : IRequestHandler<KitProductCommand, Response>
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

            public async Task<Response> Handle(KitProductCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<KitProduct>(request.Product);
                var success = await _service.UpdateProduct(request.Id, entity);
                _logger.LogInformation($"Product {request.Id} update successfully");
                return new Response { Data = success };
            }
        }


        public record Response : CQRSResponse<bool>;
    }
}