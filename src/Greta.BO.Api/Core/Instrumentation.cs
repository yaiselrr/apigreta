using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Configuration;

namespace Greta.BO.Api.Core;

[ExcludeFromCodeCoverage]
public class Instrumentation: IDisposable
{
    internal const string ActivitySourceName = "Greta.BO.Api";
    private readonly string MeterName = "Greta.BO.Api";
    private readonly Meter _meter;
    
    public Instrumentation(IConfiguration configuration)
    {
        var version = typeof(Instrumentation).Assembly.GetName().Version?.ToString() ?? "unknown";
        MeterName = $"greta-bo-api-{configuration["Company:CompanyCode"]}";
        this.ActivitySource = new ActivitySource(ActivitySourceName, version);
        this._meter = new Meter(MeterName, version);
        // this.FreezingDaysCounter = this.meter.CreateCounter<long>("weather.days.freezing", "The number of days where the temperature is below freezing");
    }
    public ActivitySource ActivitySource { get; }
    
    public void Dispose()
    {
        this.ActivitySource.Dispose();
        this._meter.Dispose();
    }
}