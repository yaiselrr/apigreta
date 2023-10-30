using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class ReportServiceTest
{
    readonly ReportRepository _repository;
    readonly ReportService _service;

    public ReportServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        _service = new ReportService(_repository, Mock.Of<ILogger<ReportService>>(), Mock.Of<IRequestClient<FilterReportRequestContract>>());
    }

    [Fact]
    public async Task GetByGuid_WithSameGuid_ResultOk()
    {
        // Arrange
        var name = "GetByGuid_WithSameGuid_ResultOk";

        var guid = Guid.NewGuid();

        var report = new Entities.Report()
        {
            Name = name,
            GuidId = guid,
        };
        await _repository.CreateAsync(report);
        // Act
        
        var result = await _service.GetByGuid(guid);        

        // Assert
        Assert.Equal(guid, result.GuidId);
    }

    [Fact]
    public async Task GetByGuid_WithNotFoundGuid_ResultNull()
    {
        // Arrange
        var name = "GetByGuid_WithNotFoundGuid_ResultNull";

        var guid = Guid.NewGuid();

        var report = new Entities.Report()
        {
            Name = name,
            GuidId = guid,
        };
        await _repository.CreateAsync(report);
        // Act

        var guidNotFound = Guid.NewGuid();

        var result = await _service.GetByGuid(guidNotFound);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetReportsSPAByFilter_WithListValidGuid_ResulOk()
    {
        // Arrange
        
        List<Guid> guids = new List<Guid>();

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            guids.Add(guid);

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid,
            };

            await _repository.CreateAsync(report);
        }       
        
        // Act

        var result = await _service.GetReportsSpaByFilterAsync(guids);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetReportsSPAByFilter_WithListNotFoundGuid_ResultEmpty()
    {
        // Arrange

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid,
            };

            await _repository.CreateAsync(report);
        }

        List<Guid> guidsNotFound = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

        // Act

        var result = await _service.GetReportsSpaByFilterAsync(guidsNotFound);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetReportsByCategory_WithValidCategory_ResulOk()
    {
        // Arrange

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid,
                Category = Entities.Enum.ReportCategory.SALES
            };

            await _repository.CreateAsync(report);
        }

        // Act

        var result = await _service.GetReportsByCategory(Entities.Enum.ReportCategory.SALES);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetReportsByCategory_WithNotFoundCategory_ResulEmpty()
    {
        // Arrange

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid,
                Category = Entities.Enum.ReportCategory.TEMPLATES
            };

            await _repository.CreateAsync(report);
        }

        // Act

        var result = await _service.GetReportsByCategory(Entities.Enum.ReportCategory.CLOSEOUT);

        // Assert
        Assert.Empty(result);        
    }

    [Fact]
    public async Task GetIdByGuid_WithSameGuid_ResulOk()
    {
        // Arrange

        List<Guid> guids = new List<Guid>();

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            guids.Add(guid);

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid               
            };

            await _repository.CreateAsync(report);
        }

        // Act

        var result = await _service.GetIdByGuid(guids.First());

        // Assert
        Assert.IsType<long>(result);
    }

    [Fact]
    public async Task GetIdByGuid_WithNotFoundGuid_ResulNull()
    {
        // Arrange

        for (int i = 0; i < 3; i++)
        {
            var guid = Guid.NewGuid();

            var report = new Entities.Report()
            {
                Name = "Report " + i,
                GuidId = guid
            };

            await _repository.CreateAsync(report);
        }

        var guidNotFound = Guid.NewGuid();

        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _service.GetIdByGuid(guidNotFound);
        });       

        // Assert
        
        Assert.Equal("Guid parameter out of bounds", exception.Message);
    }

    [Fact]
    public async Task GetByName_WithSameName_ResultOk()
    {
        // Arrange

        List<Entities.Report> reports = new List<Entities.Report>();

        for (int i = 0; i < 3; i++)
        {
            reports.Add(new Entities.Report()
            {
                Name = "Report " + i,
            });
        }

        await _repository.CreateRangeAsync(reports);

        var findName = reports.First().Name;

        // Act
       
        var result = await _service.GetByName(findName);        

        // Assert

        Assert.Equal(result.Name, findName);
    }


    [Fact]
    public async Task GetByName_WithNotFoundName_ResultNull()
    {
        // Arrange

        List<Entities.Report> reports = new List<Entities.Report>();

        for (int i = 0; i < 3; i++)
        {
            reports.Add(new Entities.Report()
            {
                Name = "Report " + i,
            });
        }

        await _repository.CreateRangeAsync(reports);

        // Act

        var result = await _service.GetByName("name");

        // Assert

        Assert.Null(result);
    }
}