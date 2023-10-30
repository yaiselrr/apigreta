using System;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.EventContracts.Report;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Report
{
    public class CreateReportConsumer : BaseConsumer<CreateReportRequestContract>
    {
        private readonly IReportRepository _reportRepository;

        public CreateReportConsumer(
            ILogger<CreateReportConsumer> logger,
            IReportRepository reportRepository,
            IOptions<MainOption> options)
            : base(logger, options)
        {
            _reportRepository = reportRepository;
        }

        public override async Task Execute(ConsumeContext<CreateReportRequestContract> context)
        {
            if (!await _reportRepository.ExistReport(context.Message.Name))
            {
                var report = new Entities.Report
                {
                    Name = context.Message.Name,
                    Category = (ReportCategory)context.Message.Category,
                    State = context.Message.State,
                    UserCreatorId = userId,
                    GuidId = context.Message.GuidId
                };

                var reportfinal = await this._reportRepository.CreateAsync<Entities.Report>(report);

                _logger.LogInformation("Report creation successfully");

                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Create report successfully",
                    Timestamp = DateTime.Now,
                });
            }
            else
                await context.RespondAsync<BooleanResponseContract>(new
                {
                    Status = true,
                    Message = "Report already exist",
                    Timestamp = DateTime.Now,
                });
        }
    }
}