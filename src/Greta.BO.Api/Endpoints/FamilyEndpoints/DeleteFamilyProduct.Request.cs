using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class DeleteFamilyProductRequest
{
    [FromRoute(Name = "entityId")]public long EntityId { get; set; }
    [FromRoute(Name = "productId")]public long ProductId { get; set; }
}