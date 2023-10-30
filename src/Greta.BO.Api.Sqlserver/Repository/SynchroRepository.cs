using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class SynchroRepository : OperationBase<long, string, Synchro>, ISynchroRepository
    {
        public SynchroRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        /// <summary>
        /// Return a open sync
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetOpenSynchroForStore(long storeId, CancellationToken cancellationToken = default)
        {
            var current = await GetEntity<Synchro>()
                .Where(x => x.StoreId == storeId && x.Status == SynchroStatus.OPEN)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (current == 0)
            {
                var newSynchro = new Synchro
                {
                    Tag = 1 + await GetEntity<Synchro>()
                        .Where(x => x.StoreId == storeId)
                        .OrderBy(x => x.Tag)
                        .Select(x => x.Tag)
                        .LastOrDefaultAsync(cancellationToken: cancellationToken),
                    // Tag = 1 + await GetEntity<Store>()
                    //     .Where(x => x.Id == storeId)
                    //     .Select(x => x.LastBackupVersion)
                    //     .FirstOrDefaultAsync(cancellationToken: cancellationToken),
                    StoreId = storeId,
                    Status = SynchroStatus.OPEN
                };
                current = await base.CreateAsync(newSynchro, cancellationToken);
            }

            return current;
        }
    }
}