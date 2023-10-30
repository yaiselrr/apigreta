using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.Api.Test.ExportImport
{
    public class EntityExtensionsTests
    {
        IServiceProvider BuildProvider()
        {
            var provider = new ServiceCollection()
                .AddScoped<IFakeService, FakeService>()
                .BuildServiceProvider();
            return provider;
        }

        [Fact]
        public void EntityExtensionsTests_IfNotMappedGetPropertyValue()
        {
            var source = new FakeEntity { Id = 1, Name = "entity 1" };
            var dest = new FakeEntity { Id = 2, Name = "" };
            var mapping = new Dictionary<string, string>();

            dest.IfNotMappedGetPropertyValue(mapping, nameof(FakeEntity.Name), source);
            Assert.Equal(source.Name, dest.Name);
        }

        [Fact]
        public void EntityExtensionsTests_GetOrAddFromCache()
        {
            var cache = new Dictionary<long, FakeEntity>();
            var service = BuildProvider().GetRequiredService<IFakeService>();
            var entity = cache.GetOrAddFromCache(1, async (id) => await service.Get(id), (hm) => {});

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public void EntityExtensionsTest_GetOrAddFromCacheNumericKeyAndValue()
        {
            var cache = new Dictionary<long, long>();
            var service = BuildProvider().GetRequiredService<IFakeService>();
            var entityId = cache.GetOrAddFromCache(1, async (id) => await service.Get(id), (hm) => { });
            Assert.NotNull(entityId);
            Assert.NotEqual(0, entityId);
        }
    }
}