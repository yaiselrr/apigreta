using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

public class WorkTimeReportRequest
{
    [FromRoute(Name = "storeId")]public int StoreId { get; set; }   
    [FromBody]public WorkTimeSearchModel Filter { get; set; }
}