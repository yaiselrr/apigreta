using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IQtyHandTrackRepository : IOperationBase<long, string, QtyHandTrack>
    {
        Task<long> CreateQtyHandTrack(Product product, Store store, decimal oldQtyHand, decimal newQtyHand,
            CancellationToken cancellationToken = default);
    }
}