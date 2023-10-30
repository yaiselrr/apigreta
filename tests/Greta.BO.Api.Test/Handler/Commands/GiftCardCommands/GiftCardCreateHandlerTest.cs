using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.GiftCardCommands;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.EventContracts.BO.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.GiftCardCommands;

public class GiftCardCreateHandlerTest
{
    private readonly GiftCardRepository _repository;
    private readonly GiftCardCreateHandler _handler;
    private readonly IBOUserService _userService;

    public GiftCardCreateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(GiftCardCreateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new GiftCardRepository(TestsSingleton.Auth, context);
        var service = new GiftCardService(_repository, Mock.Of<ILogger<GiftCardService>>());

        // var requestClientMoq = Mock.Of<IRequestClient<FilterUserRequestContract>>();
        // Mock.Get(requestClientMoq).Setup(m => m
        //         .GetByDepartmentId(It.IsInRange(1, 100000, Range.Inclusive), It.IsAny<long>()))
        //     .ReturnsAsync((int depId, long Id) => new Department() { Id = 1, Name = "MOckDepartment" })
        //     ;
        // _userService = new BOUserService(
        //     new BOUserRepository(TestsSingleton.Auth, context),
        //     Mock.Of<ILogger<BOUserService>>(),
        //     requestClientMoq);
        
        _userService = Mock.Of<IBOUserService>();
        
        
        _handler = new GiftCardCreateHandler(
            Mock.Of<ILogger<GiftCardCreateHandler>>(),
            service,
            TestsSingleton.Auth,
            _userService,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task Handler_UserNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        
        // var id = await _repository.CreateAsync(family);
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var data = new GiftCardModel
            {
                Number = null,
                Amount = 0,
                Balance = 0,
                StoreId = 0,
                GiftCardType = GiftCardType.DIGITS12
            };
            var query = new GiftCardCreateCommand(data);

            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("User not found.", exception.Message);
    }
    
    [Fact]
    public async Task Handler_NumberNull_ThrowDbUpdateException()
    {
        // Arrange
        Mock.Get(_userService).Setup(m => m
                .GetByUserId(It.IsAny<string>()))
            .ReturnsAsync((string userId) => new BOUser() { UserId = TestsSingleton.Auth.UserId});
        // var id = await _repository.CreateAsync(family);
        // Act
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var data = new GiftCardModel
            {
                Number = null,
                Amount = 0,
                Balance = 0,
                StoreId = 0,
                GiftCardType = GiftCardType.DIGITS12
            };
            var query = new GiftCardCreateCommand(data);

            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.NotNull( exception);
    }
    
    [Fact]
    public async Task Handler_NumberOk_ReturnOK()
    {
        // Arrange
        Mock.Get(_userService).Setup(m => m
                .GetByUserId(It.IsAny<string>()))
            .ReturnsAsync((string userId) => new BOUser() { UserId = TestsSingleton.Auth.UserId});
        // var id = await _repository.CreateAsync(family);
        // Act
        
        var data = new GiftCardModel
        {
            Number = "124234256756",
            Amount = 10,
            Balance = 10,
            StoreId = 3,
            GiftCardType = GiftCardType.DIGITS12
        };
        var query = new GiftCardCreateCommand(data);

        var result = await _handler.Handle(query);
       
        // Assert
        Assert.Null( result.Errors);
    }
    
}