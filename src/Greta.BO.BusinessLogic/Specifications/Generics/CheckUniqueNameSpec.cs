using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.BusinessLogic.Specifications.Generics;

/// <summary>
/// Describe the specification for check if exist one entity with the current name excluing or not one expecific id
/// </summary>
/// <typeparam name="TNameUniqueEntity"></typeparam>
public sealed class CheckUniqueNameSpec<TNameUniqueEntity>: SingleResultSpecification<TNameUniqueEntity>
    where TNameUniqueEntity: BaseEntityLong, INameUniqueEntity
{
    /// <summary>
    /// Check unique name
    /// </summary>
    /// <param name="name">name of the entity</param>
    /// <param name="id">optional id for exclude</param>
    public CheckUniqueNameSpec(string name, long id = -1)
    {
        Query.Where(x => x.Name == name);
        if (id != -1)
        {
            Query.Where(x => x.Id != id);
        }
    }
}