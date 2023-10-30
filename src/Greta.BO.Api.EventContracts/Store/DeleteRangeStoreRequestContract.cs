using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface DeleteRangeStoreRequestContract : IRegisteredEventContract
    {
        public string BO_ClientCode { get; set; }
        public List<Guid> StoresguidId { get; set; }
    }
}