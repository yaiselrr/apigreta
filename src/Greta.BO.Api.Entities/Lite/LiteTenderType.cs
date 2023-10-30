using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("TenderType")]
    public class LiteTenderType: BaseEntityLong
    {
        public string Name { get; set; }
        public bool OpenDrawer { get; set; }
        public string DisplayAs { get; set; }
        public bool CashDiscount { get; set; }
        
        public static LiteTenderType Convert(TenderType from)
        {
            return new LiteTenderType
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name =  from.Name,
                OpenDrawer =  from.OpenDrawer,
                DisplayAs =  from.DisplayAs,
                CashDiscount =  from.CashDiscount
            };
        }
    }
}