using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.BO.BusinessLogic.Specifications.ReportSpecs;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

///<inheritdoc/>
public interface IReportService : IGenericBaseService<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="filter"></param>
    /// <param name="searchstring"></param>
    /// <param name="sortstring"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<Pager<ReportGetInfoResponseContract>> FilterReportAsync(string userId, Api.Entities.Report filter, string searchstring, string sortstring, int currentPage, int pageSize);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Api.Entities.Report> GetById(long id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Api.Entities.Report> GetByName(string name, long id = -1);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="longIds"></param>
    /// <returns></returns>
    Task<List<Api.Entities.Report>> GetReportsSpaByFilterAsync(List<Guid> longIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task<List<Api.Entities.Report>> GetReportsByCategory(ReportCategory category);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    Task<long> GetIdByGuid(Guid guid);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidId"></param>
    /// <returns></returns>
    Task<Api.Entities.Report> GetByGuid(Guid guidId);
}

/// <summary>
/// Report Service
/// </summary>
public class ReportService : BaseService<IReportRepository, Api.Entities.Report>, IReportService
{
    readonly IRequestClient<FilterReportRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reportRepository"></param>
    /// <param name="logger"></param>
    /// <param name="client"></param>
    public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger, IRequestClient<FilterReportRequestContract> client)
    : base(reportRepository, logger)
    {
        _client = client;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Api.Entities.Report> GetByName(string name, long id = -1)
    {
        return await _repository.GetEntity<Api.Entities.Report>().WithSpecification(new ReportGetByNameSpec(name, id)).FirstOrDefaultAsync();

    }

    /// <summary>
    /// Get report id by guid
    /// </summary>
    /// <param name="guid">Report guid</param>
    /// <returns>Report id</returns>
    public async Task<long> GetIdByGuid(Guid guid)
    {
        var result = await _repository.GetEntity<Api.Entities.Report>().WithSpecification(new ReportGetIdByGuidSpec(guid)).FirstOrDefaultAsync();
        if (result == null)
            throw new BusinessLogicException("Guid parameter out of bounds");       
        return result.Id;
    }

    /// <summary>
    /// Get Reports list by clienId
    /// </summary>
    public async Task<List<Api.Entities.Report>> GetReportsByCategory(ReportCategory category)
    {            
        return await _repository.GetEntity<Api.Entities.Report>().WithSpecification(new ReportGetByCategorySpec(category)).ToListAsync();
    }       
  
    /// <summary>
    /// Get Report by Id
    /// </summary>
    public async Task<Api.Entities.Report> GetById(long id)
        => await _repository.GetEntity<Api.Entities.Report>()
                                 .WithSpecification(new GetByIdSpec<Api.Entities.Report>(id))  
                                 .FirstOrDefaultAsync();

    /// <summary>
    /// Filter report
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="filter"></param>
    /// <param name="searchstring"></param>
    /// <param name="sortstring"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<Pager<ReportGetInfoResponseContract>> FilterReportAsync(
        string userId,
        Api.Entities.Report filter,
        string searchstring,
         string sortstring,
         int currentPage,
         int pageSize)
    {

        //get filtered reports list from identity
        Pager<ReportGetInfoResponseContract> reportsIdentity =
            (await _client.GetResponse<FilterReportResponseContract>(
                    new
                    {
                        __Header_user = userId,
                        SearchString = searchstring,
                        SortString = sortstring,
                        CurrentPage = currentPage,
                        PageSize = pageSize
                    })).Message.Reports;
        return reportsIdentity;
    }

    /// <summary>
    /// Get reports by list of guid
    /// </summary>
    /// <param name="guidIds"></param>
    /// <returns></returns>
    public async Task<List<Api.Entities.Report>> GetReportsSpaByFilterAsync(List<Guid> guidIds)
    {
        try
        {
            var reportsSpa = await _repository.GetEntity<Api.Entities.Report>()
                                              .WithSpecification(new ReportGetSPAByFilterSpec(guidIds))                                              
                                              .ToListAsync();
            return reportsSpa;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Get report by guid
    /// </summary>
    /// <param name="guidId"></param>
    /// <returns></returns>
    public async Task<Api.Entities.Report> GetByGuid(Guid guidId)
    {
        var r = await _repository.GetEntity<Api.Entities.Report>().WithSpecification(new ReportGetByGuidSpec(guidId)).FirstOrDefaultAsync();
        return r;
    }
}
