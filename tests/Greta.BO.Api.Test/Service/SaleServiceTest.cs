using System;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greta.BO.Api.Test.Service
{
    public class SaleServiceTest
    {
        readonly SaleRepository _repository;
        readonly SaleService _service;
        private SqlServerContext _context;
        private static long _count = 0;
        public SaleServiceTest()
        {

            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(CategoryServiceTest)).Options;
            _context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new SaleRepository(TestsSingleton.Auth, _context);
            
            // var ss = new SynchroService()
             _service = new SaleService(_repository, null, Mock.Of<ILogger<SaleService>>());
             _count = 0;
        }

        // [Fact]
        // public Task TestTimeZoneOffset()
        // {
        //     var ts = TimeZoneInfo.GetSystemTimeZones();
        //     if (ts.Count > 0)
        //     {
        //         var t = ts[0];
        //     }
        //
        //     return Task.CompletedTask;
        // }
        
        [Fact]
        public async Task SaleOnlyOnChild_NotAfectParent()
        {
            // Arrange
            var parent = new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                Parent = null,
                QtyHand = 1
            };
            await _repository.CreateAsync(parent);
            var child =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                ParentId = parent.Id,
                QtyHand = 9
            });
            await _service.UpdateBreakPack(_context, child, 5);
            // Act
            var result = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child.Id);

            // Assert
            Assert.True(result.QtyHand == 4);
        }
        
        [Fact]
        public async Task SaleChild_AfectParent()
        {
            // Arrange
            var parent = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                Parent = null,
                QtyHand = 2,
                StoreId = 3 
            });
            var child =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = parent.Id,
                QtyHand = 9,
                StoreId = 3 
            });
            await _service.UpdateBreakPack(_context, child, 10);
            // Act
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            Assert.Equal(9, resultChild.QtyHand );
            Assert.Equal(1,  resultParent.QtyHand);
        }
        
        [Fact]
        public async Task SaleChild_AfectParentAnObtainNegativesOnParent()
        {
            // Arrange
            var parent = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                Parent = null,
                QtyHand = 1,
                StoreId = 3 
            });
            var child =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = parent.Id,
                QtyHand = 9,
                StoreId = 3 
            });
            await _service.UpdateBreakPack(_context, child, 21);
            // Act
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            // Assert.True(resultChild.QtyHand == 0 && resultParent.QtyHand == -1);
            Assert.Equal(0, resultChild.QtyHand );
            Assert.Equal(-1,  resultParent.QtyHand);
        }
        
        [Fact]
        public async Task SaleChild_AfectParentAndRefillChild()
        {
            // Arrange
            var parent = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                Parent = null,
                QtyHand = 2,
                StoreId = 3 
            });
            var child =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = parent.Id,
                QtyHand = 10,
                StoreId = 3 
            });
            var rParent = await _context.Set<StoreProduct>()
                .Include(x => x.Parent)
                .Where(x => x.Id == child.Id)
                .FirstOrDefaultAsync();
            await _service.UpdateBreakPack(_context, rParent, 10);
            // Act
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            Assert.Equal(10, resultChild.QtyHand);
            Assert.Equal(1, resultParent.QtyHand);
        }
        
        [Fact]
        public async Task SaleChild_AfectParentAndRefillChild3levels1()
        {
            // Arrange
            var parent = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 0,
                Parent = null,
                QtyHand = 4,
                StoreId = 3 
            });
            var child1 =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 6,
                ParentId = parent.Id,
                QtyHand = 1,
                StoreId = 3 
            });
            var child2 =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = child1.Id,
                QtyHand = 10,
                StoreId = 3 
            });
            var rParent = await _context.Set<StoreProduct>()
                .Include(x => x.Parent)
                .Where(x => x.Id == child2.Id)
                .FirstOrDefaultAsync();
            await _service.UpdateBreakPack(_context, rParent, 11);
            // Act
            var resultChild2 = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child2.Id);
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child1.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            Assert.Equal(9,resultChild2.QtyHand ); 
            Assert.Equal(6,resultChild.QtyHand );
            Assert.Equal(3,resultParent.QtyHand );
        }
        
        [Fact]
        public async Task SaleChild_AfectParentAndRefillChild3levels2()
        {
            // Arrange
            var parent = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                Parent = null,
                QtyHand = 4,
                StoreId = 3 
            });
            var child1 = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 6,
                ParentId = parent.Id,
                QtyHand = 1,
                StoreId = 3 
            });
            var child2 = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = child1.Id,
                QtyHand = 10,
                StoreId = 3 
            });
            var rParent = await _context.Set<StoreProduct>()
                .Include(x => x.Parent)
                .Where(x => x.Id == child2.Id)
                .FirstOrDefaultAsync();
            await _service.UpdateBreakPack(_context, rParent, 31);
            // await service.UpdateBreakPack(context, child2, 31);
            //await service.UpdateBreakPack(context, test, 10);
            // Act
            var resultChild2 = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child2.Id);
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child1.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            Assert.Equal(9, resultChild2.QtyHand); 
            Assert.Equal(4, resultChild.QtyHand);
            Assert.Equal(3, resultParent.QtyHand);
        }
        
        [Fact]
        public async Task SaleChild_BuyMoreThanExistenses()
        {
            // Arrange
            var parent =  await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                Parent = null,
                QtyHand = 1,
                StoreId = 3 
            });
            var child1 = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 6,
                ParentId = parent.Id,
                QtyHand = 1,
                StoreId = 3 
            });
            var child2 = await _repository.CreateAsync(new StoreProduct()
            {
                //Id = ++count,
                SplitCount = 10,
                ParentId = child1.Id,
                QtyHand = 10,
                StoreId = 3 
            });
            var rParent = await _context.Set<StoreProduct>()
                .Include(x => x.Parent)
                .Where(x => x.Id == child2.Id)
                .FirstOrDefaultAsync();
            await _service.UpdateBreakPack(_context, rParent, 82);
            // await service.UpdateBreakPack(context, child2, 82);
            //await service.UpdateBreakPack(context, test, 10);
            // Act
            var resultChild2 = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child2.Id);
            var resultChild = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == child1.Id);
            var resultParent = await _repository.GetEntity<StoreProduct>().FirstOrDefaultAsync(x => x.Id == parent.Id);

            // Assert
            Assert.Equal(0, resultChild2.QtyHand); 
            Assert.Equal(0, resultChild.QtyHand);
            Assert.Equal(-1, resultParent.QtyHand);
        }
    }
}