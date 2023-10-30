using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface CreateStoreRequestContract : IRegisteredEventContract
    {
        public string Name { get; set; }
        public string BO_ClientCode { get; set; }
        public Guid GuidId { get; set; }

        //is necesary more information???

    }
}