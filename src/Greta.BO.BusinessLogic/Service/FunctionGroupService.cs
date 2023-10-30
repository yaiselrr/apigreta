using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.FunctionGroupSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// <inheritdoc/>
/// </summary>
public interface IFunctionGroupService : IGenericBaseService<FunctionGroup>
{
    /// <summary>
    /// Get entity by Application
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    Task<List<FunctionGroup>> GetByAplication(long applicationId);
}

/// <summary>
/// 
/// </summary>
public class FunctionGroupService : BaseService<IFunctionGroupRepository, FunctionGroup>, IFunctionGroupService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryRepository"></param>
    /// <param name="logger"></param>
    public FunctionGroupService(IFunctionGroupRepository categoryRepository, ILogger<FunctionGroupService> logger)
        : base(categoryRepository, logger)
    {
    }

    /// <summary>
    ///     Get all entities
    /// </summary>
    /// <returns></returns>
    public async Task<List<FunctionGroup>> GetByAplication(long applicationId)
    {
        var spec = new FunctionGroupSpec(applicationId);
        var entities = await _repository.GetEntity<FunctionGroup>().WithSpecification(spec).ToListAsync();            
        return entities;
    }
}