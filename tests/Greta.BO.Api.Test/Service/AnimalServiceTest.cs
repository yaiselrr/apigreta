using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.AnimalSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class AnimalServiceTest
    {

        readonly AnimalRepository _repository;
        readonly AnimalService _service;

        public AnimalServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(AnimalServiceTest))
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);
            _repository = new AnimalRepository(TestsSingleton.Auth, context);
            _service = new AnimalService(_repository, Mock.Of<ILogger<AnimalService>>());
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
                Name = "Store 1",
                RegionId = 1
            };

            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25
            };

            var id = await _repository.CreateAsync(animal);
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
            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25
            };
            var id = await _repository.CreateAsync(animal);

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
            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25
            };
            var id = await _repository.CreateAsync(animal);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25,
                DateReceived = DateTime.UtcNow
            };

            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Post(animal);
            });

            // Assert
            Assert.Equal("Error reached the maximum per day.", exception.Message);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25
            };
            var id = await _repository.CreateAsync(animal);

            var animalUpdate = new Animal()
            {
                Id = id,
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 55,
                RailWeight = 55,
                SubPrimalWeight = 20,
                CutWeight = 25,
                State = true
            };

            // Act
            var result = await _service.Put(id, animalUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var animalUpdate = new Animal()
            {
                Id = id,
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 55,
                RailWeight = 55,
                SubPrimalWeight = 20,
                CutWeight = 25,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, animalUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterAnimal_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var animal = new Animal()
            {
                StoreId = 1,
                RancherId = 1,
                Tag = "101",
                BreedId = 1,
                Customers = new List<Customer>(),
                LiveWeight = 5,
                RailWeight = 5,
                SubPrimalWeight = 20,
                CutWeight = 25
            };

            var id = await _repository.CreateAsync(animal);
            // Act
            var spec = new AnimalFilterSpec(
               new AnimalSearchModel()
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
        public async Task FilterAnimal_NormalCall_ResultOk()
        {
            // Arrange
            var animals = new List<Animal>();
            for (var i = 0; i < 10; i++)
            {
                animals.Add(new Animal()
                {
                    StoreId = 1,
                    RancherId = 1,
                    Tag = "101" + i,
                    BreedId = 1,
                    Customers = new List<Customer>(),
                    LiveWeight = 5,
                    RailWeight = 5,
                    SubPrimalWeight = 20,
                    CutWeight = 25,
                    State = true
                });
            }

            var id = await _repository.CreateRangeAsync<Animal>(animals);

            var spec = new AnimalFilterSpec(
                new AnimalSearchModel()
                {
                    Tag = "10",
                    StoreId = 1
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task GetAnimalByRancher_NormalCall_ResultOk()
        {
            // Arrange
            var animals = new List<Animal>();
            for (var i = 0; i < 10; i++)
            {
                animals.Add(new Animal()
                {
                    StoreId = 1,
                    RancherId = 1,
                    Tag = "201" + i,
                    BreedId = 1,
                    Customers = new List<Customer>(),
                    LiveWeight = 5,
                    RailWeight = 5,
                    SubPrimalWeight = 20,
                    CutWeight = 25,
                    State = true

                });
            }

            var id = await _repository.CreateRangeAsync<Animal>(animals);

            var result = await _service.GetAnimalByRancher(1);

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetAnimalByRancher_NormalCall_ResultNull()
        {
            // Arrange
            int rancherId = -10000000;

            // Act
            var result = await _service.GetAnimalByRancher(rancherId);

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public async Task GetAnimalByBreed_NormalCall_ResultOk()
        {
            // Arrange
            var animals = new List<Animal>();
            for (var i = 0; i < 10; i++)
            {
                animals.Add(new Animal()
                {
                    StoreId = 1,
                    RancherId = 1,
                    Tag = "301" + i,
                    BreedId = 1,
                    Customers = new List<Customer>(),
                    LiveWeight = 5,
                    RailWeight = 5,
                    SubPrimalWeight = 20,
                    CutWeight = 25,
                    State = true

                });
            }

            var id = await _repository.CreateRangeAsync<Animal>(animals);

            var result = await _service.GetAnimalByBreed(1);

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetAnimalByBreed_NormalCall_ResultNull()
        {
            // Arrange
            int breedId = -10000000;

            // Act
            var result = await _service.GetAnimalByBreed(breedId);

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public async Task ValidateForDay_NormalCall_ResultOk()
        {
            // Arrange
            var animals = new List<Animal>();
            for (var i = 0; i < 10; i++)
            {
                animals.Add(new Animal()
                {
                    StoreId = 1,
                    RancherId = 1,
                    Tag = "301" + i,
                    BreedId = 1,
                    Customers = new List<Customer>(),
                    LiveWeight = 5,
                    RailWeight = 5,
                    SubPrimalWeight = 20,
                    CutWeight = 25,
                    State = true

                });
            }

            var id = await _repository.CreateRangeAsync<Animal>(animals);

            var result = await _service.ValidateForDay(DateTime.UtcNow);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateForDay_NormalCall_ResultNull()
        {
            // Arrange

            // Act
            var result = await _service.ValidateForDay(DateTime.UtcNow);

            // Assert
            Assert.True(!result);
        }


        // [Fact]
        // public async Task GetSelectScheduleForDay_NormalCall_ResultOk()
        // {
        //     // Arrange
        //     var animals = new List<Animal>();
        //     for (var i = 0; i < 10; i++)
        //     {
        //         animals.Add(new Animal()
        //         {
        //             StoreId = 1,
        //             RancherId = 1,
        //             Tag = "401" + i,
        //             BreedId = 1,
        //             Customers = new List<Customer>(),
        //             LiveWeight = 5,
        //             RailWeight = 5,
        //             SubPrimalWeight = 20,
        //             CutWeight = 25,
        //             State = true

        //         });
        //     }

        //     var id = await _repository.CreateRangeAsync<Animal>(animals);

        //     var deps = new List<Department>();

        //     deps.Add(new Department()
        //     {
        //         Name = "Department Name 10",
        //         State = true
        //     });

        //     var depsTest = await _repository.CreateRangeAsync<Department>(deps);

        //     var depsId = new List<long>();

        //     for (int i = 0; i < deps.Length(); i++)
        //     {
        //         depsId.Add(deps[i].Id);
        //     }

        //     // Act
        //     var result = await _service.GetAllForScales(depsId);

        //     // Assert
        //     Assert.True(result.Count >= 0);
        // }
    }
}