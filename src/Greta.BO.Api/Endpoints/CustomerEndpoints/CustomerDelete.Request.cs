using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;

public class CustomerDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}