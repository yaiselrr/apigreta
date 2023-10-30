using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.ReportDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

public class ExportCsvWorkTimeRequest
{    
    [FromBody]public List<WorkTimeReportModel> Data { get; set; }
}