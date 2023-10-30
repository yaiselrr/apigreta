using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for customer entity
/// </summary>
public interface ICustomerService : IGenericBaseService<Customer>
{
    /// <summary>
    ///     Get entity by name
    /// </summary>
    /// <param name="name">Customer name</param>
    /// <returns>Customer</returns>
    Task<Customer> GetByName(string name, long Id = -1);

    /// <summary>
    ///     Update Points
    /// </summary>
    /// <param name="customers">Customers List</param>
    /// <returns>Boolean success</returns>
    Task UpdatePoints(List<Customer> customers);

    /// <summary>
    ///     Get entity by name
    /// </summary>
    /// <param name="phone">Customer phone</param>
    /// <returns>Customer</returns>
    Task<Customer> SearchByPhone(string phone);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.ICustomerService" />
public class CustomerService : BaseService<ICustomerRepository, Customer>, ICustomerService
{
    private readonly IConfiguration configuration;

    /// <inheritdoc />
    public CustomerService(ICustomerRepository repository,
        ISynchroService synchroService, IConfiguration configuration,
        ILogger<CustomerService> logger)
        : base(repository, logger, synchroService, Converter)
    {
        this.configuration = configuration;
    }

    private static object Converter(Customer from) => (LiteCustomer.Convert(from));

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Customer> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Customer>()
            .Include(x => x.Discounts)
            .Include(x => x.MixAndMatches)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<Customer> SearchByPhone(string phone)
    {
        return await _repository.GetEntity<Customer>()
            .Include(x => x.Discounts)
            .Include(x => x.MixAndMatches)
            .Where(x => x.Phone.Contains(phone))
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Customer> GetByName(string name, long Id = -1)
    {
        if (Id == -1)
            return await _repository.GetEntity<Customer>().Where(x => x.FirstName == name).FirstOrDefaultAsync();
        return await _repository.GetEntity<Customer>().Where(x => x.FirstName == name && x.Id != Id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Customer> Post(Customer entity)
    {
        if (entity.Discounts != null)
            foreach (var t in entity.Discounts)
                _repository.GetEntity<Discount>().Attach(t);

        if (entity.MixAndMatches != null)
            foreach (var t in entity.MixAndMatches)
                _repository.GetEntity<MixAndMatch>().Attach(t);

        return await base.Post(entity);
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Customer entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
        
        // remove all stores first
        var cust = await _repository.GetEntity<Customer>()
            .Include(x => x.MixAndMatches)
            .Include(x => x.Discounts)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        entity.Discounts = ProcessMany2ManyUpdate(cust.Discounts, entity.Discounts);
        entity.MixAndMatches = ProcessMany2ManyUpdate(cust.MixAndMatches, entity.MixAndMatches);

        return await base.Put(id, entity);
    }

    /// <inheritdoc />
    public async Task UpdatePoints(List<Customer> customers)
    {
        var b = new DbContextOptionsBuilder()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection")
            , sqlopt =>
            {
                sqlopt.UseAdminDatabase("defaultdb");
                sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                    null);
            });

        using (var context = new SqlServerContext(b.Options))
        {
            try
            {
                foreach (var s in customers)
                {

                    var gc = await context.Set<Customer>()
                        .Where(x => x.Id == s.Id)
                        .FirstOrDefaultAsync();
                    if (gc != null)
                    {
                        gc.StoreCredit += s.StoreCredit;
                        gc.LastBuy = s.LastBuy;
                        context.Set<Customer>().Update(gc);
                        _logger.LogInformation($"Customer {gc.FirstName} have now {gc.StoreCredit.ToString()}.");
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UpdatePoints error.");
            }
        }
    }
}