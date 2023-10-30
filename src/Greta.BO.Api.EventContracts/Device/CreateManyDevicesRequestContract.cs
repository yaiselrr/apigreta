using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Device
{
    /// <summary>
    ///     Contract fro create user from BO
    /// </summary>
    public interface CreateManyDevicesRequestContract : IRegisteredEventContract
    {
        public Guid StoreGuidId { get; set; }
        //set string for the name and guid for the GuidId 
        public Dictionary<string, Guid> Devices { get; set; }
    }
}