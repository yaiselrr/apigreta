using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Sqlserver.Repository;

public class CutListRepository : OperationBase<long, string, CutList>, ICutListRepository
{
    public CutListRepository(IAuthenticateUser<string> authenticateUser, SqlServerContext context)
        : base(authenticateUser, context)
    {
    }
}