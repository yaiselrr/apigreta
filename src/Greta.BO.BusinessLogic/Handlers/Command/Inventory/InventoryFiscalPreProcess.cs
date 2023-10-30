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
/// Command for fiscal pre process
/// </summary>
/// <param name="Entity"></param>
public record InventoryFiscalPreProcessCommand(InventoryFiscalModel Entity) : IRequest<InventoryFiscalPreProcessResponse>, IAuthorizable
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
public class InventoryFiscalPreProcessHandler : IRequestHandler<InventoryFiscalPreProcessCommand, InventoryFiscalPreProcessResponse>
{
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public InventoryFiscalPreProcessHandler(IStoreProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<InventoryFiscalPreProcessResponse> Handle(InventoryFiscalPreProcessCommand request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.PreprocessFiscalInventory(request.Entity);
        return new InventoryFiscalPreProcessResponse() { Data = (entity) };
    }
}

///<inheritdoc/>
public record InventoryFiscalPreProcessResponse : CQRSResponse<InventoryFiscalModel>;