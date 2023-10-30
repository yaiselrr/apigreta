using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IProductRepository : IOperationBase<long, string, Product>
    {
        Task<Product> GetByUPC(string upc, bool track = true);
    }
}