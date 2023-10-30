using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

///<inheritdoc/>
public interface IRoundingTableService : IGenericBaseService<RoundingTable>
{
    /// <summary>
    /// Return the element on the rounding table that match with the endWith parameter or -1 if not found
    /// </summary>
    /// <param name="endWith"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> ChangeBy(int endWith, CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="IRoleService" />
public class RoundingTableService : BaseService<IRoundingTableRepository, RoundingTable>, IRoundingTableService
{
    ///<inheritdoc/>
    public RoundingTableService(IRoundingTableRepository repository, ILogger<RoundingTableService> logger)
        : base(repository, logger)
    {
    }

    /// <inheritdoc />
    public async Task<int> ChangeBy(int endWith, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetEntity<RoundingTable>()
            .Where(x => x.EndWith == endWith)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return result?.ChangeBy ?? -1;
    }
}