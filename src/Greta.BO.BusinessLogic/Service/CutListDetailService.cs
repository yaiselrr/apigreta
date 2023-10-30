using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for CutListDetail entity
/// </summary>
public interface ICutListDetailService : IGenericBaseService<CutListDetail>
{
    /// <summary>
    /// Save list of cut list detail and assign to a cut list
    /// </summary>
    /// <param name="entity">List of cut list detail to save</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> PostMultiple(List<CutListDetail> entity, CancellationToken cancellationToken);
}

/// <inheritdoc cref="ICutListService"/>
public class CutListDetailService : BaseService<ICutListDetailRepository, CutListDetail>, ICutListDetailService
{
    /// <inheritdoc />
    public CutListDetailService(ICutListDetailRepository cutListDetailRepository,
        ILogger<CutListDetailService> logger)
        : base(cutListDetailRepository, logger)
    {
    }
    
    /// <inheritdoc />
    public async Task<bool> PostMultiple(List<CutListDetail> entity, CancellationToken cancellationToken)
    {
        var result = await _repository.CreateRangeAsync(entity, cancellationToken);
        return result.Count == entity.Count;
    }
}