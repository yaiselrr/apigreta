using Greta.BO.Api.Entities.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.WebRequestMethods;

namespace Greta.BO.Api.Entities.Lite
{
    public class LiteScaleReasonCodes : BaseEntityLong
    {
        public string Name { get; set; }
        //public long DepartmentId { get; set; }
        public ScaleReasonCodesType Type { get; set; }

        public static LiteScaleReasonCodes Convert(ScaleReasonCodes from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                

                Name = from.Name,
                Type = from.Type
                //DepartmentId = from.DepartmentId
            };
        }
    }
}
