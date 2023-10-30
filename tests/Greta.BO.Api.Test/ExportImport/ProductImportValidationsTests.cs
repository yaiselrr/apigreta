using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations;
using Greta.Sdk.Core.Abstractions;
using Moq;

namespace Greta.BO.Api.Test.ExportImport
{
    public class ProductImportValidationsTests
    {
        [Fact]
        public void pipeline_initialization()
        {
            var notifier = Mock.Of<INotifier>();
            var pipeline = new ValidationPipeline<Product>(notifier);
            
            Assert.NotNull(pipeline);
            Assert.Equal(0, pipeline.Validations.Count);
        }

        [Fact]
        public void pipeline_valid_name()
        {
            var pipeline = GetPipeline<Product>();
            pipeline.Add<ProductValidationName>();
            var res = pipeline.Execute(FakeProductData.ValidProduct).Result;
            Assert.True(res.IsValid);
        }

        [Fact]
        public void pipeline_invalid_name()
        {
            var pipeline = GetPipeline<Product>();
            pipeline.Add<ProductValidationName>();
            var res = pipeline.Execute(FakeProductData.InValidProductEmptyName).Result;
            Assert.False(res.IsValid);
        }

        [Fact]
        public void pipeline_valid_name_departmentId()
        {
            var pipeline = GetPipeline<Product>();
            pipeline.Add<ProductValidationName>().Add<ProductValidationDepartmentId>();
            var res = pipeline.Execute(FakeProductData.ValidProduct).Result;
            Assert.True(res.IsValid);
        }

        [Fact]
        public void pipeline_valid_all_validations()
        {
            var pipeline = GetPipeline<Product>();
            pipeline
                .Add<ProductValidationName>()
                .Add<ProductValidationCategoryId>()
                .Add<ProductValidationDepartmentId>()
                .Add<ProductValidationUPC>()
                .Add<ProductValidationUPCLength>();
            var res = pipeline.Execute(FakeProductData.ValidProduct).Result;
            Assert.True(res.IsValid);
        }

        [Fact]
        public void pipeline_valid_all_validations_with_invalid_name()
        {
            var pipeline = GetPipeline<Product>();
            pipeline
                .Add<ProductValidationName>()
                .Add<ProductValidationCategoryId>()
                .Add<ProductValidationDepartmentId>()
                .Add<ProductValidationUPC>()
                .Add<ProductValidationUPCLength>();
            var res = pipeline.Execute(FakeProductData.InValidProductEmptyName).Result;
            Assert.False(res.IsValid);
        }

        private IValidationPipeline<T> GetPipeline<T>() where T : class, IBase
        {
            var notifier = Mock.Of<INotifier>();
            return new ValidationPipeline<T>(notifier);
        }
    }
}