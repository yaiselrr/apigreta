using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.DiscountSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class DiscountServiceTest
    {

        readonly DiscountRepository _repository;
        readonly DiscountService _service;

        public DiscountServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(DiscountServiceTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new DiscountRepository(TestsSingleton.Auth, context);
            _service = new DiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<DiscountService>>());
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
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };
            var id = await _repository.CreateAsync(discount);
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
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };
            var id = await _repository.CreateAsync(discount);

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
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };
            var id = await _repository.CreateAsync(discount);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };

            // Act
            var result = await _service.Post(discount);

            // Assert
            Assert.Equal(discount.Name, (result as Discount).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };
            var id = await _repository.CreateAsync(discount);

            var discountUpdate = new Discount()
            {
                Id = id,
                Name = "Discount Name Update",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true
            };

            // Act
            var result = await _service.Put(id, discountUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var discountUpdate = new Discount()
            {
                Id = id,
                Name = "Discount Name Update",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, discountUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterDiscount_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var discount = new Discount()
            {
                Name = "Discount Name ",
                Value = 1,
                Type = Entities.Enum.DiscountType.PERCENT,
                Products = new List<Product>(),
                State = true                
            };

            var id = await _repository.CreateAsync(discount);
            
            var spec = new DiscountFilterSpec(
               new DiscountSearchModel()
            );
            
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.FilterSpec(
                    0, 0, null );
            });

            // Assert
            Assert.Equal("Page parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterDiscount_NormalCall_ResultOk()
        {
            // Arrange
            var discounts = new List<Discount>();
            for (var i = 0; i < 10; i++)
            {
                discounts.Add(new Discount()
                {
                    Name = "_fil_ Discount Name " + i
                });
            }
            var id = await _repository.CreateRangeAsync<Discount>(discounts);

            var spec = new DiscountFilterSpec(
                new DiscountSearchModel()
                {
                    Name = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }        
    }
}