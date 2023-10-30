using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Report
{
    public interface DeleteReportRangeRequestContract : IRegisteredEventContract
    {
        public List<Guid> GuidIds { get; set; }
        public string BO_ClientCode { get; set; }
    }
}