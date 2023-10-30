using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Greta.BO.Api.Test.Helpers;
using Xunit;

namespace Greta.BO.Api.Test.Repository
{
    public class VendorRepositoryTest
    {
        public VendorRepositoryTest()
        {
        }

        private VendorRepository Get_Repository()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();

            var options = builder.UseInMemoryDatabase(nameof(VendorRepositoryTest)).Options;

            var context = new SqlServerContext(options, TestsSingleton.Auth);

            var repository = new VendorRepository(TestsSingleton.Auth, context);
            return repository;
        }

        /// <summary>
        /// Validate that an entity can be created and the record id is returned
        /// </summary>
        [Fact]
        public async Task CreateAsync_CreateEntity_ReturnId()
        {
            // Arrange
            var vendor = new Vendor()
            {
                Name = "Create - Name Vendor",
                Note = "Note",
                MinimalOrder = 200
            };

            var repository = Get_Repository();

            // Act
            var id = await repository.CreateAsync(vendor);

            // Assert
            Assert.Equal(vendor.Id, id);
            Assert.Equal("Create - Name Vendor", vendor.Name);
            Assert.Equal(200, vendor.MinimalOrder);
            Assert.True(vendor.State);
            Assert.Equal(TestsSingleton.Auth.UserId, vendor.UserCreatorId);
            Assert.True(vendor.CreatedAt > DateTime.MinValue);
        }


        /// <summary>
        /// Validate that an entity can be updated and true is returned
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateEntity_ReturnTrue()
        {
            // Arrange
            var vendor = new Vendor()
            {
                Name = "Create - Name Vendor",
                Note = "Note",
                MinimalOrder = 200
            };

            var repository = Get_Repository();

            var id = await repository.CreateAsync(vendor);

            // Act
            var vendorUpdate = new Vendor()
            {
                Name = "Create - Name Vendor1",
                Note = "Note",
                MinimalOrder = 300,
                State = true
            };

            var success = await repository.UpdateAsync(id, vendorUpdate);

            // Assert
            var entity = await repository.GetEntity<Vendor>().FindAsync(id);

            Assert.True(success);
            Assert.Equal(id, entity.Id);
            Assert.Equal("Create - Name Vendor1", entity.Name);
            Assert.Equal(300, vendor.MinimalOrder);
            Assert.True(entity.State);
            Assert.Equal(TestsSingleton.Auth.UserId, entity.UserCreatorId);
            Assert.Equal(vendor.CreatedAt, entity.CreatedAt);
        }

        /// <summary>
        /// Validate that an entity cannot be updated and false is returned
        /// </summary>
        [Fact]
        public async Task UpdateAsync_EntityNotExist_ReturnFalse()
        {
            // Arrange

            var repository = Get_Repository();

            // Act
            var vendor = new Vendor()
            {
                Name = "Create - Name Vendor",
                Note = "Note",
                MinimalOrder = 200
            };

            var success = await repository.UpdateAsync(new Random().Next(1, int.MaxValue), vendor);

            // Assert
            Assert.False(success);
        }

        /// <summary>
        /// Validate that an entity can be removed and true is returned
        /// </summary>
        [Fact]
        public async Task DeleteAsync_DeleteEntity_ReturnTrue()
        {
            // Arrange
            var vendor = new Vendor()
            {
                Name = "Create - Name Vendor",
                Note = "Note",
                MinimalOrder = 200
            };

            var repository = Get_Repository();

            var id = await repository.CreateAsync(vendor);

            // Act
            var success = await repository.DeleteAsync(id);

            // Assert
            Assert.True(success);
        }

        /// <summary>
        /// Validate that an entity cannot be deleted and false is returned
        /// </summary>
        [Fact]
        public async Task DeleteAsync_EntityNotExist_ReturnFalse()
        {
            // Arrange
            var repository = Get_Repository();

            // Act
            var success = await repository.DeleteAsync(new Random().Next(1, int.MaxValue));

            // Assert
            Assert.False(success);
        }
    }
}
