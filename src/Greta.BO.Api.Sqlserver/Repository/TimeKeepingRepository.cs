using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Sqlserver.Repository;

public class TimeKeepingRepository: OperationBase<long, string, TimeKeeping>, ITimeKeepingRepository
{
    public TimeKeepingRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
        : base(authenticatetUser, context)
    {
    }
}