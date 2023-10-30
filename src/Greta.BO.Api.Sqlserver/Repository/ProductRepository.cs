using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class ProductRepository : OperationBase<long, string, Product>, IProductRepository
    {
        public ProductRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<Product> GetByUPC(string upc, bool track = true)
        {
            return track
                ? await Context.Set<Product>().Include(e => e.StoreProducts).FirstOrDefaultAsync(e => e.UPC == upc)
                : await Context.Set<Product>().Include(e => e.StoreProducts).AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UPC == upc);
        }
    }
}