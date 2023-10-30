using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class DepartmentRepository : OperationBase<long, string, Department>, IDepartmentRepository
    {
        public DepartmentRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }


        public async Task<Department> GetByDepartment(int departmentId, bool track = true)
        {
            return track
                ? await Context.Set<Department>().FirstOrDefaultAsync(e => e.DepartmentId == departmentId)
                : await Context.Set<Department>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.DepartmentId == departmentId);
        }
    }
}