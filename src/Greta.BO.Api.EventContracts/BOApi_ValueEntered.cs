using System;

namespace Greta.BO.Api.MassTransit.EventContracts
{
    public interface BOApi_ValueEntered //: IRegisteredEventContract
    {
        string Value { get; }
    }

    public interface ValueEnteredResult
    {
        string Value2 { get; }
        DateTime Timestamp { get; }
    }
}