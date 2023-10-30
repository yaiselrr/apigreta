using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for breed entity
/// </summary>
public interface IBreedService : IGenericBaseService<Breed>
{
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IBreedService" />
public class BreedService : BaseService<IBreedRepository, Breed>, IBreedService
{
    /// <inheritdoc />
    public BreedService(IBreedRepository repository, ILogger<BreedService> logger)
        : base(repository, logger)
    {
    }
}