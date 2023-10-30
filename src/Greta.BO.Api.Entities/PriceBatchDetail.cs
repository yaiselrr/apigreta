using Greta.Sdk.EFCore.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Greta.BO.Api.Entities
{
    public class PriceBatchDetail : BaseEntityLong, IFullSyncronizable
    {
        public decimal Price { get; set; }
        public long? ProductId { get; set; }
        public Product Product { get; set; }

        public long? FamilyId { get; set; }
        public Family Family { get; set; }
        
        public long? CategoryId { get; set; }
        public Category Category { get; set; }

        public long HeaderId { get; set; }

        public Batch Header { get; set; }
    }
}