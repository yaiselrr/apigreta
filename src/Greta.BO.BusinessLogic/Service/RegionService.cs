using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Specifications.RegionSpecs;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for region entity
/// </summary>

public interface IRegionService : IGenericBaseService<Region>
{
    
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IRegionService" />
public class RegionService : BaseService<IRegionRepository, Region>, IRegionService
{
    /// <inheritdoc />
    public RegionService(IRegionRepository repository, ILogger<RegionService> logger)
        : base(repository, logger)
    {
    }
}
