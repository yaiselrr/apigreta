using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Hubs
{
    public class NotifyWorkerStatus
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
        public object Data { get; set; }
    }
}