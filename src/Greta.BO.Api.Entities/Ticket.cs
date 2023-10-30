using System;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Ticket : BaseEntityLong
    {
        public string Code { get; set; }
        public string Data { get; set; }
        public TicketType Type { get; set; }
        public TicketStatus Status { get; set; }
        public Guid GuidId { get; set; }
    }
}