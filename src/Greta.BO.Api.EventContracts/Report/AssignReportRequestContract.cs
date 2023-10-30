using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Report
{
    /// <summary>
    /// Contract fro create user from BO
    /// </summary>
    public interface AssignReportRequestContract : IRegisteredEventContract
    {
        public string BO_ClientCode { get; set; }
        public List<IAssignReportRequest> Reports { get; set; }
    }
    public interface BOApiAssignReportResponseContract
    {
        public List<long> Reports { get; set; }
    }

    public interface IAssignReportRequest
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public int Category { get; set; }
    }
}
