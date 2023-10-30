using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.BO.BusinessLogic.Specifications.MixAndMatchSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// 
/// </summary>
public interface IMixAndMatchService : IGenericBaseService<MixAndMatch>
{    
}

/// <summary>
/// 
/// </summary>
public class MixAndMatchService : BaseService<IMixAndMatchRepository, MixAndMatch>, IMixAndMatchService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="familyRepository"></param>
    /// <param name="logger"></param>
    public MixAndMatchService(IMixAndMatchRepository familyRepository, ILogger<MixAndMatchService> logger)
       : base(familyRepository, logger)
    {
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="synchroService"></param>
    /// <param name="logger"></param>
    public MixAndMatchService(IMixAndMatchRepository repository,
        ISynchroService synchroService,
        ILogger<MixAndMatchService> logger)
        : base(repository, logger, synchroService, Converter)
    {
    }

    private static object Converter(MixAndMatch from) => (LiteMixAndMatch.Convert(from));
    
    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<MixAndMatch> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<MixAndMatch>()
                                      .WithSpecification(new MixAndMatchGetByIdSpec(id))
                                      .FirstOrDefaultAsync();
            
        return entity;
    }      

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<MixAndMatch> Post(MixAndMatch entity)
    {        
        if (entity.Products != null)
            foreach (var t in entity.Products)
                _repository.GetEntity<Product>().Attach(t);

        if (entity.Families != null)
            foreach (var t in entity.Families)
                _repository.GetEntity<Family>().Attach(t);

        return await base.Post(entity);        
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, MixAndMatch entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
                
        var model = await _repository.GetEntity<MixAndMatch>()
            .WithSpecification(new MixAndMatchGetByIdUpdateSpec(id))            
            .FirstOrDefaultAsync();

        entity.Products = ProcessMany2ManyUpdate(model.Products, entity.Products);
        entity.Families = ProcessMany2ManyUpdate(model.Families, entity.Families);

        return await base.Put(id, entity);        
    }
}