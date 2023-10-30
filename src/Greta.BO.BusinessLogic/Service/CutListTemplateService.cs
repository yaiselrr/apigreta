using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.CutListTemplateSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for CutListTemplate entity
/// </summary>
public interface ICutListTemplateService : IGenericBaseService<CutListTemplate>
{
    /// <summary>
    /// Determine if this CutListTemplate entity can be deleted
    /// </summary>
    /// <param name="id">CutListTemplate Id</param>
    /// <returns>Return true if this CutListTemplate don't have any product associated</returns>
    Task<bool> CanDeleted(long id);    
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.ICutListTemplateService" />
public class CutListTemplateService : BaseService<ICutListTemplateRepository, CutListTemplate>, ICutListTemplateService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cutListTemplateRepository"></param>
    /// <param name="logger"></param>
    public CutListTemplateService(ICutListTemplateRepository cutListTemplateRepository, ILogger<CutListTemplateService> logger)
        : base(cutListTemplateRepository, logger)
    {
    }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(long id)
    {
        return await _repository.GetEntity<CutListTemplate>()
            .AnyAsync(e => e.Id == id && !e.ScaleProducts.Any());
    }   

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<CutListTemplate> Post(CutListTemplate entity)
    {
        if (entity.ScaleProducts != null)
            foreach (var t in entity.ScaleProducts)
                _repository.GetEntity<Product>().Attach(t);      

        return await base.Post(entity);
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, CutListTemplate entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var model = await _repository.GetEntity<CutListTemplate>()
            .WithSpecification(new CutListTemplateGetByIdSpec(id))
            .FirstOrDefaultAsync();

        entity.ScaleProducts = ProcessMany2ManyUpdate(model.ScaleProducts, entity.ScaleProducts);        

        return await base.Put(id, entity);
    }
}