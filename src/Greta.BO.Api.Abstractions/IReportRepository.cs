using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;
using System;
using System.Threading.Tasks;

namespace Greta.BO.Api.Abstractions
{
    public interface IReportRepository : IOperationBase<long, string, Report>
    {
        Task<bool> Delete(Guid guid);
        Task<bool> ExistReport(string name);
        Task<Report> GetByGuid(Guid guidId);
    }
}