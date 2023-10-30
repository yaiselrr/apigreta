using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for scale label type entity
/// </summary>
public interface IScaleLabelTypeService : IGenericBaseService<ScaleLabelType>
{

    /// <summary>
    ///     Get Entities by type
    /// </summary>
    /// <param name="type">ScaleType 0 for shelfTag, 1 GretaLabel 2 for External</param>
    /// <returns></returns>
    Task<List<ScaleLabelType>> GetByType(ScaleType type);

    /// <summary>
    ///     Filter and sort list of entities
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter params</param>
    /// <param name="searchstring">basic searc string</param>
    /// <param name="sortstring">sort string </param>
    /// <returns></returns>
    Task<Pager<ScaleLabelType>> FilterTag(int currentPage, int pageSize, ScaleLabelType filter, string searchstring,
        string sortstring);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IScaleLabelTypeService" />
public class ScaleLabelTypeService : BaseService<IScaleLabelTypeRepository, ScaleLabelType>, IScaleLabelTypeService
{
    /// <inheritdoc />
    public ScaleLabelTypeService(IScaleLabelTypeRepository scaleLabelTypeRepository,
        ILogger<ScaleLabelTypeService> logger, ISynchroService synchroService)
        : base(scaleLabelTypeRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(ScaleLabelType from) => LiteScaleLabelType.Convert(from);

    /// <inheritdoc />
    public async Task<Pager<ScaleLabelType>> FilterTag(int currentPage, int pageSize, ScaleLabelType filter,
        string searchstring, string sortstring)
    {
        if (currentPage < 1 || pageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

        var query = _repository.GetEntity<ScaleLabelType>();

        IQueryable<ScaleLabelType> query1 = null;

        if (!string.IsNullOrEmpty(searchstring))
            query1 = query.Where(c => c.Name.Contains(searchstring));
        else
            query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));

        query1 = query1
            .Switch(splited)
            .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
            .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
            .OrderByDefault(e => e.Name);

        query1 = query1.Where(x => x.ScaleType == ScaleType.SHELFTAG)
            .Select(x => new ScaleLabelType()
            {
                Id = x.Id,
                State = x.State,
                Name = x.Name,
                LabelId = x.LabelId,
                ScaleType = x.ScaleType,
                Design = "" //x.Design
            });

        var entities = await query1.ToPageAsync(currentPage, pageSize);
        return entities;
    }

    /// <inheritdoc />
    public async Task<List<ScaleLabelType>> GetByType(ScaleType type)
    {
        return await _repository.GetEntity<ScaleLabelType>().Where(x => x.ScaleType == type)
            .Select(x => new ScaleLabelType()
            {
                Id = x.Id,
                State = x.State,
                Name = x.Name,
                LabelId = x.LabelId,
                ScaleType = x.ScaleType,
                Design = "" //x.Design
            })
            .ToListAsync();
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<ScaleLabelType> Post(ScaleLabelType entity)
    {
        if (entity.ScaleType == ScaleType.GRETALABEL)
        {
            // we need add a 500+ value
            var maxValue = await _repository.GetEntity<ScaleLabelType>()
                .Where(x => x.ScaleType == ScaleType.GRETALABEL)
                .Select(x => x.LabelId)
                .OrderBy(x => x)
                .LastOrDefaultAsync();

            if (maxValue == 0)
                maxValue = 500;
            else
                maxValue++;
            entity.LabelId = maxValue;
        }

        return await base.Post(entity);
    }

    /// <summary>
    ///     Update a entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="id">Entity Id</param>
    /// <returns>Entity</returns>
    public override async Task<bool> Put(long id, ScaleLabelType entity)
    {
        _repository.GetContext<SqlServerContext>().Entry(entity).Property(nameof(ScaleLabelType.LabelId))
            .IsModified = false;

        return await base.Put(id, entity);
    }
    
    protected override IQueryable<ScaleLabelType> FilterqueryBuilder(
        ScaleLabelType filter,
        string searchstring,
        string[] splited,
        DbSet<ScaleLabelType> query)
    {
        IQueryable<ScaleLabelType> query1 = null;

        if (!string.IsNullOrEmpty(searchstring))
            query1 = query.Where(c => c.Name.Contains(searchstring));
        else
            query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));

        query1 = query1
            .Switch(splited)
            .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
            .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
            .OrderByDefault(e => e.Name);

        return query1.Where(x => x.ScaleType != ScaleType.SHELFTAG).Select(x => new ScaleLabelType()
        {
            Id = x.Id,
            State = x.State,
            Name = x.Name,
            LabelId = x.LabelId,
            ScaleType = x.ScaleType,
            Design = "" //x.Design
        });
    }
}