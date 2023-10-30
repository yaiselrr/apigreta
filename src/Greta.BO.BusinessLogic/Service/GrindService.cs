using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for grind entity
/// </summary>
public interface IGrindService : IGenericBaseService<Grind>
{
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IGrindService" />
public class GrindService : BaseService<IGrindRepository, Grind>, IGrindService
{
    /// <inheritdoc />
    public GrindService(IGrindRepository repository, ILogger<GrindService> logger)
        : base(repository, logger)
    {
    }
}