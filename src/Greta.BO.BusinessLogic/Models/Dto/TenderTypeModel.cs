using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class TenderTypeModel : IDtoLong<string>, IMapFrom<TenderType>
    {
        public string Name { get; set; }
        public string DisplayAs { get; set; }
        public bool OpenDrawer { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }
        public bool CashDiscount { get; set; }
        public bool PaymentGateway { get; set; }
    }
}