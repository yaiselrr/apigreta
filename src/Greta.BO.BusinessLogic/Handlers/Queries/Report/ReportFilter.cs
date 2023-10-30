using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ReportSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Report;

/// <summary>
/// Query for filter and paginate Report
/// </summary>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record ReportFilterQuery(int CurrentPage, int PageSize, ReportSearchModel Filter) : IRequest<ReportFilterResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("view_report")
    };
}

///<inheritdoc/>
public class Validator : AbstractValidator<ReportFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public Validator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

///<inheritdoc/>
public class ReportFilterHandler : IRequestHandler<ReportFilterQuery, ReportFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IReportService _service;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ReportFilterHandler(ILogger<ReportFilterHandler> logger, IReportService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<ReportFilterResponse> Handle(ReportFilterQuery request, CancellationToken cancellationToken=default)
    {
        try
        {
            if (request.CurrentPage < 1 || request.PageSize < 1)
            {
                this._logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
                throw new BusinessLogicException("Page parameter out of bounds");
            }

            //set corporate category if ShowCorporateReports is true
            
            var spec = new ReportFilterSpec(request.Filter);
            var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
            
            return new ReportFilterResponse() { Data = this._mapper.Map<Pager<ReportModel>>(entities) };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException($"Error filter reports. {e.Message}{e.InnerException?.Message}");
        }
    }
}

///<inheritdoc/>
public record ReportFilterResponse : CQRSResponse<Pager<ReportModel>>;