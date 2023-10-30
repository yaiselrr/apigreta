using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

public class MixAndMatchCreateRequest
{
    [FromBody] 
    public MixAndMatchModel EntityDto { get; set; }
}