using System.Collections.Generic;
using Greta.BO.Api.Entities.Lite;
using MediatR;

namespace Greta.BO.Api.Entities.Events.Internal.Products;

public record ProductCreated(LiteProduct Product, List<long> Stores, long StoreProductReferenceId) : INotification;
public record ProductDeleted(LiteProduct Product, List<long> Stores, long StoreProductReferenceId) : INotification;
public record ProductUpdated(LiteProduct Product, List<long> Stores, long OldDepartment, long OldCategory, long StoreProductReferenceId) : INotification;
public record ProductChangeState(LiteProduct Product, List<long> Stores, long StoreProductReferenceId, bool State) : INotification;