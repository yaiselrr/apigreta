using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Report;

/// <summary>
/// Query for get all Report
/// </summary>
public record ReportGetAllQuery : IRequest<ReportGetAllResponse>, IAuthorizable
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
public class ReportGetAllHandler : IRequestHandler<ReportGetAllQuery, ReportGetAllResponse>
{
    private readonly IReportService _service;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ReportGetAllHandler(IReportService service, IMapper mapper)
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
    public async Task<ReportGetAllResponse> Handle(ReportGetAllQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await _service.Get();
            return new ReportGetAllResponse() { Data = _mapper.Map<List<ReportModel>>(entities) };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException($"Error get all report. {e.Message}{e.InnerException?.Message}");
        }
    }
}

///<inheritdoc/>
public record ReportGetAllResponse : CQRSResponse<List<ReportModel>>;