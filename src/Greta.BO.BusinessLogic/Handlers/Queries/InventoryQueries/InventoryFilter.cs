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
/// Query for filter and paginate Inventory
/// </summary>
/// <param name="StoreId"></param>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record InventoryFilterQuery
    (long StoreId, int CurrentPage, int PageSize, InventorySearchModel Filter) : IRequest<InventoryFilterResponse>, IAuthorizable
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
public class InventoryFilterValidator : AbstractValidator<InventoryFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public InventoryFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

///<inheritdoc/>
public class InventoryFilterHandler : IRequestHandler<InventoryFilterQuery, InventoryFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public InventoryFilterHandler(IStoreProductService service, IMapper mapper)
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
    public async Task<InventoryFilterResponse> Handle(InventoryFilterQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.FilterInventory(request.Filter, request.CurrentPage, request.PageSize, request.StoreId);
        return new InventoryFilterResponse() { Data = this._mapper.Map<Pager<InventoryResponseModel>>(entities) };
    }
}

///<inheritdoc/>
public record InventoryFilterResponse : CQRSResponse<Pager<InventoryResponseModel>>;
