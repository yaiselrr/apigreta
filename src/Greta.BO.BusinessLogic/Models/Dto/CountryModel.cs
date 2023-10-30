using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CountryModel : IMapFrom<Country>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}