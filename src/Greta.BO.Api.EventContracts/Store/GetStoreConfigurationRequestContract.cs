using System;
using Greta.Sdk.MassTransit.Interfaces;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface GetStoreConfigurationRequestContract: IRegisteredEventContract
    {
        public Guid StoreGuid { get; set; }
    }
}