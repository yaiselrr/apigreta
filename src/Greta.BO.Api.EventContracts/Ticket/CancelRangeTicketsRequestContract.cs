
using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Ticket
{
    /// <summary>
    /// Contract fro create user from BO
    /// </summary>
    public interface CancelRangeTicketsRequestContract : IRegisteredEventContract
    {      
        public List<Guid> TicketGuidIds { get; set; }
        public string BO_ClientCode { get; set; }
    }
}