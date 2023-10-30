using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class ExternalJob: BaseEntityLong
    {
        public ExternalJobType Type { get; set; }
        public ExternalJobStatus Status { get; set; }
        public string Data { get; set; }
        /// <summary>
        /// Store the count of retry every time we change the device we reset the count max by device 3????????
        /// </summary>
        public int FailRetry { get; set; }

        public string  Messages { get; set; }
        public string RawData { get; set; }
    }
}