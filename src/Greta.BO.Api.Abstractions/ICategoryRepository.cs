using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface ICategoryRepository : IOperationBase<long, string, Category>
    {
        Task<Category> GetByCategory(int categoryId, bool track = true);
    }
}