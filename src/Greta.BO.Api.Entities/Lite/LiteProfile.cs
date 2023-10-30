using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Profile")]
    public class LiteProfile: BaseEntityLong
    {
        public string Name { get; set; }

        [SqlFkTable("ProfilePermission", "ProfilesId")]
        [SqlFkColumn("PermissionsId")]
        public List<long> Permissions { get; set; }

        public static LiteProfile Convert(Profiles from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                Permissions = from.Permissions == null ? new List<long>() : from.Permissions.Select(x => x.Id).ToList()
            };
        }
    }
}