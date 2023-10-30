using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Customer;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.CustomerSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class CustomerServiceTest
    {

        readonly CustomerRepository _repository;
        readonly CustomerService _service;

        readonly IConfiguration _configuration;
        readonly ISynchroService _synchroService;

        public CustomerServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(CustomerServiceTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new CustomerRepository(TestsSingleton.Auth, context);
            _service = new CustomerService(_repository, Mock.Of<ISynchroService>(), Mock.Of<IConfiguration>(), Mock.Of<ILogger<CustomerService>>());
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
            var customer = new Customer()
            {
                LastName = "Pan",
                FirstName = "Peter",
                Phone = "(949) 375-1998",
                Email = "peter@gmail.com",
            };
            var id = await _repository.CreateAsync(customer);
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
            var customer = new Customer()
            {
                LastName = "Pan1",
                FirstName = "Peter1",
                Phone = "(949) 375-1999",
                Email = "peter1@gmail.com",
            };
            var id = await _repository.CreateAsync(customer);

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
            var customer = new Customer()
            {
                LastName = "Pan2",
                FirstName = "Peter2",
                Phone = "(949) 375-1997",
                Email = "peter2@gmail.com",
            };
            var id = await _repository.CreateAsync(customer);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan3",
                FirstName = "Peter3",
                Phone = "(949) 375-1996",
                Email = "peter3@gmail.com",
            };
            // Act
            var result = await _service.Post(customer);

            // Assert
            Assert.Equal(customer.FirstName, (result as Customer).FirstName);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan4",
                FirstName = "Peter4",
                Phone = "(949) 375-1995",
                Email = "peter4@gmail.com",
            };
            var id = await _repository.CreateAsync(customer);

            var customerUpdate = new Customer()
            {
                Id = id,
                LastName = "Pan416",
                FirstName = "Peter416",
                Phone = "(949) 374-1995",
                Email = "peter416@gmail.com",
                Discounts = new List<Discount>(),
                MixAndMatches = new List<MixAndMatch>()
            };

            // Act
            var result = await _service.Put(id, customerUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var customerUpdate = new Customer()
            {
                LastName = "Pan41",
                FirstName = "Peter41",
                Phone = "(949) 375-1995",
                Email = "peter41@gmail.com",
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, customerUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterCustomer_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan415",
                FirstName = "Peter415",
                Phone = "(949) 375-1994",
                Email = "peter415@gmail.com",
            };

            var id = await _repository.CreateAsync(customer);

            var spec = new CustomerFilterSpec(
               new CustomerSearchModel()
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
        public async Task FilterCustomer_NormalCall_ResultOk()
        {
            // Arrange
            var custs = new List<Customer>();

            for (var i = 0; i < 10; i++)
            {
                custs.Add(new Customer()
                {
                    LastName = "_fil_ Pan415" + i,
                    FirstName = "_fil_ Peter415" + i,
                    Phone = "(949) 375-197" + i,
                    Email = "peter415" + i + "@gmail.com",
                });
            }
            var id = await _repository.CreateRangeAsync<Customer>(custs);

            var spec = new CustomerFilterSpec(
                new CustomerSearchModel()
                {
                    LastName = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task SearchByPhone_NormalCallOk()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan415",
                FirstName = "Peter415",
                Phone = "(949) 375-1994",
                Email = "peter415@gmail.com",
            };

            var id = await _repository.CreateAsync(customer);
            // Act
            var result = await _service.SearchByPhone(customer.Phone);
            // Assert
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task SearchByPhone_NormalCallFalse()
        {
            // Arrange
            // Act
            var result = await _service.SearchByPhone("(949) 300-0004");
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByName_NormalCall_ResultNull()
        {
            // Arrange
            string name = "sdkfskdjhfkds";

            // Act
            var result = await _service.GetByName(name, -1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByName_NormalCallOk()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan4156",
                FirstName = "Peter4156",
                Phone = "(949) 375-1984",
                Email = "peter4156@gmail.com",
            };

            var id = await _repository.CreateAsync(customer);
            // Act
            var result = await _service.GetByName(customer.FirstName, -1);

            // Assert
            Assert.Equal(id, result.Id);
        }


        [Fact]
        public async Task GetByNameId_NormalCallOk()
        {
            // Arrange
            var customer = new Customer()
            {
                LastName = "Pan41568",
                FirstName = "Peter41568",
                Phone = "(949) 375-1988",
                Email = "peter41568@gmail.com",
            };

            var id = await _repository.CreateAsync(customer);
            // Act
            var result = await _service.GetByName(customer.FirstName);

            // Assert
            Assert.Equal(id, result.Id);
        }

        // [Fact]
        // public async Task UpdatePoints_NormalCall()
        // {
        //     // Arrange
        //     var custs = new List<Customer>();
        //
        //     for (var i = 0; i < 10; i++)
        //     {
        //         custs.Add(new Customer()
        //         {
        //             LastName = "Pan515" + i,
        //             FirstName = "_fil_ Peter515" + i,
        //             Phone = "(949) 475-197" + i,
        //             Email = "peter515" + i + "@gmail.com",
        //         });
        //     }
        //     var id = await _repository.CreateRangeAsync<Customer>(custs);
        //     // Act
        //     await _service.UpdatePoints(custs);
        //
        //     // Assert
        //     Assert.Equal(1, 1);
        // }
    }
}