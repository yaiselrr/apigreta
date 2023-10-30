using System;

namespace Greta.BO.Api.EventContracts.Ticket
{
    /// <summary>
    /// Contract fro create user from BO
    /// </summary>
    public interface CompleteTicketCreateStoreResponseContract 
    {      
        public Guid GuidId { get; set; }
        
        public string Data { get; set; }
    }
}