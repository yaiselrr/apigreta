using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.Api.Core
{
    [ExcludeFromCodeCoverage]
    public class MetricReporter
    {
        private readonly ILogger<MetricReporter> _logger;
        private readonly Counter _requestCounter;
        private readonly Histogram _responseTimeHistogram;
        private readonly Histogram _handlerTimeHistogram;

        public MetricReporter(ILogger<MetricReporter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _requestCounter =
                Metrics.CreateCounter("total_requests", "The total number of requests serviced by this API.");

            _responseTimeHistogram = Metrics.CreateHistogram("request_duration_seconds",
                "The duration in seconds between the response to a request.", new HistogramConfiguration
                {
                    Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                    LabelNames = new[] { "status_code", "method" }
                });

            _handlerTimeHistogram = Metrics.CreateHistogram("handler_duration_seconds",
                "The duration in seconds between the response to a request of handlers.", new HistogramConfiguration
                {
                    Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                    LabelNames = new[] { "name" }
                });
        }

        public void RegisterRequest()
        {
            _requestCounter.Inc();
        }

        public void RegisterResponseTime(int statusCode, string method, TimeSpan elapsed)
        {
            _responseTimeHistogram.Labels(statusCode.ToString(), method).Observe(elapsed.TotalSeconds);
        }
        public void RegisterHandlerTime(string name, TimeSpan elapsed)
        {
            _handlerTimeHistogram.Labels(name).Observe(elapsed.TotalSeconds);
        }
    }
}