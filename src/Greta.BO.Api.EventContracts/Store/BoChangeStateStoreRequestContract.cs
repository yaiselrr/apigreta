using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface BoChangeStateStoreRequestContract : IRegisteredEventContract
    {       
        public string BO_ClientCode { get; set; }
        public bool State { get; set; }
        public Guid GuidId { get; set; }

        //is necesary more information???

    }
}