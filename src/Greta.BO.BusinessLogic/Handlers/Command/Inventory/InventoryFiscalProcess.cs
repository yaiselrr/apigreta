using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Command.Inventory;

/// <summary>
/// Command for create fiscal process
/// </summary>
/// <param name="Entity"></param>
public record InventoryFiscalProcessCommand(InventoryFiscalModel Entity) :IRequest<InventoryFiscalProcessResponse>, IAuthorizable
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
public class InventoryFiscalProcessHandler : IRequestHandler<InventoryFiscalProcessCommand, InventoryFiscalProcessResponse>
{
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public InventoryFiscalProcessHandler(IStoreProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<InventoryFiscalProcessResponse> Handle(InventoryFiscalProcessCommand request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.ProcessFiscalInventory(request.Entity);
        return new InventoryFiscalProcessResponse() { Data = entity };
    }
}

///<inheritdoc/>
public record InventoryFiscalProcessResponse : CQRSResponse<bool>;

