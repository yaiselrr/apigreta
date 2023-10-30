using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IScaleCategoryRepository : IOperationBase<long, string, ScaleCategory>
    {
        Task<ScaleCategory> GetByCategory(int categoryId, bool track = true);
    }
}