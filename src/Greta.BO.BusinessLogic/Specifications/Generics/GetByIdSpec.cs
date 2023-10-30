using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.Generics;

/// <summary>
/// This specification is to filter values by id and throws an exception if more than one result is founded
/// </summary>
/// <typeparam name="TEntity">This type must inherit <see cref="BaseEntityLong"/> </typeparam>
public sealed class GetByIdSpec<TEntity> : SingleResultSpecification<TEntity> where TEntity : BaseEntityLong
{
    /// <inheritdoc />
    public GetByIdSpec(long id)
    {
        Query.Where(x => x.Id == id);
    }
}