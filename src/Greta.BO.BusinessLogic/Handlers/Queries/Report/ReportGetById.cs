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
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Report;

/// <summary>
/// Query for get Report by id
/// </summary>
/// <param name="Id"></param>
public record ReportGetByIdQuery(long Id) : IRequest<ReportGetByIdResponse>, IAuthorizable
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
public class ReportGetByIdHandler : IRequestHandler<ReportGetByIdQuery, ReportGetByIdResponse>
{
    private readonly IReportService _service;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ReportGetByIdHandler(IReportService service, IMapper mapper)
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
    public async Task<ReportGetByIdResponse> Handle(ReportGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            //get report info from SPA
            var reportCorp = await _service.Get(new GetByIdSpec<Api.Entities.Report>(request.Id), cancellationToken);
            return reportCorp == null
                ? null
                : new ReportGetByIdResponse() { Data = _mapper.Map<ReportModel>(reportCorp) };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException($"Error get report. {e.Message}{e.InnerException?.Message}");
        }
    }
}

///<inheritdoc/>
public record ReportGetByIdResponse : CQRSResponse<ReportModel>;