using Greta.BO.Api.Entities.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleGetByStoreRequest
{
    [FromRoute(Name = "storeId")]public long storeId { get; set; }
}