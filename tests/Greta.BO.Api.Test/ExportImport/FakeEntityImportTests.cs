using CsvHelper;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeEntityImport : BaseImport<FakeEntity>
    {
        public FakeEntityImport(ILogger<FakeEntityImport> logger, IServiceProvider provider, 
            ILogger<Introspector<FakeEntity>> inLogger, INotifier notifier) 
            : base(logger, provider, inLogger, notifier)
        {
        }

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {

        }
    }

    public class FakeEntityImportTests
    {
        private readonly IServiceProvider _provider;

        public FakeEntityImportTests(IServiceProvider provider)
        {
            _provider = new ServiceCollection()
                .AddSingleton<INotifier, FakeNotifier>()
                .BuildServiceProvider();;
        }
    }
}
