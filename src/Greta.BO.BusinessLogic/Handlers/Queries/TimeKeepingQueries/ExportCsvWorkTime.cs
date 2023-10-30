using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.ReportDto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;

/// <summary>
/// Command for export csv
/// </summary>
/// <param name="WorkTimeReportModels"></param>
public record ExportCsvWorkTimeQuery(List<WorkTimeReportModel> WorkTimeReportModels): IRequest<ExportCsvWorkTimeResponse>;
/// <summary>
/// ClockOutResponse
/// </summary>
public record ExportCsvWorkTimeResponse : CQRSResponse<string>;

/// <summary>
/// ClockOutHandler
/// </summary>
public class ExportCsvWorkTimeHandler : IRequestHandler<ExportCsvWorkTimeQuery, ExportCsvWorkTimeResponse>
{
    private readonly ITimeKeepingService _timeKeepingService;
    
    /// <summary>
    /// ClockOutHandler
    /// </summary>
    /// <param name="timeKeepingService"></param>
    public ExportCsvWorkTimeHandler(ITimeKeepingService timeKeepingService)
    {
        _timeKeepingService = timeKeepingService;       
    }

    /// <inheritdoc />
    public async Task<ExportCsvWorkTimeResponse> Handle(ExportCsvWorkTimeQuery request, CancellationToken cancellationToken = default)
    {
        var csvReport = await _timeKeepingService.ExportCsv(request.WorkTimeReportModels);
        return new ExportCsvWorkTimeResponse() { Data = csvReport };
    }
}