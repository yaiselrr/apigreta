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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for scale category entity
/// </summary>
public interface IScaleCategoryService : IGenericBaseService<ScaleCategory>
{
    /// <summary>
    ///Get Scale Category by category id and entity Id
    /// </summary>
    /// <param name="catId">Category Id</param>
    /// <param name="id">Entity Id</param>
    /// <returns></returns>
    Task<ScaleCategory> GetByScaleCategoryId(int catId, long id = -1);

    /// <summary>
    /// Get a list of Category
    /// </summary>
    /// <param name="last">Last Date</param>
    /// <param name="deps">Lis of departments Ids</param>
    /// <returns></returns>
    Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetUpdatesForScales(DateTime last,
        List<long> deps);

    /// <summary>
    /// Get a list of Category
    /// </summary>
    /// <param name="deps">Lis of departments Ids</param>
    /// <returns></returns>
    Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetAllForScales(List<long> deps);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IScaleCategoryService" />
public class ScaleCategoryService : BaseService<IScaleCategoryRepository, ScaleCategory>, IScaleCategoryService
{
    /// <inheritdoc />
    public ScaleCategoryService(IScaleCategoryRepository repository,
        ISynchroService synchroService,
        ILogger<ScaleCategoryService> logger)
        : base(repository, logger, synchroService, Converter)
    {
    }

    private static object Converter(ScaleCategory from) => (LiteScaleCategory.Convert(from));

    /// <summary>
    ///     Get entity by name
    /// </summary>
    /// <returns>ScaleCategory</returns>
    public override async Task<List<ScaleCategory>> Get()
    {
        var entity = await _repository.GetEntity<ScaleCategory>()
            .Include(x => x.Department)
            .Include(x => x.Parent)
            .ToListAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<ScaleCategory> GetByScaleCategoryId(int catId, long id = -1)
    {
        if (id == -1)
            return await _repository.GetEntity<ScaleCategory>().Where(x => x.CategoryId == catId)
                .Include(x => x.Department).FirstOrDefaultAsync();
        return await _repository.GetEntity<ScaleCategory>().Where(x => x.CategoryId == catId && x.Id != id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetUpdatesForScales(DateTime last, List<long> deps)
    {
        if (deps == null || deps.Count == 0)
        {
            return await _repository.GetEntity<ScaleCategory>()
                .Include(x => x.Parent)
                .Where(x =>
                    x.State &&
                    (
                        x.CreatedAt > last ||
                        x.UpdatedAt > last
                    ))
                .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
                {
                    Id = x.CategoryId,
                    Name = x.Name,
                    DepartmentId = x.DepartmentId,
                    ParentID = x.Parent != null ? x.Parent.CategoryId : 0
                })
                .ToListAsync();
        }
        else
        {
            return await _repository.GetEntity<ScaleCategory>()
                .Include(x => x.Parent)
                .Where(x =>
                    x.State &&
                    (
                        x.CreatedAt > last ||
                        x.UpdatedAt > last
                    ) && deps.Any(d => d == x.DepartmentId))
                .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
                {
                    Id = x.CategoryId,
                    Name = x.Name,
                    DepartmentId = x.DepartmentId,
                    ParentID = x.Parent != null ? x.Parent.CategoryId : 0
                })
                .ToListAsync();
        }

    }

    /// <inheritdoc />
    public async Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetAllForScales(List<long> deps)
    {
        if (deps == null || deps.Count == 0)
        {
            return await _repository.GetEntity<ScaleCategory>()
                .Include(x => x.Parent)
                .Where(x =>
                    x.State)
                .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
                {
                    Id = x.CategoryId,
                    Name = x.Name,
                    DepartmentId = x.DepartmentId,
                    ParentID = x.Parent != null ? x.Parent.CategoryId : 0
                })
                .ToListAsync();
        }
        else
        {
            return await _repository.GetEntity<ScaleCategory>()
                .Include(x => x.Parent)
                .Where(x =>
                    x.State && deps.Any(d => d == x.DepartmentId))
                .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
                {
                    Id = x.CategoryId,
                    Name = x.Name,
                    DepartmentId = x.DepartmentId,
                    ParentID = x.Parent != null ? x.Parent.CategoryId : 0
                })
                .ToListAsync();
        }
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, ScaleCategory entity)
    {
        var re = await _repository.GetEntity<ScaleCategory>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (re == null)
            return false;

        re.ParentId = entity.ParentId;
        re.Name = entity.Name;
        re.DepartmentId = entity.DepartmentId;
        re.CategoryId = entity.CategoryId;
        re.BackgroundColor = entity.BackgroundColor;
        re.ForegroundColor = entity.ForegroundColor;

        return await base.Put(id, re);
    }
}