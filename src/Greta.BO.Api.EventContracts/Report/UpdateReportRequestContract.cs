using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Report
{
    /// <summary>
    /// Contract fro create user from BO
    /// </summary>
    public interface UpdateReportRequestContract : IRegisteredEventContract
    {
        public Guid GuidId { get; set; }
        public string Name { get; set; }        
        public bool State { get; set; }
        public string BO_ClientCode { get; set; }
        public int Category { get; set; }
    }
}