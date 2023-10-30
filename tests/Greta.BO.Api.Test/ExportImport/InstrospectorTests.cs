using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.Api.Test.ExportImport
{
    public class InstrospectorTests
    {
        private readonly INotifier _notifier;

        public InstrospectorTests()
        {
            _notifier = new ServiceCollection()
                .AddSingleton<INotifier, FakeNotifier>()
                .BuildServiceProvider()
                .GetRequiredService<INotifier>();
        }

        [Fact]
        public void InstrospectorTests_get_base_properties()
        {
            var entity = new FakeEntity();
            var inst = Introspector<FakeEntity>.New(_notifier);

            var list = inst.GetBaseProperties();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void InstrospectorTests_get_all_base_properties()
        {
            var entity = new FakeEntity();
            var inst = Introspector<FakeEntity>.New(_notifier);

            var list = inst.GetAllBaseProperties();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void InstrospectorTests_get_column_names()
        {
            var entity = new FakeEntity();
            var inst = Introspector<FakeEntity>.New(_notifier);

            var list = inst.GetColumnName();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void Instrospector_get_all_column_names()
        {
            var entity = new FakeEntity();
            var inst = Introspector<FakeEntity>.New(_notifier);

            var list = inst.GetAllColumnsNames();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void InstrospectorTests_get_base_properties_notified()
        {
            var entity = new FakeEntity { Id = 1, Name = "entity1"};
            var inst = new Introspector<FakeEntity>(_notifier);

            var list = inst.GetBaseProperties();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void InstrospectorTests_get_base_properties_notified_fluent_api()
        {
            var entity = new FakeEntity { Id = 1, Name = "etity1"};
            var list = Introspector<FakeEntity>.New(_notifier)
                            .GetBaseProperties();
            
            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }
    }
}
