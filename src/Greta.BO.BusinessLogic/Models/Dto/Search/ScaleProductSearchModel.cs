using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleProductSearchModel : BaseSearchModel, IMapFrom<Product>
    {
        public string Upc { get; set; }
        public int Plu { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
    }
}