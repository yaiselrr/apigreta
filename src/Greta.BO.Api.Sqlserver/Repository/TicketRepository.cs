 namespace Greta.BO.Api.Sqlserver.Repository
{
    using Greta.BO.Api.Entities;
    using Greta.Sdk.EFCore.Middleware;
    using Greta.Sdk.EFCore.Operations;
    using Greta.BO.Api.Abstractions;
    public class TicketRepository : OperationBase<long, string, Ticket>, ITicketRepository
    {
        public TicketRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }
    }
}