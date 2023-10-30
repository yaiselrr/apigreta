using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class CategoryRepository : OperationBase<long, string, Category>, ICategoryRepository
    {
        public CategoryRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<Category> GetByCategory(int categoryId, bool track = true)
        {
            return track
                ? await Context.Set<Category>().FirstOrDefaultAsync(e => e.CategoryId == categoryId)
                : await Context.Set<Category>().AsNoTracking().FirstOrDefaultAsync(e => e.CategoryId == categoryId);
        }
    }
}