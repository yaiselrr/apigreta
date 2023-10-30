using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;

public class CustomerGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}