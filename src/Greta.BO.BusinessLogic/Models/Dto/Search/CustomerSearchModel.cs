using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class CustomerSearchModel : BaseSearchModel, IMapFrom<Customer>
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}