using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface ISynchroRepository : IOperationBase<long, string, Synchro>
    {
        Task<long> GetOpenSynchroForStore(long storeId, CancellationToken cancellationToken = default);
    }
}