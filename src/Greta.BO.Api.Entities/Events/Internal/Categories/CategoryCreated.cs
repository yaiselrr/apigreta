using System.Collections.Generic;
using Greta.BO.Api.Entities.Lite;
using MediatR;

namespace Greta.BO.Api.Entities.Events.Internal.Categories;

public record CategoryCreated(LiteCategory Category):INotification;
public record CategoryUpdated(LiteCategory Category, long OldDepartment, List<Product> Products) : INotification;
public record CategoryUpd(LiteCategory Category, long OldDepartment):INotification;
public record CategoryDeleted(List<long> Ids, List<long> IdsToRemove) : INotification;
// public record CategoryDeleted : INotification
// {
//     public List<long> Ids { get; }
//     public List<long> IdsToRemove { get; }

//     public CategoryDeleted(List<long> ids, List<long> idsToRemove)
//     {
//         Ids = ids;
//         IdsToRemove = idsToRemove;
//     }
// }