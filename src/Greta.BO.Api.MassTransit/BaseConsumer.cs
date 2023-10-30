using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Greta.BO.Api.EventContracts;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using Greta.Sdk.MassTransit.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseConsumer<TContract> : IConsumer<TContract>
        where TContract : class, IRegisteredEventContract
    {
        protected readonly ILogger _logger;
        private readonly MainOption _options;
        protected string userId;

        public BaseConsumer(
            ILogger logger,
            IOptions<MainOption> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task Consume(ConsumeContext<TContract> context)
        {
            try
            {
                var clientCode = context.Headers.Get("clientCode", "nopresent");
                if (_options.CompanyCode != clientCode)
                {
                    _logger.LogInformation("Masstransit call to BO ignore");
                    return;
                }

                userId = context.Headers.Get("user", "fail");
                if (userId == "fail")
                {
                    await context.RespondAsync<FailResponseContract>(new
                    {
                        errorMessages = new List<string>() {"User isn't authenticate."},
                        Timestamp = DateTime.Now,
                    });
                    return;
                }

                await Execute(context);
            }
            catch (Exception e)
            {
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string>() {e.Message},
                    Timestamp = DateTime.Now,
                });
                throw new BusinessLogicException($"Error Update Store. {e.Message}{e.InnerException?.Message}");
            }
        }

        public abstract Task Execute(ConsumeContext<TContract> context);
    }
}