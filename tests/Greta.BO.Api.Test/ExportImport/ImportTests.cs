using CsvHelper;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeImport : BaseImport<FakeEntity>
    {
        public FakeImport(ILogger<FakeImport> logger, IServiceProvider provider,
            ILogger<Introspector<FakeEntity>> inLogger, INotifier notifier) 
            : base(logger, provider, inLogger, notifier)
        {

        }
        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var msg = HandlerMessage.New("Initializing");
            NotifyUpdate(msg);
        }
    }

    public class ImportTests
    {
        private INotifier _notifier;

        private FakeImport BuildImporter()
        {
            var provider = new ServiceCollection()
                .AddSingleton<INotifier, FakeNotifier>()
                .BuildServiceProvider();
            var logger = Mock.Of<ILogger<FakeImport>>(); 
            var inLogger =  Mock.Of<ILogger<Introspector<FakeEntity>>>();
            _notifier = provider.GetRequiredService<INotifier>();

            return new FakeImport(logger, provider, inLogger, _notifier);
        }

        private CsvReader csv
        {
            get {
                var reader = new StringReader(CsvContent());
                return new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture, true);
            }
        }

        private string CsvContent ()
        {
            return "Name,Id\nElement 1,1\nElement 2,2\nE;ement 3,3";
        }

        private Dictionary<string, string> mapping =>
            new Dictionary<string, string>
            {
                {"Name", "Name"},
                {"Id", "Idx"}
            };

        [Fact]
        public async Task ImportTests_instantiation()
        {
            var importer = BuildImporter();
            await importer.Process(mapping, csv, null);
            Assert.Equal(2, _notifier.Message.AllMessages.Count);
        }
    }
}
