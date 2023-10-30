using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.GrindSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class GrindServiceTest
    {

        readonly GrindRepository _repository;
        readonly GrindService _service;

        public GrindServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(GrindServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new GrindRepository(TestsSingleton.Auth, context);
            _service = new GrindService(_repository, Mock.Of<ILogger<GrindService>>());
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
            var grind = new Grind()
            {
                Name = "Grind Name "
            };
            var id = await _repository.CreateAsync(grind);
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
            var grind = new Grind()
            {
                Name = "Grind Name "
            };
            var id = await _repository.CreateAsync(grind);

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
            var grind = new Grind()
            {
                Name = "Grind Name "
            };
            var id = await _repository.CreateAsync(grind);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var grind = new Grind()
            {
                Name = "Grind Name "
            };

            // Act
            var result = await _service.Post(grind);

            // Assert
            Assert.Equal(grind.Name, (result as Grind).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var grind = new Grind()
            {
                Name = "Grind Name "
            };
            var id = await _repository.CreateAsync(grind);

            var grindUpdate = new Grind()
            {
                Id = id,
                Name = "Grind Name Update",
                State = true
            };

            // Act
            var result = await _service.Put(id, grindUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var grindUpdate = new Grind()
            {
                Name = "Grind Name Update",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, grindUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterGrind_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var grind = new Grind()
            {
                Name = "Grind Name "
            };

            var id = await _repository.CreateAsync(grind);

            var spec = new GrindFilterSpec(
               new GrindSearchModel()
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
        public async Task FilterGrind_NormalCall_ResultOk()
        {
            // Arrange
            var grinds = new List<Grind>();
            for (var i = 0; i < 10; i++)
            {
                grinds.Add(new Grind()
                {
                    Name = "_fil_ Grind Name " + i
                });
            }
            var id = await _repository.CreateRangeAsync<Grind>(grinds);

            var spec = new GrindFilterSpec(
                new GrindSearchModel()
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