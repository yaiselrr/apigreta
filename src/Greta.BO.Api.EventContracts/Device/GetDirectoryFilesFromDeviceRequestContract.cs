using Greta.Sdk.MassTransit.Interfaces;

namespace Greta.BO.Api.EventContracts.Device
{
    // ReSharper disable once InconsistentNaming
    public interface GetDirectoryFilesFromDeviceRequestContract : IRegisteredEventContract
    {
        public string DeviceGuid { get; set; }
        public string Path { get; set; }
    }
}