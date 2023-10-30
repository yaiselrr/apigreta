using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.StoreSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class StoreServiceTest
    {

        readonly StoreRepository _repository;
        readonly StoreService _service;

        public StoreServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(StoreServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new StoreRepository(TestsSingleton.Auth, context);
            _service = new StoreService(_repository, Mock.Of<ILogger<StoreService>>());
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
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 1
            };
            var id = await _repository.CreateAsync(store);
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
        public void GetAllIds_NormalCall_ResultOk()
        {
            // Arrange

            // Act
            var result = _service.GetAllIds().Result;

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public void GetStoresWithExternalScales_NormalCall_ResultOk()
        {
            // Arrange

            // Act
            var result = _service.GetStoresWithExternalScales().Result;

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public void GetForDashboard_NormalCall_ResultOk()
        {
            // Arrange

            // Act
            var result = _service.GetForDashboard().Result;

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetByRegion_NormalCall_ResultSameAsInsert()
        {
            // Arrange
            // Arrange
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 1
            };
            var id = await _repository.CreateAsync(store);
            // Act
            var result = await _service.GetByRegion(store.RegionId);

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetByRegion_CallNotExist_ReturnNull()
        {
            // Arrange
            long id = 1000;
            // Act
            var result = await _service.GetByRegion(id);

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public async Task GetWithStores_NormalCall()
        {
            // Arrange
            var reg = await _repository.CreateAsync<Region>(new Region()
            {
                Name = "reg _fil_"
            });
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = reg.Id,
            };
            var id = await _repository.CreateAsync(store);
            // Act
            var result = await _service.GetWithStores(id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByGuid_NormalCall()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 1,
                GuidId = guid
            };
            var id = await _repository.CreateAsync(store);
            // Act
            var result = await _service.GetByGuid(guid);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FilterStore_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 1,
            };

            var id = await _repository.CreateAsync(store);

            var spec = new StoreFilterSpec(
               new StoreSearchModel()
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
        public async Task FilterStore_NormalCall_ResultOk()
        {
            var stores = new List<Store>();

            var reg = await _repository.CreateAsync<Region>(new Region()
            {
                Name = "reg _fil_"
            });
            for (var i = 0; i < 10; i++)
            {
                stores.Add(new Store()
                {
                    Name = "_fil_ Store Name " + i,
                    RegionId = reg.Id,
                    IsDeleted = false
                });
            }
            var id = await _repository.CreateRangeAsync<Store>(stores);
            
            var spec = new StoreFilterSpec(
                new StoreSearchModel()
                {
                    Name = "_fil_",
                    IsDeleted = false
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        // [Fact]
        // public async Task GetWithStores_NormalCall_ResultOk()
        // {
        //     // Arrange
        //     var store = new Store()
        //     {
        //         Name = "Store Name test ",
        //         RegionId = 184,
        //     };
        //     var regId = _repository.GetEntity<Region>().Add(new Region()
        //     {
        //         Id = 184,
        //         Name = "Region 1rew test"
        //     });
        //     await _repository.SaveChangesAsync();
        //     var stores = new List<Store>();
        //     for (var i = 0; i < 10; i++)
        //     {
        //         stores.Add(new Store()
        //         {
        //             Name = "_fil_ Store Name test ",
        //             RegionId = 184,
        //         });
        //     }
        //     var id = await _repository.CreateRangeAsync<Store>(stores);

        //     var filter = new StoreSearchModel();

        //     var result = await _service.GetWithStores(
        //             1, 11, "_fil_", 184, 0);

        //     // Assert
        //     Assert.Equal(10, result.Data.Count);
        // }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 164,
            };
            var id = await _repository.CreateAsync(store);

            var storeUpdate = new Store()
            {
                Id = id,
                Name = "Store Name Update",
                RegionId = 164,
                State = true
            };

            // Act
            var result = await _service.Put(id, storeUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var storeUpdate = new Store()
            {
                Id = id,
                Name = "Store Name Update",
                RegionId = 164,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, storeUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }
        [Fact]
        public async Task PutConfiguration_NormalCall_ChangeSuccess()
        {
            // Arrange
            var store = new Store()
            {
                Name = "Store Name ",
                RegionId = 164,
            };
            var id = await _repository.CreateAsync(store);

            var storeUpdate = new Store()
            {
                Id = id,
                Name = "Store Name Update",
                RegionId = 164,
                State = true
            };

            // Act
            var result = await _service.PutConfiguration(id, storeUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task PutConfiguration_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var storeUpdate = new Store()
            {
                Id = id,
                Name = "Store Name Update",
                RegionId = 164,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PutConfiguration (id, storeUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }
    }
}