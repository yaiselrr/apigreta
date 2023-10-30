using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

public class MixAndMatchUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public MixAndMatchModel EntityDto { get; set; }
}