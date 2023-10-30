using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Report;

/// <summary>
/// Query for get Report by Category
/// </summary>
/// <param name="Category"></param>
public record ReportsGetByCategoryQuery(ReportCategory Category) : IRequest<ReportsGetByCategoryResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new List<IRequirement>
        { new PermissionRequirement.Requirement("view_report") };
}

///<inheritdoc/>
public class ReportsGetByCategoryHandler : IRequestHandler<ReportsGetByCategoryQuery, ReportsGetByCategoryResponse>
{
    private readonly IReportService _service;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ReportsGetByCategoryHandler(IReportService service, IMapper mapper)
    {
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
    public async Task<ReportsGetByCategoryResponse> Handle(ReportsGetByCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await this._service.GetReportsByCategory(request.Category);
            var data = this._mapper.Map<List<ReportModel>>(reports);
            return data == null ? null : new ReportsGetByCategoryResponse() { Data = data };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException($"Error get reports. {e.Message}{e.InnerException?.Message}");
        }
    }
}

///<inheritdoc/>
public record ReportsGetByCategoryResponse : CQRSResponse<List<ReportModel>>;