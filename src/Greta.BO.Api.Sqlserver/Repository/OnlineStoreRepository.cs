using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Sqlserver.Repository;

public class OnlineStoreRepository : OperationBase<long, string, OnlineStore>, IOnlineStoreRepository
{
    public OnlineStoreRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
        : base(authenticatetUser, context)
    {
    }
}

public class OnlineCategoryRepository : OperationBase<long, string, OnlineCategory>, IOnlineCategoryRepository
{
    public OnlineCategoryRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
        : base(authenticatetUser, context)
    {
    }
}

public class OnlineProductRepository : OperationBase<long, string, OnlineProduct>, IOnlineProductRepository
{
    public OnlineProductRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
        : base(authenticatetUser, context)
    {
    }
}