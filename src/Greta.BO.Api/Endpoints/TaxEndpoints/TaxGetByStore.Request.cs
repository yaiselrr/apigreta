using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

public class TaxGetByStoreRequest
{
    [FromRoute(Name = "storeId")]public int Id { get; set; }
}