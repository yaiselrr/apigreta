using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class BOUserSearchModel : BaseSearchModel, IMapFrom<BOUser>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}