using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Tax;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class TaxServiceTest
    {

        readonly TaxRepository _repository;
        readonly TaxService _service;

        public TaxServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(TaxServiceTest))
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);
            _repository = new TaxRepository(TestsSingleton.Auth, context);
            _service = new TaxService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<TaxService>>());
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
            var tax = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170
            };
            var id = await _repository.CreateAsync(tax);
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
            var tax = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170
            };
            var id = await _repository.CreateAsync(tax);

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
            var tax = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170
            };
            var id = await _repository.CreateAsync(tax);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var tax = new Tax()
            {
                Name = "Tax Name Post1",
                Description = "Tax Description Post1",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170,
                Stores = new List<Store>()
            };


            // Act
            var result = await _service.Post(tax);

            // Assert
            Assert.Equal(tax.Name, (result as Tax).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var tax = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170,
                Stores = new List<Store>()
            };
            var id = await _repository.CreateAsync(tax);

            var taxUpdate = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170,
                State = true,
                Stores = new List<Store>()
            };

            // Act
            var result = await _service.Put(id, taxUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var taxUpdate = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, taxUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterTax_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var tax = new Tax()
            {
                Name = "Tax Name ",
                Description = "Tax Description ",
                Type = Entities.Enum.TaxType.PERCENT,
                Value = 150,
                SpecialValue = 170,
                State = true
            };

            var id = await _repository.CreateAsync(tax);
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.FilterTax(
                    0, 0, null, "", "");
            });

            // Assert
            Assert.Equal("Page parameter out of bounds.", exception.Message);
        }

        [Fact]
        public async Task FilterTax_NormalCall_ResultOk()
        {
            // Arrange
            var taxs = new List<Tax>();
            for (var i = 0; i < 10; i++)
            {
                taxs.Add(new Tax()
                {
                    Name = "Tax Name" + i,
                    Description = "Tax Description" + i,
                    Type = Entities.Enum.TaxType.PERCENT,
                    Value = 150,
                    SpecialValue = 170,
                    State = true
                });
            }

            var id = await _repository.CreateRangeAsync<Tax>(taxs);

            var filter = new TaxSearchModel();

            var result = await _service.FilterTax(
                    1, 10, filter, filter.Search = "", filter.Sort = "");

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task GetByStored_NormalCall_ResultOk()
        {
            // Arrange
            var taxs = new List<Tax>();
            for (var i = 0; i < 10; i++)
            {
                taxs.Add(new Tax()
                {
                    Name = "Tax Name" + i,
                    Description = "Tax Description" + i,
                    Type = Entities.Enum.TaxType.PERCENT,
                    Value = 150,
                    SpecialValue = 170,
                    State = true,
                    
                });
            }

            var id = await _repository.CreateRangeAsync<Tax>(taxs);

            var result = await _service.GetTaxByStore(1);

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetByStored_NormalCall_ResultNull()
        {
            // Arrange
            int storedId = -10000000;

            // Act
            var result = await _service.GetTaxByStore(storedId);

            // Assert
            Assert.True(result.Count == 0);
        }
    }
}