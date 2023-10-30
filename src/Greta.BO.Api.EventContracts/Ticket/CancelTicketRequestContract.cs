using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Ticket
{
    /// <summary>
    /// Contract fro create user from BO
    /// </summary>
    public interface CancelTicketRequestContract : IRegisteredEventContract
    {      
        public Guid GuidId { get; set; }
        public string BO_ClientCode { get; set; }
    }
}