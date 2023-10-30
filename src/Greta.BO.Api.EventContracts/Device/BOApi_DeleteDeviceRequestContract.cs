using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Device
{
    /// <summary>
    ///     Contract fro create user from BO
    /// </summary>
    public interface BOApi_DeleteDeviceRequestContract : IRegisteredEventContract
    {
        public Guid GuidId { get; set; }

    }
}