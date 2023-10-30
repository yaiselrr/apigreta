using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using Greta.BO.BusinessLogic.Service;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sprache;

namespace Greta.BO.Api.Test.Handler.Queries.ReportQueries;

public class ReportGetByIdHandlerTest
{
    private readonly ReportRepository _repository;
    private readonly ReportGetByIdHandler _handler;

    public ReportGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        var service = new ReportService(_repository, Mock.Of<ILogger<ReportService>>(), Mock.Of<IRequestClient<FilterReportRequestContract>>());

        _handler = new ReportGetByIdHandler(service, TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task ReportGetById_CallWithNotFoundId_ResultNull()
    {
        // Arrange
        var mediator = new  Mock<IMediator>();
        var name = "FilterSpec_OutRange_ThrowBusinessLogicException";
        var report = new Api.Entities.Report()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(report);
        // Act
       
        var query = new ReportGetByIdQuery(-13);

        var result = await _handler.Handle(query);       

        // Assert
        Assert.Null(result);
    }
            
    [Fact]
    public async Task ReportGetById_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var name = "FilterSpec_OutRange_ThrowBusinessLogicException";
        var report = new Entities.Report()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(report);

        // Act
        var query = new ReportGetByIdQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(id, result.Data.Id);
    }
}