using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ExternalScale")]
    public class LiteExternalScale : BaseEntityLong
    {
        public string Ip { get; set; }

        public string Port { get; set; }
        
        public BoExternalScaleType ExternalScaleType { get; set; }
        public string Device { get; set; }

        public static LiteExternalScale Convert(ExternalScale from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Ip = from.Ip,
                Port = from.Port,
                ExternalScaleType = from.ExternalScaleType,
                Device = from.SyncDevice?.DeviceId
            };
        }
    }
}