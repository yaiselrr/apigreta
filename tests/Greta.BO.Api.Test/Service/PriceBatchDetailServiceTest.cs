using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Greta.BO.Api.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Greta.BO.Api.Test.Service
{
    public class PriceBatchDetailServiceTest
    {

        readonly PriceBatchDetailRepository _repository;
        readonly PriceBatchDetailService _service;

        public PriceBatchDetailServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(PriceBatchDetailServiceTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new PriceBatchDetailRepository(TestsSingleton.Auth, context);
            _service = new PriceBatchDetailService(_repository, Mock.Of<ILogger<PriceBatchDetailService>>(), Mock.Of<ISynchroService>());

        }


        [Fact]
        public void Get_NormalCall_ResultOk()
        {
            // Arrange

            // Act
            var result = _service.Get().Result;

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetId_NormalCall_ResultSameAsInsert()
        {
            // Arrange
            var priceBatchDetail = new PriceBatchDetail()
            {
                Price = 9,
                HeaderId = 203,
                ProductId = 7875,
                CategoryId = -1,
                FamilyId = -1
            };
            var id = await _repository.CreateAsync(priceBatchDetail);
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
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var priceBatchDetailUpdate = new PriceBatchDetail()
            {
                Price = 9,
                HeaderId = 203,
                ProductId = 7875,
                CategoryId = -1,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, priceBatchDetailUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterPriceBatchDetail_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var priceBatchDetail = new PriceBatchDetail()
            {
                Price = 9,
                HeaderId = 203,
                ProductId = 7875,
                CategoryId = -1,
                FamilyId = -1
            };
            var id = await _repository.CreateAsync(priceBatchDetail);

            var spec = new PriceBatchDetailFilterSpec(
               new PriceBatchDetailSearchModel()
            );

            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.FilterSpec(
                    0, 0, null);
            });

            // Assert
            Assert.Equal("Page parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterPriceBatchDetail_NormalCall_ResultOk()
        {
            // Arrange
            var cats = new List<PriceBatchDetail>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new PriceBatchDetail()
                {
                    Price = 9,
                    HeaderId = 203,
                    ProductId = 7875,
                    CategoryId = -1,
                    FamilyId = -1
                });
            }
            var id = await _repository.CreateRangeAsync<PriceBatchDetail>(cats);

            var spec = new PriceBatchDetailFilterSpec(
                new PriceBatchDetailSearchModel()
                {
                    HeaderId = 203,
                    ProductId = 7875
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task GetFullDetails_NormalCall()
        {
            // Arrange
            var priceBatchDetail = new PriceBatchDetail()
            {
                Price = 9,
                HeaderId = 203,
                ProductId = 7875,
                CategoryId = -1,
                FamilyId = -1
            };
            var id = await _repository.CreateAsync(priceBatchDetail);
            // Act
            var result = await _service.GetFullDetails(priceBatchDetail.HeaderId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByProductAndHEader_NormalCall()
        {
            // Arrange
            var priceBatchDetail = new PriceBatchDetail()
            {
                Price = 9,
                HeaderId = 203,
                ProductId = 7875,
                CategoryId = -1,
                FamilyId = -1
            };
            var id = await _repository.CreateAsync(priceBatchDetail);
            // Act
            var result = await _service.GetByProductAndHEader(7875, 203);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductIdByUpc_NormalCall()
        {
            // Arrange
            List<Product> products = new List<Product>() {
                new Product(){ Name = "Product1" , UPC = "UPC"},
                new Product(){ Name = "Product2" , UPC = "UPC"}
            };

            await _repository.CreateRangeAsync(products);
            // Act
            var result = await _service.GetProductIdByUpc("UPC");

            // Assert
            Assert.NotNull(result);
        }
    }
}