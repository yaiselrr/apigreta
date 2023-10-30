using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

public class CutListGetScaleProductByUpcAndPluRequest
{
    [FromRoute(Name = "upc")] public string Upc { get; set; }
    [FromRoute(Name = "pluNumber")] public int Plu { get; set; }
    [FromRoute(Name = "animalId")] public long AnimalId { get; set; }
}