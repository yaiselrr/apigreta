using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IDepartmentRepository : IOperationBase<long, string, Department>
    {
        Task<Department> GetByDepartment(int departmentId, bool track = true);
    }
}