using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Report;
using Greta.BO.BusinessLogic.Exceptions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Report
{
    public class DeleteReportConsumer : BaseConsumer<DeleteReportRequestContract>
    {
        protected readonly IReportRepository _reportRepository;

        public DeleteReportConsumer(
            ILogger<DeleteReportConsumer> logger,
            IReportRepository reportRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _reportRepository = reportRepository;
        }

        public override async Task Execute(ConsumeContext<DeleteReportRequestContract> context)
        {
            //delete report
            Entities.Report report = await _reportRepository.GetByGuid(context.Message.GuidId);

            var resultSuccess = report == null ? true : await _reportRepository.DeleteAsync(report.Id);

            await context.RespondAsync<BooleanResponseContract>(new
            {
                Status = resultSuccess,
                Message = "Report delete successfully",
                Timestamp = DateTime.Now,
            });
        }
    }
}