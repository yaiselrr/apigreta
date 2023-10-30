using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class AdBatchCreateModel : IDtoLong<string>, IMapFrom<AdBatch>
    {
        [Required] public string Name { get; set; }

        [Required] public DateTime StartTime { get; set; }

        public long StoreId { get; set; }

        public bool AllStores { get; set; }

        public long RegionId { get; set; }

        public StoreModel Store { get; set; }

        public virtual List<PriceBatchDetailModel> PriceBatchDetails { get; set; }


        [Required] public DateTime EndTime { get; set; }


        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}