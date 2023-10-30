using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

public class FeeGetByIdRequest
{
    [FromRoute(Name = "entityId")]public long Id { get; set; }
}