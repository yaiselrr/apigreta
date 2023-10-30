using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using SerilogTimings;

namespace Greta.BO.Api.PipelineBehavior
{
    [ExcludeFromCodeCoverage]
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> logger;
        private readonly MetricReporter reporter;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger,
            MetricReporter reporter)
        {
            this.logger = logger;
            this.reporter = reporter;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType();
            // logger.LogInformation("{Request} is starting.", requestName);
            
            using (Operation.Time("{Request}.", requestName))   
            {
                var response = await next();
                return response;
            }
            
            // var timer = Stopwatch.StartNew();
            // var response = await next();
            // timer.Stop();
            // logger.LogInformation("{Request} has finished in {Time}ms.", requestName, timer.ElapsedMilliseconds);
            // reporter.RegisterHandlerTime(
            //     requestName.ToString(),
            //     timer.Elapsed); 
            //return response;
        }
    }
}