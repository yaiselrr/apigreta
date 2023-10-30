using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Id"></param>
/// <param name="ProductIds"></param>
public record FamilyDeleteRangeProductsCommand
    (long Id, List<long> ProductIds) : IRequest<FamilyDeleteRangeProductsResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class
    FamilyDeleteRangeProductsHandler : IRequestHandler<FamilyDeleteRangeProductsCommand,
        FamilyDeleteRangeProductsResponse>
{
    private readonly ILogger _logger;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FamilyDeleteRangeProductsHandler(
        ILogger<FamilyDeleteRangeProductsHandler> logger,
        IFamilyService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<FamilyDeleteRangeProductsResponse> Handle(FamilyDeleteRangeProductsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRangeProduct(request.Id, request.ProductIds.ToList());
        _logger.LogInformation("Entities with ids = {RequestProductIds} Deleted successfully", request.ProductIds);
        return new FamilyDeleteRangeProductsResponse { Data = result };
    }
}

/// <inheritdoc />
public record FamilyDeleteRangeProductsResponse : CQRSResponse<bool>;