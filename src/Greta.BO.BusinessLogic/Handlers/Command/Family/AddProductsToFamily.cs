using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Add products to family
/// </summary>
/// <param name="Id"></param>
/// <param name="UpCs"></param>
public record AddProductsToFamilyCommand(long Id, List<string> UpCs) : IRequest<AddProductsToFamilyResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class AddProductsToFamilyHandler : IRequestHandler<AddProductsToFamilyCommand, AddProductsToFamilyResponse>
{
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public AddProductsToFamilyHandler(
        IFamilyService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public async Task<AddProductsToFamilyResponse> Handle(AddProductsToFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var data = await _service.AddProductsToFamily(request.Id, request.UpCs);
        return new AddProductsToFamilyResponse() { Data = data };
    }
}

/// <inheritdoc />
public record AddProductsToFamilyResponse : CQRSResponse<string>;