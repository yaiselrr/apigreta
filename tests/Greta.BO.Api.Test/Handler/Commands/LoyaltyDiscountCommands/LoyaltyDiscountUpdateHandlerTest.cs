using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.LoyaltyDiscountCommands;


public class LoyaltyDiscountUpdateHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountUpdateHandler _handler;
    private readonly IMapper _mapper;

    public LoyaltyDiscountUpdateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountUpdateHandlerTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountUpdateHandler(
            Mock.Of<ILogger<LoyaltyDiscountUpdateHandler>>(),
            service,
            TestsSingleton.Mapper);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TestMappingProfile());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task Put_NormalCall_ResultTrue()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Put_NormalCall_ResultTrue "
        };             
       
        var id = await _repository.CreateAsync(loyaltyDiscount);

        loyaltyDiscount.Name = "LoyaltyDiscount Updated";

        var command = new LoyaltyDiscountUpdateCommand(id ,_mapper.Map<LoyaltyDiscountUpdateModel>(loyaltyDiscount));

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Put_WithIdNotExist_ResultTrue()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Put_WithIdNotExist_ResultTrue"
        };

        await _repository.CreateAsync(loyaltyDiscount);

        loyaltyDiscount.Name = "LoyaltyDiscount Updated";

        var command = new LoyaltyDiscountUpdateCommand(5, _mapper.Map<LoyaltyDiscountUpdateModel>(loyaltyDiscount));

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task Put_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Put_WithIdLessThanCiro_ResultBusinessLogicException"
        };

        await _repository.CreateAsync(loyaltyDiscount);

        loyaltyDiscount.Name = "LoyaltyDiscount Updated";

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new LoyaltyDiscountUpdateCommand(-1, _mapper.Map<LoyaltyDiscountUpdateModel>(loyaltyDiscount));

            await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}