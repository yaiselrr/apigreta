using System.Diagnostics;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Employee")]
    public class LiteEmployee : BaseEntityLong
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Pin { get; set; }
        public long? ProfileId { get; set; }



        public static LiteEmployee Convert(BOUser from)
        {
            Debug.Assert(@from.POSProfileId != null, "from.POSProfileId != null");
            return new LiteEmployee
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                
                FirstName = from.FirstName,
                LastName = from.LastName,
                UserName = from.UserName,
                Email = from.Email,
                Pin = from.Pin,
                ProfileId = from.POSProfileId
            };
        }
    }
}