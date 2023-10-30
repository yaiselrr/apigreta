using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Employee : BaseLocationEntityLong
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Store> Stores { get; set; }

    }
}