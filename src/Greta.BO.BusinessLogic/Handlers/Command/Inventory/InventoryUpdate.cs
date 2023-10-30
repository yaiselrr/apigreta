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

namespace Greta.BO.BusinessLogic.Handlers.Command.Inventory;

/// <summary>
/// Command for update inventory
/// </summary>
/// <param name="Update"></param>
public record InventoryUpdateCommand(InventoryUpdateModel Update) : IRequest<InventoryUpdateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_inventory")
    };
}
    
///<inheritdoc/>
public class InventoryUpdateValidator : AbstractValidator<InventoryUpdateCommand>
{
    /// <summary>
    /// 
    /// </summary>
    public InventoryUpdateValidator()
    {
        RuleFor(x => x.Update.StoreProductId).GreaterThan(0);
    }
}
    
///<inheritdoc/>
public class InventoryUpdateHandler : IRequestHandler<InventoryUpdateCommand, InventoryUpdateResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public InventoryUpdateHandler(IStoreProductService service, IMapper mapper)
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
    public async Task<InventoryUpdateResponse> Handle(InventoryUpdateCommand request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.UpdateInventory(request.Update);
        return new InventoryUpdateResponse() {Data = this._mapper.Map<InventoryResponseModel>(entity) };
    }
}

///<inheritdoc/>
public record InventoryUpdateResponse : CQRSResponse<InventoryResponseModel>;
