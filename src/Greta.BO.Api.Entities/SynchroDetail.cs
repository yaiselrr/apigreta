using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class SynchroDetail : BaseEntityLong
    {
        public string Entity { get; set; }

        /// <summary>
        ///     Determine if this detail is for update, create or remove
        /// </summary>
        /// <value></value>
        public SynchroType Type { get; set; }

        /// <summary>
        ///     Json string with the data to sincro
        /// </summary>
        /// <value></value>
        public string Data { get; set; }

        public long SynchroId { get; set; }

        // public Synchro Synchro { get; set; }
    }
}