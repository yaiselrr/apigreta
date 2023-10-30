using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Family;

/// <inheritdoc />
public record ProductFilterNotIncludedInFamilyQuery(long FamilyId, int CurrentPage, int PageSize,
    ProductSearchModel Filter) : IRequest<ProductFilterNotIncludedInFamilyResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_product")
    };
}

/// <inheritdoc />
public class ProductFilterNotIncludedInFamilyValidator : AbstractValidator<ProductFilterNotIncludedInFamilyQuery>
{
    /// <inheritdoc />
    public ProductFilterNotIncludedInFamilyValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ProductFilterNotIncludedInFamilyHandler : IRequestHandler<ProductFilterNotIncludedInFamilyQuery,
    ProductFilterNotIncludedInFamilyResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ProductFilterNotIncludedInFamilyHandler(ILogger<ProductFilterNotIncludedInFamilyHandler> logger, IProductService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ProductFilterNotIncludedInFamilyResponse> Handle(
        ProductFilterNotIncludedInFamilyQuery request, CancellationToken cancellationToken = default)
    {
        var prod = _mapper.Map<Product>(request.Filter);
        prod.FamilyId = request.FamilyId;
        
        var entities = await _service.FilterNotByFamily(
            request.CurrentPage,
            request.PageSize,
            prod,
            request.Filter.Search,
            request.Filter.Sort);
        return new ProductFilterNotIncludedInFamilyResponse { Data = _mapper.Map<Pager<ProductModel>>(entities) };
    }
}

/// <inheritdoc />
public record ProductFilterNotIncludedInFamilyResponse : CQRSResponse<Pager<ProductModel>>;