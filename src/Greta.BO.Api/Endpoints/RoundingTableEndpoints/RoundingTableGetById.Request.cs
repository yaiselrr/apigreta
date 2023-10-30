using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;

public class RoundingTableGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}