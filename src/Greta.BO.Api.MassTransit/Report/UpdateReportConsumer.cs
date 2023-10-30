using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Report;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.MassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Report
{
    public class UpdateReportConsumer : BaseConsumer<UpdateReportRequestContract>
    {
        private readonly IReportService _reportService;

        public UpdateReportConsumer(
            ILogger<UpdateReportConsumer> logger,
            IReportService reportService,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _reportService = reportService;
        }

        public override async Task Execute(ConsumeContext<UpdateReportRequestContract> context)
        {
            Entities.Report report = await _reportService.GetByGuid(context.Message.GuidId);

            if ((ReportCategory) context.Message.Category != ReportCategory.CORPORATES)
            {
                //update report data                       

                report.Name = context.Message.Name;
                report.Category = (ReportCategory) context.Message.Category;
                report.GuidId = context.Message.GuidId;
                report.State = context.Message.State;

                bool resultSuccess = await _reportService.Put(report.Id, report);

                _logger.LogInformation("Update report successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = resultSuccess,
                    Message = "Update report successfully",
                    Timestamp = DateTime.Now,
                });
            }
            else
            {
                //delete report when category change to Corporate
                var resultSuccess = await _reportService.Delete(report.Id);

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = resultSuccess,
                    Message = "Report delete becouse change category",
                    Timestamp = DateTime.Now,
                });
            }
        }
    }
}