using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface DeleteStoreRequestContract : IRegisteredEventContract
    {
        public string BO_ClientCode { get; set; }
        public Guid GuidId { get; set; }
    }
}