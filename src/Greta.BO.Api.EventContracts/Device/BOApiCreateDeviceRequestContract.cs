using Greta.Sdk.MassTransit.Interfaces;
using System;

namespace Greta.BO.Api.EventContracts.Device
{
    /// <summary>
    ///     Contract from create user from BO
    /// </summary>
    public interface BOApiCreateDeviceRequestContract : IRegisteredEventContract
    {
        public string Name { get; set; }
        public string LicenseCode { get; set; }
        public Guid GuidId { get; set; }
        public Guid StoreGuidId { get; set; }
    }
}