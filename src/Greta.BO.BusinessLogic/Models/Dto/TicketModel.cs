using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class TicketModel : IDtoLong<string>, IMapFrom<Ticket>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public TicketType Type { get; set; }
        public TicketStatus Status { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
