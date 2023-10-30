using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class ReportRepository : OperationBase<long, string, Report>, IReportRepository
    {
        public ReportRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<bool> Delete(Guid guid)
        {
            var report = await this.Context.Set<Report>().FirstOrDefaultAsync(x => x.GuidId == guid);
            var result = this.Context.Remove(report);
            await this.Context.SaveChangesAsync();
            return result != null;
        }

        public async Task<bool> ExistReport(string name)
        {
            return await this.Context.Set<Report>().AnyAsync(x => x.Name == name);
        }

        public async Task<Report> GetByGuid(Guid guidId)
        {
           return await this.Context.Set<Report>().FirstOrDefaultAsync(x => x.GuidId == guidId);
        }
    }
}
