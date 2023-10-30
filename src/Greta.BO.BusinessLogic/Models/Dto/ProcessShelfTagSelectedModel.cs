using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProcessShelfTagSelectedModel
    {
        public long StoreId { get; set; }
        public long TagId { get; set; }
        public List<long> ShelfTagIds { get; set; }
    }
}