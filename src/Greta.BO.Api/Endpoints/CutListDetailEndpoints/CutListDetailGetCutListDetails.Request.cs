using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

public class CutListDetailGetCutListDetailsRequest
{
    [FromRoute(Name = "cutListId")] public int CutListId { get; set; }
}