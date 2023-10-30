using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.InventoryQueries;

/// <summary>
/// Query for suggested filter and paginate Inventory 
/// </summary>
/// <param name="StoreId"></param>
/// <param name="VendorId"></param>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record InventorySuggestedFilterQuery
    (long StoreId, long VendorId, int CurrentPage, int PageSize, InventorySearchModel Filter) : IRequest<InventorySuggestedFilterResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("view_inventory")
    };
}

///<inheritdoc/>
public class InventorySuggestedFilterValidator : AbstractValidator<InventorySuggestedFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public InventorySuggestedFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

///<inheritdoc/>
public class InventorySuggestedFilterHandler : IRequestHandler<InventorySuggestedFilterQuery, InventorySuggestedFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public InventorySuggestedFilterHandler(IStoreProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<InventorySuggestedFilterResponse> Handle(InventorySuggestedFilterQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.FilterPaginatedSuggested(request.Filter, request.CurrentPage, request.PageSize, request.StoreId, request.VendorId);
        return new InventorySuggestedFilterResponse() { Data = this._mapper.Map<Pager<InventoryResponseModel>>(entities) };
    }
}

///<inheritdoc/>
public record InventorySuggestedFilterResponse : CQRSResponse<Pager<InventoryResponseModel>>;
