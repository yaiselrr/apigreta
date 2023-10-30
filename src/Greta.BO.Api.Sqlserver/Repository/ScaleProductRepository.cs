using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class ScaleProductRepository : OperationBase<long, string, ScaleProduct>, IScaleProductRepository
    {
        public ScaleProductRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<ScaleProduct> GetByUPC(string upc, bool track = true)
        {
            return track
                ? await Context.Set<ScaleProduct>().Include(e => e.StoreProducts).Include(x => x.ScaleLabelDefinitions)
                    .FirstOrDefaultAsync(e => e.UPC == upc)
                : await Context.Set<ScaleProduct>()
                    .Include(e => e.StoreProducts)
                    .Include(x => x.ScaleLabelDefinitions)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UPC == upc);
        }
    }
}