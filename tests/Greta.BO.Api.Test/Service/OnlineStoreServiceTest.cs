using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class OnlineStoreServiceTest
    {
        readonly OnlineStoreRepository _repository;
        readonly OnlineStoreService _service;

        public OnlineStoreServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(OnlineStoreServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new OnlineStoreRepository(TestsSingleton.Auth, context);
            _service = new OnlineStoreService(_repository, Mock.Of<ILogger<OnlineStoreService>>());
        }

        [Fact]
        public async Task GetId_NormalCall_ResultSameAsInsert()
        {
            // Arrange
            var OnlineStore = new OnlineStore()
            {
                Name = "OnlineStore Name ",
                NameWebsite = "www.onlinestorename.com",
                Type = Entities.Enum.StoreType.WIX,
                IsActiveWebSite = true,
                IsAllowStorePickup = true,
                IsStockUpdated = true,
                RefreshToken = "OAUTH2.eyJraWQiOiJkZ0x3cjNRMCIsImFsZyI6IkhTMjU2In0.eyJkYXRhIjoie1wiaWRcIjpcIjc3NGE2ZGJjLTdlN2ItNDkxZi05Yzc4LTdlYWQ3MzFhYmVlZlwifSIsImlhdCI6MTY5NDQ3MDA2NywiZXhwIjoxNzU3NTQyMDY3fQ.iK9GLFz-IH4oyuaUY_3Ikcgl07jzqj-MtjkQxLO18S8",
                StoreId = 3,
                State = true
            };
            var id = await _repository.CreateAsync(OnlineStore);
            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetId_CallNotExist_ReturnNull()
        {
            // Arrange
            long id = 1000;
            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetId_CallNegative_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Get(id);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task Delete_NormalCall_ResultSucess()
        {
            // Arrange
            var OnlineStore = new OnlineStore()
            {
                Name = "OnlineStore Name "
            };
            var id = await _repository.CreateAsync(OnlineStore);

            // Act
            var result = await _service.Delete(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Delete(id);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task ChangeState_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.ChangeState(id, true);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task ChangeState_NormalCall_ResultTrue()
        {
            // Arrange
            var OnlineStore = new OnlineStore()
            {
                Name = "OnlineStore Name "
            };
            var id = await _repository.CreateAsync(OnlineStore);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var OnlineStore = new OnlineStore()
            {
                Name = "OnlineStore Name "
            };

            // Act
            var result = await _service.Post(OnlineStore);

            // Assert
            Assert.Equal(OnlineStore.Name, (result as OnlineStore).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var OnlineStore = new OnlineStore()
            {
                Name = "OnlineStore Name "
            };
            var id = await _repository.CreateAsync(OnlineStore);

            var OnlineStoreUpdate = new OnlineStore()
            {
                Id = id,
                Name = "OnlineStore Name Update",
                State = true
            };

            // Act
            var result = await _service.Put(id, OnlineStoreUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var OnlineStoreUpdate = new OnlineStore()
            {
                Name = "OnlineStore Name Update",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, OnlineStoreUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }
    }
}