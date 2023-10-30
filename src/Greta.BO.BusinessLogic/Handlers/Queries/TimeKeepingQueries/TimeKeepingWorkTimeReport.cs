using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.ReportDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.TimeKeepingSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;

/// <summary>
/// Query for get the work time by employee
/// </summary>
/// <param name="storeId"></param>
/// <param name="Filter"></param>
public record TimeKeepingWorkTimeReportQuery(long storeId, WorkTimeSearchModel Filter): IRequest<TimeKeepingWorkTimeReportResponse>;

///<inheritdoc/>
public record TimeKeepingWorkTimeReportResponse : CQRSResponse<List<WorkTimeReportModel>>;


/// <inheritdoc />
public class TimeKeepingWorkTimeReportValidator : AbstractValidator<TimeKeepingWorkTimeReportQuery>
{
    /// <inheritdoc />
    public TimeKeepingWorkTimeReportValidator()
    {
        RuleFor(x => x.storeId).GreaterThan(0);        
    }
}

///<inheritdoc/>
public class TimeKeepingWorkTimeReportHandler : IRequestHandler<TimeKeepingWorkTimeReportQuery, TimeKeepingWorkTimeReportResponse>
{
    private readonly ITimeKeepingService _timeKeepingService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeKeepingService"></param>
    public TimeKeepingWorkTimeReportHandler(ITimeKeepingService timeKeepingService)
    {
        _timeKeepingService = timeKeepingService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TimeKeepingWorkTimeReportResponse> Handle(TimeKeepingWorkTimeReportQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _timeKeepingService.WorkTimeReport(request.storeId, request.Filter);
        return new TimeKeepingWorkTimeReportResponse { Data = entities };
    }
}