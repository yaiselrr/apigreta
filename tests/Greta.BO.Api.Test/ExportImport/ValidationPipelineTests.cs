using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Moq;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeValidationNameIsValid : ImportValidation<FakeEntity>
    {
        public override async Task<(object, bool)> Validate(FakeEntity parameter, 
            Func<FakeEntity, Task<(object, bool)>> next, params object[] args)
        {
            (object?, bool) res = (null, false);
            if (string.IsNullOrEmpty(parameter.Name))
                return await Task.FromResult(res);
            return await next(parameter);
        }
    }

    public class FakeValidationIsIsNotZero : ImportValidation<FakeEntity>
    {
        public override async Task<(object?, bool)> Validate(FakeEntity parameter, 
            Func<FakeEntity, Task<(object?, bool)>> next, params object[] args)
        {
            (object?, bool) res = (null, false);
            if (parameter.Id <= 0)
                return await Task.FromResult(res);
            return await next(parameter);
        }
    }

    public class ValidationPipelineTests
    {
        // [Fact]
        // public void ValidationPipelineTests_register_validation()
        // {
        //     var notifier = Mock.Of<INotifier>();
        //     var pipeline = new ValidationPipeline<FakeEntity>(notifier);
        //     pipeline
        //         .Add<FakeValidationNameIsValid>()
        //         .Add<FakeValidationIsIsNotZero>();
        // }

        [Fact]
        public async Task ValidationPipelineTests_valid_name()
        {
            var notifier = Mock.Of<INotifier>();
            var entity = new FakeEntity { Id = 1, Name = "entity 1"};
            var pipeline = new ValidationPipeline<FakeEntity>(notifier);
            pipeline
                .Add<FakeValidationNameIsValid>();
            var result = await pipeline.Execute(entity);
            Assert.True(result.IsValid);
        }
        
        [Fact]
        public async Task ValidationPipelineTests_valid_name_and_id()
        {
            var notifier = Mock.Of<INotifier>();
            var entity = new FakeEntity { Id = 1, Name = "entity 1"};
            var pipeline = new ValidationPipeline<FakeEntity>(notifier);
            pipeline
                .Add<FakeValidationNameIsValid>()
                .Add<FakeValidationIsIsNotZero>();
            var result = await pipeline.Execute(entity);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidationPipelineTests_invalid_name()
        {
            var notifier = Mock.Of<INotifier>();
            var entity = new FakeEntity { Id = 1, Name = null };
            var pipeline = new ValidationPipeline<FakeEntity>(notifier);
            pipeline
                .Add<FakeValidationNameIsValid>();
            var result = await pipeline.Execute(entity);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task ValidationPipelineTests_valid_name_and_invalid_id()
        {
            var notifier = Mock.Of<INotifier>();
            var entity = new FakeEntity { Id = 0, Name = "entity 1"};
            var pipeline = new ValidationPipeline<FakeEntity>(notifier);
            pipeline
                .Add<FakeValidationNameIsValid>()
                .Add<FakeValidationIsIsNotZero>();
            var result = await pipeline.Execute(entity);
            Assert.False(result.IsValid);
        }
    }
}
