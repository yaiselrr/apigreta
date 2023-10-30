using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.Core.Models.Pager;

namespace Greta.BO.BusinessLogic.Interfaces
{
    /// <summary>
    /// Describe a generic service for greta system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericBaseService<T> : IBaseService where T : IBase
    {
        /// <summary>
        /// Filter a list of entities
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> Get(Specification<T> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter a list of entities and return a result model
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<TResult>> Get<TResult>(Specification<T, TResult> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a single item or default
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Get(SingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a single item or default
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Get<TResult>(SingleResultSpecification<T, TResult> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter an d paginate the data and get one object of type <see cref="Pager{T}"/>
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="filter">Filter object</param>
        /// <param name="search">Search string for build the like object</param>
        /// <param name="sort"> Source string with the format [field-sort order]</param>
        /// <returns>Pager object <see cref="Pager{T}"/></returns>
        Task<Pager<T>>  Filter(int currentPage, int pageSize, T filter, string search, string sort);

        /// <summary>
        /// Filter an d paginate the data and get one object of type <see cref="Pager{T}"/>
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="spec">Filter specification</param>
        /// <param name="cancellationToken">Cancellation object</param>
        /// <returns>Pager object <see cref="Pager{T}"/></returns>
        Task<Pager<T>> FilterSpec(
            int currentPage,
            int pageSize,
            ISpecification<T> spec,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filter an d paginate the data and get one object of type <see cref="Pager{T}"/>
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="spec">Filter specification</param>
        /// <param name="cancellationToken">Cancellation object</param>
        /// <returns>Pager object <see cref="Pager{T}"/></returns>
        Task<Pager<TResult>> FilterSpec<TResult>(
            int currentPage,
            int pageSize,
            ISpecification<T,TResult> spec,
            CancellationToken cancellationToken = default)
            where TResult: class, IDtoLong<string>;

        /// <summary>
        ///     Change State for entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="state">new State</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        Task<bool> ChangeState(long id, bool state);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns></returns>
        Task<bool> Delete(long id);

        /// <summary>
        /// Delete an Range of entities
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteRange(List<long> ids);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Return an List <see cref="List{T}"/> of entities</returns>
        Task<List<T>> Get();

        /// <summary>
        /// Get the entity of type {T} by id <see cref="T"/>
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Return an entity</returns>
        Task<T> Get(long id);

        /// <summary>
        /// Create a entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Post(T entity);

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        Task<bool> Put(long id, T entity);
    }
}