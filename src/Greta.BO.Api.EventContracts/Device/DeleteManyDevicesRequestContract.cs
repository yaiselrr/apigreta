using Greta.Sdk.MassTransit.Interfaces;
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.EventContracts.Device
{
    /// <summary>
    ///     Contract fro create user from BO
    /// </summary>
    public interface DeleteManyDevicesRequestContract : IRegisteredEventContract
    {
        public List<Guid> DevicesId { get; set; }
    }
}