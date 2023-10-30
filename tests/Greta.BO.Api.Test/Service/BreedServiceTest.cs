﻿using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Breed;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.BreedSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class BreedServiceTest
    {

        readonly BreedRepository _repository;
        readonly BreedService _service;

        public BreedServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(BreedServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new BreedRepository(TestsSingleton.Auth, context);
            _service = new BreedService(_repository, Mock.Of<ILogger<BreedService>>());
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
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };
            var id = await _repository.CreateAsync(breed);
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
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };
            var id = await _repository.CreateAsync(breed);

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
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };
            var id = await _repository.CreateAsync(breed);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };

            // Act
            var result = await _service.Post(breed);

            // Assert
            Assert.Equal(breed.Name, (result as Breed).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };
            var id = await _repository.CreateAsync(breed);

            var breedUpdate = new Breed()
            {
                Id = id,
                Name = "Breed Name Update",
                AnimalBreedType = 0,
                Maxx = 2,
                State = true
            };

            // Act
            var result = await _service.Put(id, breedUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var breedUpdate = new Breed()
            {
                Name = "Breed Name Update",
                AnimalBreedType = 0,
                Maxx = 2,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, breedUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterBreed_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var breed = new Breed()
            {
                Name = "Breed Name ",
                AnimalBreedType = 0,
                Maxx = 2
            };

            var id = await _repository.CreateAsync(breed);

            var spec = new BreedFilterSpec(
               new BreedSearchModel()
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
        public async Task FilterBreed_NormalCall_ResultOk()
        {
            // Arrange
            var cats = new List<Breed>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new Breed()
                {
                    Name = "_fil_ Breed Name " + i,
                    AnimalBreedType = 0,
                    Maxx = 2
                });
            }
            var id = await _repository.CreateRangeAsync<Breed>(cats);

            var spec = new BreedFilterSpec(
                new BreedSearchModel()
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