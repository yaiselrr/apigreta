using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Report
{
    public interface DeleteReportRequestContract : IRegisteredEventContract
    {
        public Guid GuidId { get; set; }
        public string BO_ClientCode { get; set; }
    }
}