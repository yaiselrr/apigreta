using System.Collections.Generic;
using Greta.BO.Api.Entities.Lite;
using MediatR;

namespace Greta.BO.Api.Entities.Events.Internal.OnlineStores;

public record OnlineStoreCreated(long Id, string Token,  bool IsImport = false):INotification;
public record OnlineStoreDeleted(List<long> Ids) : INotification;