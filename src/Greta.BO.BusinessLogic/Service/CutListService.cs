using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for CutList entity
/// </summary>
public interface ICutListService : IGenericBaseService<CutList>
{
    /// <summary>
    /// List of customer associated with an animal
    /// </summary>
    /// <param name="animalId">Animal Id</param>
    /// <returns>List of customers</returns>
    Task<List<Customer>> GetCustomerByAnimal(long animalId);

    /// <summary>
    /// Get CutList by animal and customer 
    /// </summary>
    /// <param name="cutListId"></param>
    /// <returns>If exist return CutList, else return null</returns>
    Task<List<CutListDetail>> GetCutListDetails(long cutListId);
    
    /// <summary>
    /// Get CutList order with details
    /// </summary>
    /// <param name="id">Cut List Id</param>
    /// <returns>Return a Cut List with details</returns>
    Task<CutList> GetWithDetails(long id);

    /// <summary>
    /// Get ScaleProduct by UPC and PLUNumber 
    /// </summary>
    /// <param name="upc"></param>
    /// <param name="plu"></param>
    /// <param name="animalId"></param>
    Task<List<ScaleProduct>> GetScaleProductsByUpcAndPlu(string upc, int plu, long animalId);

    /// <summary>
    /// Get ScaleProduct of Template 
    /// </summary>
    /// <param name="cutListTemplateId"></param>
    /// <param name="animalId"></param>
    Task<List<ScaleProduct>> GetScaleProductsOfTemplate(long cutListTemplateId, long animalId);
}

/// <inheritdoc cref="ICutListService"/>
public class CutListService : BaseService<ICutListRepository, CutList>, ICutListService
{
    /// <inheritdoc />
    public CutListService(ICutListRepository cutListRepository, ILogger<CutListService> logger)
        : base(cutListRepository, logger)
    {
    }

    /// <inheritdoc />
    public async Task<List<Customer>> GetCustomerByAnimal(long animalId)
    {
        return await _repository.GetEntity<Customer>().Where(x => x.Animals.Any( a=>a.Id == animalId)).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<CutListDetail>> GetCutListDetails(long cutListId)
    {
        return await _repository.GetEntity<CutListDetail>().Where(x => x.CutListId == cutListId).ToListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="upc"></param>
    /// <param name="plu"></param>
    /// <param name="animalId"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<List<ScaleProduct>> GetScaleProductsByUpcAndPlu(string upc, int plu, long animalId)
    {
        if (string.IsNullOrEmpty(upc) || plu < 1 || animalId < 1)
        {
            throw new BusinessLogicException("Parameters out of bounds");
        }

        var animal = await _repository.GetEntity<Animal>().WithSpecification(new GetByIdSpec<Animal>(animalId)).SingleOrDefaultAsync();

        if (animal is { StoreId: > 0 })
        {
            return await _repository.GetEntity<ScaleProduct>()
                                .WithSpecification(new CutListGetScaleProductByUpcAndPluSpec(upc, plu, animal.StoreId))
                                .ToListAsync();
        }

        _logger.LogError("Id parameter out of bounds");
        throw new BusinessLogicException("Id parameter out of bounds");

    }

    /// <summary>
    /// Get list of ScaleProduct of Template
    /// </summary>
    /// <param name="cutListTemplateId"></param>
    /// <param name="animalId"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<List<ScaleProduct>> GetScaleProductsOfTemplate(long cutListTemplateId, long animalId)
    {
        if (cutListTemplateId < 1 || animalId < 1)
        {
            throw new BusinessLogicException("Parameters out of bounds");
        }

        var animal = await _repository.GetEntity<Animal>().WithSpecification(new GetByIdSpec<Animal>(animalId)).SingleOrDefaultAsync();

        if (animal is { StoreId: > 0 })
        {
            return await _repository.GetEntity<ScaleProduct>()
                                .WithSpecification(new CutListGetScaleProductOfTemplateSpec(cutListTemplateId, animal.StoreId))
                                .ToListAsync();
        }

        _logger.LogError("Id parameter out of bounds");
        throw new BusinessLogicException("Id parameter out of bounds");

    }

    /// <inheritdoc />
    public async Task<CutList> GetWithDetails(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<CutList>()
            .Include(v => v.CutListDetails)
            .FirstOrDefaultAsync(v => v.Id == id);

        return entity;
    }
    
}