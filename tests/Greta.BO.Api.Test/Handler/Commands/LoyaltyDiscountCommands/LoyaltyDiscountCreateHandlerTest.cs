
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.LoyaltyDiscountCommands;


public class LoyaltyDiscountCreateHandlerTest
{
    private readonly LoyaltyDiscountCreateHandler _handler;
    private readonly IMapper _mapper;

    public LoyaltyDiscountCreateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountCreateHandlerTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        var repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountCreateHandler(
            Mock.Of<ILogger<LoyaltyDiscountCreateHandler>>(),
            service,
            TestsSingleton.Mapper);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TestMappingProfile());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Name "
        };             
       
        var command = new LoyaltyDiscountCreateCommand(_mapper.Map<LoyaltyDiscountCreateModel>(loyaltyDiscount));

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<LoyaltyDiscountCreateResponse>(result);
        Assert.Equal(loyaltyDiscount.Name, result.Data.Name);
    }
}