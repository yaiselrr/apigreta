
using Amazon.S3.Model;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;
using Greta.BO.BusinessLogic.Models.Dto.ReportDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.TimeKeepingQueries;


public class TimeKeepingWorkTimeReportHandlerTest
{
    private readonly TimeKeepingRepository _repository;
    private readonly TimeKeepingWorkTimeReportHandler _handler;
    private readonly ExportCsvWorkTimeHandler _handlerExport;

    public TimeKeepingWorkTimeReportHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(TimeKeepingWorkTimeReportHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new TimeKeepingRepository(TestsSingleton.Auth, context);
        var service = new TimeKeepingService(_repository, Mock.Of<ILogger<TimeKeepingService>>());

        _handler = new TimeKeepingWorkTimeReportHandler(service);
        _handlerExport = new ExportCsvWorkTimeHandler(service);

        Seed().Wait();
    }

    [Fact]
    private async Task Seed()
    { 
        List<TimeKeeping> timeKeepings = new List<TimeKeeping>();
         
        timeKeepings.Add(new TimeKeeping() { 
            Begin = new DateTime(2023,08,17,6,3,37), 
            End = new DateTime(2023,08,17,7,13,37), 
            EmployeeId = 1, 
            EmployeeName= "Temporal", 
            TimeWorked=1.2, 
            TimeWorkedFormat= "1 hours 10 minutes 0 seconds",
            BeginStoreId = 3 });

        timeKeepings.Add(new TimeKeeping()
        {
            Begin = new DateTime(2023, 08, 17, 6, 30, 53),
            End = new DateTime(2023, 08, 17, 6, 36, 14),
            EmployeeId = 30,
            EmployeeName = "francis",
            TimeWorked = 0.08916174627777777,
            TimeWorkedFormat = "00 hours 05 minutes 20 seconds",
            BeginStoreId = 3
        });

        timeKeepings.Add(new TimeKeeping()
        {
            Begin = new DateTime(2023, 08, 18, 1, 9, 13),
            End = new DateTime(2023, 08, 18, 1, 26, 7),
            EmployeeId = 30,
            EmployeeName = "francis",
            TimeWorked = 0.08916174627777777,
            TimeWorkedFormat = "00 hours 05 minutes 20 seconds",
            BeginStoreId = 3
        });

        timeKeepings.Add(new TimeKeeping()
        {
            Begin = new DateTime(2023, 08, 24, 6, 30, 53),
            End = new DateTime(2023, 08, 24, 6, 36, 14),
            EmployeeId = 30,
            EmployeeName = "francis",
            TimeWorked = 0.08916174627777777,
            TimeWorkedFormat = "00 hours 05 minutes 20 seconds",
            BeginStoreId = 3
        });

        timeKeepings.Add(new TimeKeeping()
        {
            Begin = new DateTime(2023, 08, 17, 1, 26, 30),
            End = new DateTime(2023, 08, 17, 1, 27, 22),
            EmployeeId = 1,
            EmployeeName = "Greta1 Admin1",
            TimeWorked = 0.0146163585,
            TimeWorkedFormat = "00 hours 00 minutes 52 seconds",
            BeginStoreId = 3
        });

        await _repository.CreateRangeAsync(timeKeepings);        
    }

    [Fact]
    public async Task GetAll_NormalCallBetweenTwoWeeks_ResultOk()
    {
        // Arrange

        var search = new WorkTimeSearchModel() {
            IsPagingEnabled = false,
            Search = "",
            Sort = "",
            From = new DateTime(2023, 8, 7, 0, 0, 0),
            To = new DateTime(2023, 8, 27, 23, 59, 59)
        };

        var query = new TimeKeepingWorkTimeReportQuery(3, search);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(4, result.Data.Count);
        Assert.Equal(2, result.Data.Count(x => x.EmployeeName.Equals("francis")));
    }

    [Fact]
    public async Task GetAll_NormalCallBetweenTwoWeeksWithFilter_ResultOk()
    {
        // Arrange

        var search = new WorkTimeSearchModel()
        {
            IsPagingEnabled = false,
            Search = "Temporal",
            Sort = "",
            From = new DateTime(2023, 8, 7, 0, 0, 0),
            To = new DateTime(2023, 8, 27, 23, 59, 59)
        };

        var query = new TimeKeepingWorkTimeReportQuery(3, search);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Single(result.Data);
        Assert.Equal("Temporal", result.Data.First().EmployeeName);
    }

    // [Fact]
    // public async Task GetAll_InvalidRangeDayStartOrEnd_ResultBusinessLogicException()
    // {
    //     // Arrange
    //
    //     var search = new WorkTimeSearchModel()
    //     {
    //         IsPagingEnabled = false,
    //         Search = "Temporal",
    //         Sort = "",
    //         From = new DateTime(2023, 8, 10, 0, 0, 0),
    //         To = new DateTime(2023, 8, 27, 23, 59, 59)
    //     };
    //
    //     var query = new TimeKeepingWorkTimeReportQuery(3, search);
    //
    //     // Act
    //
    //     var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
    //     {
    //         await _handler.Handle(query);
    //     });        
    //
    //     // Assert
    //     Assert.Equal("The range of date is invalid. Select Monday to Sunday", exception.Message);
    // }

    // [Fact]
    // public async Task GetAll_InvalidDateFromUpperTo_BusinessLogicException()
    // {
    //     // Arrange
    //
    //     var search = new WorkTimeSearchModel()
    //     {
    //         IsPagingEnabled = false,
    //         Search = "Temporal",
    //         Sort = "",
    //         From = new DateTime(2023, 8, 28, 0, 0, 0),
    //         To = new DateTime(2023, 8, 27, 23, 59, 59)
    //     };
    //
    //     var query = new TimeKeepingWorkTimeReportQuery(3, search);
    //
    //     // Act
    //
    //     var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
    //     {
    //         await _handler.Handle(query);
    //     });
    //
    //     // Assert
    //     Assert.Equal("The range of date is invalid. Select Monday to Sunday", exception.Message);
    // }

    // [Fact]
    // public async Task GetAll_NormalCallWithoutRangeDate_ResultEmpty()
    // {
    //     // Arrange
    //
    //     var search = new WorkTimeSearchModel()
    //     {
    //         IsPagingEnabled = false,
    //         Search = "",
    //         Sort = ""           
    //     };
    //
    //     var query = new TimeKeepingWorkTimeReportQuery(3, search);
    //
    //     // Act
    //     var result = await _handler.Handle(query);
    //
    //     // Assert
    //     Assert.Empty(result.Data);
    // }

    [Fact]
    public async Task Export_InvalidJson_ResultBusinessLogicException()
    {
        // Arrange

        var model = new List<WorkTimeReportModel>();
       
        var query = new ExportCsvWorkTimeQuery(model);

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _handlerExport.Handle(query);
        });

        // Assert
        Assert.Equal("Invalid or empty list of WorkTimeModelReport", exception.Message);
    }

    [Fact]
    public async Task Export_NormalCall_ResultOk()
    {
        // Arrange

        var search = new WorkTimeSearchModel()
        {
            IsPagingEnabled = false,
            Search = "Temporal",
            Sort = "",
            From = new DateTime(2023, 8, 7, 0, 0, 0),
            To = new DateTime(2023, 8, 27, 23, 59, 59)
        };

        var data = new TimeKeepingWorkTimeReportQuery(3, search);
                
        var resultData = await _handler.Handle(data);

        var query = new ExportCsvWorkTimeQuery(resultData.Data);

        // Act

        var result = await _handlerExport.Handle(query);       

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.IsType<string>(result.Data);
    }
}