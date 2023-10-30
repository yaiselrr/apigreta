using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class PriceBatchDetailModel : IDtoLong<string>, IMapFrom<PriceBatchDetail>
    {
        public decimal Price { get; set; }
        public long? ProductId { get; set; }
        public ProductModel Product { get; set; }

        public long RegionId { get; set; }
        public bool AllStores { get; set; }

        public long? FamilyId { get; set; }
        public FamilyModel Family { get; set; }
        
        public long? CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public long HeaderId { get; set; }
        public IFormFile Csv { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}