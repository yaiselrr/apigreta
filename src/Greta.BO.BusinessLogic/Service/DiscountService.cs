using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for discount entity
/// </summary>
public interface IDiscountService : IGenericBaseService<Discount>
{
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IDiscountService" />
public class DiscountService : BaseService<IDiscountRepository, Discount>, IDiscountService
{
    /// <inheritdoc />
    public DiscountService(IDiscountRepository discountRepository,
        ISynchroService synchroService,
        ILogger<DiscountService> logger)
        : base(discountRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(Discount from) => (LiteDiscount.Convert(from));

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Discount> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Discount>()
            .Include(x => x.Products)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Discount> Post(Discount entity)
    {
        if (entity.CategoryId == -1) entity.CategoryId = null;
        if (entity.DepartmentId == -1) entity.DepartmentId = null;
        // var elem = new List<Store>();
        if (entity.Products != null)
            for (var i = 0; i < entity.Products.Count; i++)
                _repository.GetEntity<Product>().Attach(entity.Products[i]);
        return await base.Post(entity);
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Discount entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        if (entity.CategoryId == -1) entity.CategoryId = null;
        if (entity.DepartmentId == -1) entity.DepartmentId = null;
        // remove all stores first
        var tax = await _repository.GetEntity<Discount>()
            .Include(x => x.Products)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        entity.Products = ProcessMany2ManyUpdate(tax.Products, entity.Products);

        return await base.Put(id, entity);
    }
}