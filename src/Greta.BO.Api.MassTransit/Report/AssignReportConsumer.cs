using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Report;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Report
{
    public class AssignReportConsumer : BaseConsumer<AssignReportRequestContract>
    {
        protected readonly IReportService _reportService;
        private readonly IAuthenticateUser<string> _auth;

        public AssignReportConsumer(
            ILogger<AssignReportConsumer> logger,
            IReportService reportService,
            IAuthenticateUser<string> auth,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _reportService = reportService;
            _auth = auth;
        }

        public override async Task Execute(ConsumeContext<AssignReportRequestContract> context)
        {
            _auth.IsAuthenticated = true;
            _auth.UserId = userId;

            var successIdReport = new List<long>();

            foreach (var rep in context.Message.Reports)
            {
                Entities.Report report = await _reportService.GetByGuid(rep.GuidId);
                if ((ReportCategory) rep.Category != ReportCategory.CORPORATES)
                {
                    if (report == null)
                    {
                        report = new Entities.Report
                        {
                            Name = rep.Name,
                            Category = (ReportCategory) rep.Category,
                            State = rep.State,
                            GuidId = rep.GuidId,
                            UserCreatorId = userId
                        };

                        var resultPost = await _reportService.Post(report);
                        if (resultPost.Id != 0)
                            successIdReport.Add(rep.Id);
                    }
                    else //update report data  
                    {
                        report.Name = rep.Name;
                        report.Category = (ReportCategory) rep.Category;
                        report.GuidId = rep.GuidId;
                        report.State = rep.State;

                        if (await _reportService.Put(report.Id, report))
                            successIdReport.Add(rep.Id);
                    }
                }
                else
                {
                    //delete report when category change to Corporate
                    var resultSuccess = await _reportService.Delete(report.Id);

                    await context.RespondAsync<BooleanResponseContract>(new
                    {
                        Status = resultSuccess,
                        Message = "Report delete because change category",
                        Timestamp = DateTime.Now,
                    });
                }
            }

            if (context.Message.Reports.Count == successIdReport.Count)
            {
                _logger.LogInformation("Create/Update report successfully");
            }
            else
            {
                _logger.LogInformation("Some report creations/updates could not be done");
            }

            await context.RespondAsync<BOApiAssignReportResponseContract>(new
            {
                Reports = successIdReport
            });
        }
    }
}