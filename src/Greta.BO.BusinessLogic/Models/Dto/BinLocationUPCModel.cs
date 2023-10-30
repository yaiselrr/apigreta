

using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class BinLocationUPCModel
    {
        public long StoreId { get; set; }
        public long BinLocationId { get; set; }
        public bool IgnoreCurrents { get; set; }
        public List<string> UPCs { get; set; }
    }
}
