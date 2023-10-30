using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.RegionSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class RegionServiceTest
    {
        readonly RegionRepository _repository;
        readonly RegionService _service;

        public RegionServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(RegionServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new RegionRepository(TestsSingleton.Auth, context);
            _service = new RegionService(_repository, Mock.Of<ILogger<RegionService>>());
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
            var region = new Region()
            {
                Name = "Region Name "
            };
            var id = await _repository.CreateAsync(region);
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
            var region = new Region()
            {
                Name = "Region Name "
            };
            var id = await _repository.CreateAsync(region);

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
            var region = new Region()
            {
                Name = "Region Name "
            };
            var id = await _repository.CreateAsync(region);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var region = new Region()
            {
                Name = "Region Name "
            };

            // Act
            var result = await _service.Post(region);

            // Assert
            Assert.Equal(region.Name, (result as Region).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var region = new Region()
            {
                Name = "Region Name "
            };
            var id = await _repository.CreateAsync(region);

            var regionUpdate = new Region()
            {
                Id = id,
                Name = "Region Name Update",
                State = true
            };

            // Act
            var result = await _service.Put(id, regionUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var regionUpdate = new Region()
            {
                Name = "Region Name Update",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, regionUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterRegion_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var region = new Region()
            {
                Name = "Region Name "
            };

            var id = await _repository.CreateAsync(region);

            var spec = new RegionFilterSpec(
                new RegionSearchModel()
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
        public async Task FilterRegion_NormalCall_ResultOk()
        {
            // Arrange
            var cats = new List<Region>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new Region()
                {
                    Name = "_fil_ Region Name " + i
                });
            }

            var id = await _repository.CreateRangeAsync<Region>(cats);

            var spec = new RegionFilterSpec(
                new RegionSearchModel()
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