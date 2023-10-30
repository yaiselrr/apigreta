using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ScalendarSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class ScalendarServiceTest
    {

        readonly ScalendarRepository _repository;
        readonly ScalendarService _service;

        public ScalendarServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(ScalendarServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new ScalendarRepository(TestsSingleton.Auth, context);
            _service = new ScalendarService(_repository, Mock.Of<ILogger<ScalendarService>>());
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
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };
            var id = await _repository.CreateAsync(scalendar);
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
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };
            var id = await _repository.CreateAsync(scalendar);

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
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };
            var id = await _repository.CreateAsync(scalendar);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };

            // Act
            var result = await _service.Post(scalendar);

            // Assert
            Assert.Equal(scalendar.Day, (result as Scalendar).Day);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };
            var id = await _repository.CreateAsync(scalendar);

            var scalendarUpdate = new Scalendar()
            {
                Id = id,
                DayId = 1,
                Day = "Monday Update",
                State = true,
                Breeds = new List<Breed>()
            };

            // Act
            var result = await _service.Put(id, scalendarUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var scalendarUpdate = new Scalendar()
            {
                DayId = 1,
                Day = "Monday Update",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, scalendarUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterScalendar_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var scalendar = new Scalendar()
            {
                DayId = 1,
                Day = "Monday"
            };
            var id = await _repository.CreateAsync(scalendar);

            var spec = new ScalendarFilterSpec(
               new ScalendarSearchModel()
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
        public async Task FilterScalendar_NormalCall_ResultOk()
        {
            // Arrange
            var scals = new List<Scalendar>();
            for (var i = 0; i < 10; i++)
            {
                scals.Add(new Scalendar()
                {
                    DayId = 2 + i,
                    Day = "_fil_ Monday"
                });
            }
            var id = await _repository.CreateRangeAsync<Scalendar>(scals);

            var spec = new ScalendarFilterSpec(
                new ScalendarSearchModel()
                {
                    Day = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }
    }
}