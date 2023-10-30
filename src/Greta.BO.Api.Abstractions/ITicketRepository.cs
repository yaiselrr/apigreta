
namespace Greta.BO.Api.Abstractions
{
    using Greta.BO.Api.Entities;
    using Greta.Sdk.EFCore.Operations;
    public interface ITicketRepository : IOperationBase<long, string, Ticket>
    {
    }
}