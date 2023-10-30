using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Sqlserver.Repository;

public class CutListDetailRepository : OperationBase<long, string, CutListDetail>, ICutListDetailRepository
{
    public CutListDetailRepository(IAuthenticateUser<string> authenticateUser, SqlServerContext context)
        : base(authenticateUser, context)
    {
    }
}