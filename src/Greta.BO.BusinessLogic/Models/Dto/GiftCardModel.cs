using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class GiftCardModel : IMapFrom<GiftCard>
    {
        public string Number { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
        
        public long StoreId { get; set; }
        
        public GiftCardType GiftCardType  { get; set; }
    }
}
