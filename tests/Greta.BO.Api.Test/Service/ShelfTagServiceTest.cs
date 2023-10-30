using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ShelfTagSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class ShelfTagServiceTest
    {
        readonly ShelfTagRepository _repository;
        readonly ShelfTagService _service;

        public ShelfTagServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(ShelfTagServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new ShelfTagRepository(TestsSingleton.Auth, context);
            _service = new ShelfTagService(_repository, Mock.Of<ILogger<ShelfTagService>>());
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
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);
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
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);
            // Act
            var result = await _service.PutByQty(id, 50);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var shelfTagUpdate = new ShelfTag()
            {
                Id = id,
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI",
                State = true

            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PutByQty(id, 70);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterShelfTag_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };

            var id = await _repository.CreateAsync(shelfTag);

            var spec = new ShelfTagFilterSpec(
                new ShelfTagSearchModel()
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
        public async Task FilterShelfTag_NormalCall_ResultOk()
        {
            // Arrange
            var shelfTags = new List<ShelfTag>();
            for (var i = 0; i < 10; i++)
            {
                shelfTags.Add(new ShelfTag()
                {
                    BinLocationId = 0,
                    BinLocationName = null,
                    CasePack = 10,
                    CategoryId = 1,
                    CategoryName = "Grocery",
                    DepartmentId = 1,
                    DepartmentName = "Store Coupon",
                    Price = 165,
                    ProductCode = "123456",
                    ProductId = 7478,
                    ProductName = "_fil_ 101 CIDER HOUSE BLACK DOG 12oz 4PK CANS" + i,
                    QTYToPrint = 20,
                    StoreId = 3,
                    StoreName = "Yavpe` Market",
                    UPC = "0471534812",
                    VendorId = 267,
                    VendorName = "LIPARI"
                });
            }

            var id = await _repository.CreateRangeAsync<ShelfTag>(shelfTags);

            var spec = new ShelfTagFilterSpec(
                new ShelfTagSearchModel()
                {
                    ProductName = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task DeleteByStore_NormalCall()
        {
            // Arrange            
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);
            // Act
            var result = await _service.DeleteByStore(3);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PostFromVendorProduct_NormalCall()
        {
            // Arrange
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);

            // Act
            var result = await _service.PostFromVendorProduct(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PostFromVendorProduct_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long vendorProductId = -1000;
            var shelfTagUpdate = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI",
                State = true

            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PostFromVendorProduct(vendorProductId);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task PostFromStoreProduct_NormalCall()
        {
            // Arrange
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);

            // Act
            var result = await _service.PostFromStoreProduct(5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PostFromStoreProduct_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long storeId = -1000;
            var shelfTagUpdate = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI",
                State = true

            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PostFromStoreProduct(storeId);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task PostFromCategory_NormalCall()
        {
            // Arrange
            var shelfTag = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI"

            };
            var id = await _repository.CreateAsync(shelfTag);

            // Act
            var result = await _service.PostFromCategory(shelfTag.CategoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PostFromCategory_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var shelfTagUpdate = new ShelfTag()
            {
                BinLocationId = 0,
                BinLocationName = null,
                CasePack = 10,
                CategoryId = 1,
                CategoryName = "Grocery",
                DepartmentId = 1,
                DepartmentName = "Store Coupon",
                Price = 165,
                ProductCode = "123456",
                ProductId = 7478,
                ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS",
                QTYToPrint = 20,
                StoreId = 3,
                StoreName = "Yavpe` Market",
                UPC = "0471534812",
                VendorId = 267,
                VendorName = "LIPARI",
                State = true

            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PostFromCategory(id);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task DeleteRangeShelfTags_ResultTrue()
        {
            // Arrange
            var shelfTags = new List<ShelfTag>();

            for (var i = 0; i < 10; i++)
            {
                shelfTags.Add(new ShelfTag()
                {
                    BinLocationId = 0,
                    BinLocationName = null,
                    CasePack = 10,
                    CategoryId = 1,
                    CategoryName = "Grocery",
                    DepartmentId = 1,
                    DepartmentName = "Store Coupon",
                    Price = 165,
                    ProductCode = "123456",
                    ProductId = 7478,
                    ProductName = "101 CIDER HOUSE BLACK DOG 12oz 4PK CANS" + i,
                    QTYToPrint = 20,
                    StoreId = 3,
                    StoreName = "Yavpe` Market",
                    UPC = "0471534812",
                    VendorId = 267,
                    VendorName = "LIPARI"
                });
            }

            var id = await _repository.CreateRangeAsync<ShelfTag>(shelfTags);

            List<long> list = new List<long>();

            for (int i = 0; i < shelfTags.Length(); i++)
            {
                list.Add(shelfTags[i].Id);
            }

            // Act
            var result = await _service.DeleteRange(list);

            // Assert
            Assert.True(result);
        }
    }
}