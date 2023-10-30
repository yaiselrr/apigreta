using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class TimeKeepingServiceTest
{
    private readonly TimeKeepingService _service;

    public TimeKeepingServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FamilyServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        var repository = new TimeKeepingRepository(TestsSingleton.Auth, context);
        _service = new TimeKeepingService(repository, Mock.Of<ILogger<TimeKeepingService>>());
    }
    
    [Fact]
    public async Task ClockIn_NormalCall_ResultNull()
    {
        var employeeId = 1;
        var employeeName = "Test";
        var deviceId = 1;
        var date = DateTime.Now;
        var formatDate = date.ToString("MM/dd/yyyy HH:mm:ss");
        
        var result = await _service.ClockIn(employeeId, employeeName, deviceId, 1, "Store",date, formatDate);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task ClockIn_TheUserHasAlreadyClockedInThisDate_ResultError()
    {
        var employeeId = 2;
        var employeeName = "Test";
        var deviceId = 1;
        var date = DateTime.Now;
        var formatDate = date.ToString("MM/dd/yyyy HH:mm:ss");
        
        await _service.ClockIn(employeeId, employeeName, deviceId,1, "Store", date, formatDate);
        var result = await _service.ClockIn(employeeId, employeeName, deviceId,1, "Store", date, formatDate);
        
        Assert.NotNull(result);
        Assert.Equal("The user has already clocked in this date", result);
    }
    
    [Fact]
    public async Task ClockOut_NormalCall_ResultNull()
    {
        var employeeId = 4;
        var employeeName = "Test";
        var deviceId = 1;
        var date = DateTime.Now;
        var formatDate = date.ToString("MM/dd/yyyy HH:mm:ss");
        
        await _service.ClockIn(employeeId, employeeName, deviceId, 1, "Store", date, formatDate);
        var result = await _service.ClockOut(employeeId, employeeName, deviceId, 1, "Store", date.AddHours(1).AddMinutes(30).AddSeconds(10), formatDate);
        
        Assert.Null(result);
        // Assert.Equal("01:30:10", result.Data.TimeWorkedFormat);
    }
    
    [Fact]
    public async Task ClockOut_TheUserHasNotClockedInThisDate_ResultError()
    {
        const int employeeId = 6;
        const string employeeName = "Test";
        const int deviceId = 1;               
        var date = DateTime.Now;
        var formatDate = date.ToString("MM/dd/yyyy HH:mm:ss");
        
        var result = await _service.ClockOut(employeeId, employeeName, deviceId, 1, "Store", date, formatDate);
        
        Assert.NotNull(result);
        Assert.Equal("The user has not clocked in this date", result);
    }
    
    
}