using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using System;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class InventorySearchModel:BaseSearchModel, IMapFrom<StoreProduct>
    {
        public long DepartmentId { get; set; }
        public long CategoryId { get; set; }
        public long BinLocationId { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}