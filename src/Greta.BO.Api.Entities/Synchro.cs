using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class Synchro : BaseEntityLong
    {
        public long Tag { get; set; }

        public long StoreId { get; set; }

        // public Store Store { get; set; }

        public SynchroStatus Status { get; set; }

        public List<SynchroDetail> SynchroDetails { get; set; }

        public string FilePath { get; set; }
    }
}