using Greta.BO.Api.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Microsoft.Extensions.Options;
using Greta.Sdk.MassTransit.Contracts;

namespace Greta.BO.Api.MassTransit.Report
{
    public class DeleteReportsRangeConsumer : BaseConsumer<Greta.BO.Api.EventContracts.Report.DeleteReportRangeRequestContract>
    {
        private readonly IReportRepository _reportRepository;

        public DeleteReportsRangeConsumer(
            ILogger<DeleteReportsRangeConsumer> logger,
            IReportRepository reportRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _reportRepository = reportRepository;
        }

        public override async Task Execute(ConsumeContext<Greta.BO.Api.EventContracts.Report.DeleteReportRangeRequestContract> context)
        {
            //delete reports
            List<Entities.Report> reports = new List<Entities.Report>();
            context.Message.GuidIds.ForEach(guid =>
                reports.Add(_reportRepository.GetEntity<Entities.Report>().FirstOrDefaultAsync(e => e.GuidId == guid)
                    .Result));

            var resultSuccess = await _reportRepository.DeleteRangeAsync(reports);

            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = resultSuccess,
                Message = "Report delete successfully",
                Timestamp = DateTime.Now,
            });
        }
    }
}