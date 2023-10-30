using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Region;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.RegionSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class CutListServiceTest
    {

        readonly CutListRepository _repository;
        readonly CutListService _service;

        readonly CustomerRepository _repositoryCustomer;
        readonly AnimalRepository _repositoryAnimal;

        public CutListServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(CutListServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new CutListRepository(TestsSingleton.Auth, context);
            _service = new CutListService(_repository, Mock.Of<ILogger<CutListService>>());

            _repositoryCustomer = new CustomerRepository(TestsSingleton.Auth, context);
            _repositoryAnimal = new AnimalRepository(TestsSingleton.Auth, context);
        }

        [Fact]
        public async Task GetCustomerByAnimal_NormalCall_ResultOk()
        {
            // Arrange
            var name = "GetCustomerByAnimal_NormalCall_ResultOk";

            var customers = new List<Customer>();
            for (var i = 0; i < 3; i++)
            {
                customers.Add(new Customer()
                {
                    FirstName = "FirstName1 GetCustomerByAnimal" +i,
                    LastName = "LastName1 GetCustomerByAnimal" +i,
                    Phone = "Phone1 GetCustomerByAnimal" + i
                });
            }
            var custList = await _repositoryCustomer.CreateRangeAsync(customers);

            var animal = new Animal()
            {
                Tag = name
            };
            animal.Customers = new List<Customer>();
            animal.Customers.Add(customers[0]);
            animal.Customers.Add(customers[1]);
            var animalId = await _repositoryAnimal.CreateAsync(animal);

            // Act
            var result = await _service.GetCustomerByAnimal(animalId);
           
            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetScaleProductsByUpcAndPlu_NormalCall_ResultOk()
        {
            // Arrange

            var product = new Product() { Name = "Product ok", UPC = "UPC POK" };
            var prod = await _repository.CreateAsync(product);

            var store = new Store() { Name = "StoreOK" };
            var str = await _repository.CreateAsync(store);

            List<StoreProduct> products = new List<StoreProduct>();
            for (var i = 0; i < 3; i++)
            {
                products.Add(new StoreProduct()
                {
                    StoreId = str.Id,
                    ProductId = prod.Id                    
                });
            }
            var storeProducts = await _repository.CreateRangeAsync(products);

            List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 1OK", Description1 = "Desc1", UPC = "UPC 1OK", PLUNumber = 1, StoreProducts = storeProducts.Take(2).ToList() });
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 2OK", Description1 = "Desc2", UPC = "UPC 2OK", PLUNumber = 2, StoreProducts = storeProducts.Skip(2).ToList() });
            await _repository.CreateRangeAsync(scaleProducts);

            var animal = new Animal()
            {
                Tag = "GetScaleProductsByUpcAndPlu_NormalCall_Ok",
                StoreId = str.Id
            };
            
            var animalId = await _repositoryAnimal.CreateAsync(animal);

            // Act
            var result = await _service.GetScaleProductsByUpcAndPlu("UPC 1OK", 1, animalId);

            // Assert
            Assert.Equal(1, result.ToList().Count());
        }

        [Fact]
        public async Task GetScaleProductsByUpcAndPlu_WithInvalidCall_ResultBusinesLogicException()
        {
            // Arrange

            var product = new Product() { Name = "Product", UPC = "UPC P" };
            var prod = await _repository.CreateAsync(product);

            var store = new Store() { Name = "Store" };
            var str = await _repository.CreateAsync(store);

            List<StoreProduct> products = new List<StoreProduct>();
            for (var i = 0; i < 3; i++)
            {
                products.Add(new StoreProduct()
                {
                    StoreId = str.Id,
                    ProductId = prod.Id
                });
            }
            var storeProducts = await _repository.CreateRangeAsync(products);

            List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 1", Description1 = "Desc1", UPC = "UPC 1", PLUNumber = 1, StoreProducts = storeProducts.Take(2).ToList() });
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 2", Description1 = "Desc2", UPC = "UPC 2", PLUNumber = 2, StoreProducts = storeProducts.Skip(2).ToList() });
            await _repository.CreateRangeAsync(scaleProducts);

            var animal = new Animal()
            {
                Tag = "GetScaleProductsByUpcAndPlu_NormalCall_ResultOk",
                StoreId = str.Id
            };

            var animalId = await _repositoryAnimal.CreateAsync(animal);

            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.GetScaleProductsByUpcAndPlu("UPC 1", 0, animalId);
            });            

            // Assert
            Assert.Equal("Parameters out of bounds", exception.Message);
        }

        [Fact]
        public async Task GetScaleProductsOfTemplate_NormalCall_ResultOk()
        {
            // Arrange
                        
            var product = new Product() { Name = "Product", UPC = "UPC P" };
            var prod = await _repository.CreateAsync(product);

            var store = new Store() { Name = "Store" };
            var str = await _repository.CreateAsync(store);

            List<StoreProduct> products = new List<StoreProduct>();
            for (var i = 0; i < 3; i++)
            {
                products.Add(new StoreProduct()
                {
                    StoreId = str.Id,
                    ProductId = prod.Id
                });
            }
            var storeProducts = await _repository.CreateRangeAsync(products);

            List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 1", Description1 = "Desc1", UPC = "UPC 1", PLUNumber = 1, StoreProducts = storeProducts.Where(x => x.Id < 2).ToList() });
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 2", Description1 = "Desc2", UPC = "UPC 2", PLUNumber = 2, StoreProducts = storeProducts.Where(x => x.Id >= 2).ToList() });
            await _repository.CreateRangeAsync(scaleProducts);

            var template = new CutListTemplate() { Name = "Template 1", ScaleProducts = scaleProducts };
            var temp = await _repository.CreateAsync(template);           

            var animal = new Animal()
            {
                Tag = "GetScaleProductsOfTemplate_NormalCall_ResultOk",
                StoreId = str.Id
            };
           
            var animalId = await _repositoryAnimal.CreateAsync(animal);

            var cutList = new CutList()
            {
                AnimalId = animalId,
                CutListTemplateId = temp.Id,                
            };
            var cut = await _repository.CreateAsync(cutList);

            // Act
            var result = await _service.GetScaleProductsOfTemplate(temp.Id, animalId);

            // Assert
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task GetScaleProductsOfTemplate_WithInvalidCall_ResultBusinessLogicException()
        {
            // Arrange

            var product = new Product() { Name = "Product", UPC = "UPC P" };
            var prod = await _repository.CreateAsync(product);

            var store = new Store() { Name = "Store" };
            var str = await _repository.CreateAsync(store);

            List<StoreProduct> products = new List<StoreProduct>();
            for (var i = 0; i < 3; i++)
            {
                products.Add(new StoreProduct()
                {
                    StoreId = str.Id,
                    ProductId = prod.Id
                });
            }
            var storeProducts = await _repository.CreateRangeAsync(products);

            List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 1", Description1 = "Desc1", UPC = "UPC 1", PLUNumber = 1, StoreProducts = storeProducts.Where(x => x.Id < 2).ToList() });
            scaleProducts.Add(new ScaleProduct() { Name = "Scale Product 2", Description1 = "Desc2", UPC = "UPC 2", PLUNumber = 2, StoreProducts = storeProducts.Where(x => x.Id >= 2).ToList() });
            await _repository.CreateRangeAsync(scaleProducts);

            var template = new CutListTemplate() { Name = "Template 1", ScaleProducts = scaleProducts };
            var temp = await _repository.CreateAsync(template);

            var animal = new Animal()
            {
                Tag = "GetScaleProductsOfTemplate_NormalCall_ResultOk",
                StoreId = str.Id
            };

            var animalId = await _repositoryAnimal.CreateAsync(animal);

            var cutList = new CutList()
            {
                AnimalId = animalId,
                CutListTemplateId = temp.Id,
            };
            var cut = await _repository.CreateAsync(cutList);

            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                await _service.GetScaleProductsOfTemplate(0, animalId);
            });           

            // Assert
            Assert.Equal("Parameters out of bounds", exception.Message);
        }        
    }
}