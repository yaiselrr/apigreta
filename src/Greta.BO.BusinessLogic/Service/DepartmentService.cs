using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.ExternalScale.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for department entity
/// </summary>
public interface IDepartmentService : IGenericBaseService<Department>
{
    /// <summary>
    ///     Get entity by departmentId
    /// </summary>
    /// <param name="departmentId">department Id</param>
    /// <param name="id">Id</param>
    /// <returns>Category</returns>
    Task<Department> GetByDepartmentId(int departmentId, long id = -1);

    /// <summary>
    ///     Get entity by perishable
    /// </summary>
    /// <param name="perishable">Bool perishable</param>
    /// <returns>List of Perishables departments</returns>
    Task<List<Department>> Get(bool perishable);

    /// <summary>
    ///     get a list of departments by department id
    /// </summary>
    /// <param name="departmentId">department id</param>
    /// <param name="track">department id</param>
    /// <returns>department</returns>
    Task<Department> GetByDepartment(int departmentId, bool track = true);

    /// <summary>
    ///     Get Id by departmentId
    /// </summary>
    /// <param name="departmentId">Department Id</param>
    /// <returns>Id</returns>
    Task<long> GetIdFromDepartmentId(int departmentId);

    /// <summary>
    /// Determine if this depoartment entity can be deleted
    /// </summary>
    /// <param name="id">Department Id</param>
    /// <returns>Return true if this department dont have any element associated</returns>
    Task<bool> CanDeleted(long id);

    /// <summary>
    /// Determine if this category entity can be deleted of list
    /// </summary>
    /// <param name="ids">Department list Id</param>
    /// <returns>Return true if this department dont have any element associated</returns>
    Task<bool> CanDeleted(List<long> ids);

    /// <summary>
    ///     Updates departments for scales
    /// </summary>
    /// <param name="last">last date</param>
    /// <param name="deps">department list</param>
    /// <returns>department</returns>
    Task<List<DepartmentModel>> GetUpdatesForScales(DateTime last, List<long> deps);

    /// <summary>
    ///     Get departments for scales
    /// </summary>
    /// <param name="deps">department list</param>
    /// <returns>department</returns>
    Task<List<DepartmentModel>> GetAllForScales(List<long> deps);

    // /// <summary>
    // ///     Filter and sort list of entities
    // /// </summary>
    // /// <param name="currentPage">Current page</param>
    // /// <param name="pageSize">Page size</param>
    // /// <param name="filter">Filter params</param>
    // /// <param name="searchstring">basic searc string</param>
    // /// <param name="sortstring">sort string </param>
    // /// <returns></returns>
    // Task<Pager<Department>> FilterDepartment(int currentPage, int pageSize, DepartmentSearchModel filter, string searchstring,
    //         string sortstring);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IDepartmentService" />
public class DepartmentService : BaseService<IDepartmentRepository, Department>, IDepartmentService
{
    /// <inheritdoc />
    public DepartmentService(IDepartmentRepository departmentRepository,
        ILogger<DepartmentService> logger, ISynchroService synchroService)
        : base(departmentRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(Department from) =>
        (LiteDepartment.Convert(from));

    /// <inheritdoc />
    public async Task<Department> GetByDepartmentId(int depId, long id = -1)
    {
        if (id == -1)
            return await _repository.GetEntity<Department>().Where(x => x.DepartmentId == depId)
                .FirstOrDefaultAsync();
        return await _repository.GetEntity<Department>().Where(x => x.DepartmentId == depId && x.Id != id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<List<Department>> Get(bool perishable)
    {
        return await _repository.GetEntity<Department>()
            .Where(x => x.Perishable == perishable)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<DepartmentModel>> GetUpdatesForScales(DateTime last, List<long> deps)
    {
        if (deps == null || deps.Count == 0)
        {
            return await _repository.GetEntity<Department>()
                .Where(x =>
                    x.Perishable && x.State &&
                    (
                        x.CreatedAt > last ||
                        x.UpdatedAt > last
                    ))
                .Select(x => new DepartmentModel()
                {
                    Id = x.DepartmentId,
                    Name = x.Name
                })
                .ToListAsync();
        }
        else
        {
            return await _repository.GetEntity<Department>()
                .Where(x =>
                    x.Perishable && x.State &&
                    (
                        x.CreatedAt > last ||
                        x.UpdatedAt > last
                    ) && deps.Any(d => d == x.Id))
                .Select(x => new DepartmentModel()
                {
                    Id = x.DepartmentId,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }

    /// <inheritdoc />
    public async Task<List<DepartmentModel>> GetAllForScales(List<long> deps)
    {
        if (deps == null || deps.Count == 0)
        {
            return await _repository.GetEntity<Department>()
                .Where(x => x.Perishable && x.State)
                .Select(x => new DepartmentModel()
                {
                    Id = x.DepartmentId,
                    Name = x.Name
                })
                .ToListAsync();
        }
        else
        {
            return await _repository.GetEntity<Department>()
                .Where(x => x.Perishable && x.State && deps.Any(d => d == x.Id))
                .Select(x => new DepartmentModel()
                {
                    Id = x.DepartmentId,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }

    /// <inheritdoc />
    public async Task<Department> GetByDepartment(int departmentId, bool track = true)
    {
        return await _repository.GetByDepartment(departmentId, track);
    }

    /// <inheritdoc />
    public async Task<long> GetIdFromDepartmentId(int departmentId)
    {
        return await _repository.GetEntity<Department>()
            .Where(x => x.DepartmentId == departmentId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    // /// <inheritdoc />
    // public async Task<Pager<Department>> FilterDepartment(int currentPage, int pageSize, DepartmentSearchModel filter, string searchstring, string sortstring)
    // {
    //     if (currentPage < 1 || pageSize < 1)
    //     {
    //         _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
    //         throw new BusinessLogicException("Page parameter out of bounds.");
    //     }

    //     var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

    //     var query = _repository.GetEntity<Department>().IgnoreAutoIncludes();

    //     if (!string.IsNullOrEmpty(searchstring))
    //         query = query.Where(c => c.Name.Contains(searchstring) || (c.DepartmentId).ToString().Contains(searchstring));
    //     else
    //         query = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
    //             .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId);

    //     query = query
    //         .Switch(splited)
    //         .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
    //         .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
    //         .OrderByCase(e => e[0] == "perishable" && e[1] == "asc", e => e.Perishable)
    //         .OrderByDescendingCase(e => e[0] == "perishable" && e[1] == "desc", e => e.Perishable)
    //         .OrderByCase(e => e[0] == "departmentId" && e[1] == "asc", e => e.DepartmentId)
    //         .OrderByDescendingCase(e => e[0] == "departmentId" && e[1] == "desc", e => e.DepartmentId)
    //         .OrderByDefault(e => e.DepartmentId);

    //     var entities = await query.ToPageAsync(currentPage, pageSize);

    //     return entities;
    // }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(long id)
    {
        return await _repository.GetEntity<Department>()
            .Include(e => e.Categories)
            .Include(e => e.Products)
            .AnyAsync(e => e.Id == id && !e.Products.Any() && !e.Categories.Any());
    }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(List<long> ids)
    {
        foreach (var id in ids)
            if (!await CanDeleted(id))
                return false;
        return true;
    }

    /// <summary>
    ///     Get entity Department by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Department</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Department> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Department>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        return entity;
    }
}