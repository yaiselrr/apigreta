using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.CategorySpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class CategoryServiceTest
    {

        readonly CategoryRepository _repository;
        readonly CategoryService _service;

        public CategoryServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(CategoryServiceTest))
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new CategoryRepository(TestsSingleton.Auth, context);
            _service = new CategoryService(_repository, Mock.Of<ISynchroService>(),Mock.Of<ILogger<CategoryService>>());
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
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description ",
                DepartmentId = 3,
                Department = new Department()
                {
                    Id = 3,
                    Name = "Department 3"
                }
            };
            var id = await _repository.CreateAsync(category);
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
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description "
            };
            var id = await _repository.CreateAsync(category);

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
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description ",
                DepartmentId = 12,
                Department = new Department()
                {
                    Id = 12,
                    Name = "Department 12"
                }
            };
            var id = await _repository.CreateAsync(category);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description "
            };

            // Act
            var result = await _service.Post(category);

            // Assert
            Assert.Equal(category.Name, (result as Category).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description "
            };
            var id = await _repository.CreateAsync(category);

            var categoryUpdate = new Category()
            {
                Id = id,
                Name = "Category Name Update",
                Description = "Category Description Update",
                State = true
            };

            // Act
            var result = await _service.Put(id, categoryUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var categoryUpdate = new Category()
            {
                Name = "Category Name Update",
                Description = "Category Description Update",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, categoryUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterCategory_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description "
            };

            var id = await _repository.CreateAsync(category);

            var spec = new CategoryFilterSpec(
               new CategorySearchModel()
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
        public async Task FilterCategory_NormalCall_ResultOk()
        {
            // Arrange
            var category = new Category()
            {
                Name = "Category Name ",
                Description = "Category Description "
            };
            var depId = _repository.GetEntity<Department>().Add(new Department()
            {
                Id = 164,
                Name = "Department 1rew"
            });
            await _repository.SaveChangesAsync();
            var cats = new List<Category>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new Category()
                {
                    Name = "_fil_ Category Name " + i,
                    Description = "Category Description " + i,
                    
                    CategoryId = 11004 + i,
                    DepartmentId = 164
                });
            }
            var id = await _repository.CreateRangeAsync<Category>(cats);

            var spec = new CategoryFilterSpec(
                new CategorySearchModel()
                {
                    Name = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }


        [Fact]
        public async Task GetCategoryWithProduct_NormalCall_ResultSameAsInsert()
        {
            // Arrange
            var category = new Category()
            {
                CategoryId = 110003,
                Name = "Category Name ",
                Description = "Category Description ",
                DepartmentId = 2,
                Department = new Department()
                {
                    Id = 2,
                    Name = "Department 2"
                }
            };
            var id = await _repository.CreateAsync(category);
            // Act
            var result = await _service.GetCategoryWithProduct(category.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategoryWithProduct_NormalCall_ResultNull()
        {
            // Arrange
            long categoryId = 10000000;
            // Act
            var result = await _service.GetCategoryWithProduct(categoryId);

            // Assert
            Assert.Null(result);
        }
    }
}