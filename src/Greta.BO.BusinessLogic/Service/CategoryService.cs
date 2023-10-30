using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for category entity
/// </summary>
public interface ICategoryService : IGenericBaseService<Category>
{
    /// <summary>
    ///     Get entity by categoryId
    /// </summary>
    /// <param name="catId">Category Id</param>
    /// <param name="id">Category Id</param>
    /// <returns>Category</returns>
    Task<Category> GetByCategoryId(int catId, long id = -1);

    /// <summary>
    ///     Get Category Id by categoryId
    /// </summary>
    /// <param name="catId">Category Id</param>
    /// <returns>Id</returns>
    Task<long> GetIdByCategoryId(int catId);

    /// <summary>
    ///     get a list of categories by department id
    /// </summary>
    /// <param name="dep">department id</param>
    /// <returns>List Categories</returns>
    Task<List<Category>> GetByDepartment(long dep);

    /// <summary>
    ///     Get Id by categoryId
    /// </summary>
    /// <param name="categoryId">Category Id</param>
    /// <returns>Id</returns>
    Task<long> GetIdFromCategoryId(int categoryId);
    /// <summary>
    /// Determine if this category entity can be deleted
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns>Return true if this category dont have any element associated</returns>
    Task<bool> CanDeleted(long id);
    /// <summary>
    /// Determine if this category entity can be deleted of list
    /// </summary>
    /// <param name="ids">Category Id</param>
    /// <returns>Return true if this category dont have any element associated</returns>
    Task<bool> CanDeleted(List<long> ids);

    /// <summary>
    ///     Get category entity with your products by categoryId
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns>Category</returns>
    Task<Category> GetCategoryWithProduct(long id);

    /// <summary>
    ///     Filter and sort list of entities
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter params</param>
    /// <param name="searchstring">basic searc string</param>
    /// <param name="sortstring">sort string </param>
    /// <returns></returns>
    Task<Pager<Category>> FilterCategory(int currentPage, int pageSize, CategorySearchModel filter, string searchstring,
            string sortstring);
    
    /// <summary>
    /// Get Category with online stores
    /// </summary>
    /// <param name="id">Category id</param>
    /// <returns></returns>
    Task<Category> GetWithOnlineStores(long id);
    
    /// <summary>
    /// Get a list of Category with online stores
    /// </summary>
    /// <param name="ids">List of ids of categories</param>
    /// <returns></returns>
    Task<List<Category>> GetWithOnlineStores(List<long> ids);

    /// <summary>
    /// Change isLiquorCategory of the Category
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isLiquorCategory"></param>
    /// <returns></returns>
    Task<bool> ChangeLiquor(long id, bool isLiquorCategory);

}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.ICategoryService" />
public class CategoryService : BaseService<ICategoryRepository, Category>, ICategoryService
{
    /// <inheritdoc />
    public CategoryService(ICategoryRepository categoryRepository,
        ISynchroService synchroService,
        ILogger<CategoryService> logger)
        : base(categoryRepository, logger, synchroService)
    {
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Category> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Category>()
            .Include(x => x.Taxs)
            .Include(x => x.Department)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <inheritdoc cref="BaseService{TRepository,TEntity}"/>
    public override async Task<List<Category>> Get()
    {
        var entity = await _repository.GetEntity<Category>()
            // .Include(x => x.Taxs)
            .Include(x => x.Department)
            .ToListAsync();
        return entity;
    }

    // /// <summary>
    // ///     Get entity by name
    // /// </summary>
    // /// <param name="name">Category name</param>
    // /// <returns>Category</returns>
    // public async Task<Category> GetByName(string name, long Id = -1)
    // {
    //     if (Id == -1)
    //         return await _repository.GetEntity<Category>().Where(x => x.Name == name).FirstOrDefaultAsync();
    //     return await _repository.GetEntity<Category>().Where(x => x.Name == name && x.Id != Id)
    //         .FirstOrDefaultAsync();
    // }

    /// <inheritdoc />
    public async Task<List<Category>> GetByDepartment(long dep)
    {
        return await _repository.GetEntity<Category>()
            .Where(x => x.DepartmentId == dep)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Category> GetByCategoryId(int catId, long id = -1)
    {
        if (id == -1)
            return await _repository.GetEntity<Category>().Where(x => x.CategoryId == catId).FirstOrDefaultAsync();
        return await _repository.GetEntity<Category>().Where(x => x.CategoryId == catId && x.Id != id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<long> GetIdByCategoryId(int catId)
    {
        return await _repository.GetEntity<Category>()
            .Where(x => x.CategoryId == catId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }


    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Category> Post(Category entity)
    {
        if (entity.DefaulShelfTagId == -1) entity.DefaulShelfTagId = null;
        if (entity.MinimumAge == -1) entity.MinimumAge = null;
        if (entity.Taxs != null)
            for (var i = 0; i < entity.Taxs.Count; i++)
                _repository.GetEntity<Tax>().Attach(entity.Taxs[i]);

        var response = await _repository.CreateAsync(entity);
        entity.Id = response;

        var stores = await _repository.GetEntity<Store>()
            .Include(x => x.Taxs)
            .Select(x => new
            {
                StoreId = x.Id,
                Taxes = x.Taxs.Select(t => t.Id).ToList()
            })
            .ToListAsync();

        foreach (var s in stores)
        {
            await synchroService.AddSynchroToStore(s.StoreId, LiteCategory.Convert(entity, s.Taxes), SynchroType.CREATE);
        }
        
        return entity;
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Category entity)
    {
        if (entity.DefaulShelfTagId == -1) entity.DefaulShelfTagId = null;
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        // remove all stores first
        var sP = await _repository.GetEntity<Category>()
            .Include(x => x.Taxs)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (entity.Taxs != null)
        {
            var longList = entity.Taxs.Select(x => x.Id).ToList();
            var removeList = new List<long>();
            var addList = new List<long>();
            var updateList = new List<long>();
            foreach (var store in sP.Taxs.ToList())
                // Remove the roles which are not in the list of new roles
                if (!longList.Contains(store.Id))
                {
                    sP.Taxs.Remove(store);
                    removeList.Add(store.Id);
                }

            foreach (var newStoreId in longList)
                // Add the roles which are not in the list of user's roles
                if (!sP.Taxs.Any(r => r.Id == newStoreId))
                {
                    addList.Add(newStoreId);
                    var newEnt = new Tax { Id = newStoreId };
                    _repository.GetEntity<Tax>().Attach(newEnt);
                    sP.Taxs.Add(newEnt);
                }
                else
                {
                    updateList.Add(newStoreId);
                }

            entity.Taxs = sP.Taxs;
        }
        
        var response = await _repository.UpdateAsync(id, entity);
        
        var stores = await _repository.GetEntity<Store>()
            .Include(x => x.Taxs)
            .Select(x => new
            {
                StoreId = x.Id,
                Taxes = x.Taxs.Select(t => t.Id).ToList()
            })
            .ToListAsync();
        entity.Id = id;
        foreach (var s in stores)
        {
            await synchroService.AddSynchroToStore(s.StoreId, LiteCategory.Convert(entity, s.Taxes), SynchroType.UPDATE);
        }
        

        return response;
    }

    /// <inheritdoc />
    public async Task<long> GetIdFromCategoryId(int categoryId)
    {
        return await _repository.GetEntity<Category>()
            .Where(x => x.CategoryId == categoryId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Pager<Category>> FilterCategory(int currentPage, int pageSize, CategorySearchModel filter, string searchstring, string sortstring)
    {
        if (currentPage < 1 || pageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

        var query = _repository.GetEntity<Category>()
            .Include(x => x.Department)
            .Include(x => x.Taxs).IgnoreAutoIncludes();

        if (!string.IsNullOrEmpty(searchstring))
            query = query.Where(c => c.Name.Contains(searchstring) || c.Description.Contains(searchstring) 
            ||  (c.CategoryId).ToString().Contains(searchstring) || c.Department.Name.Contains(searchstring));
        else
            query = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId)
                .WhereIf(!string.IsNullOrEmpty(filter.Description),
                    c => c.Description.Contains(filter.Description));

        query = query
            .Switch(splited)
            .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
            .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
            .OrderByCase(e => e[0] == "description" && e[1] == "asc", e => e.Description)
            .OrderByDescendingCase(e => e[0] == "description" && e[1] == "desc", e => e.Description)
            .OrderByCase(e => e[0] == "categoryId" && e[1] == "asc", e => e.CategoryId)
            .OrderByDescendingCase(e => e[0] == "categoryId" && e[1] == "desc", e => e.CategoryId)
            .OrderByCase(e => e[0] == "department" && e[1] == "asc", e => e.Department.Name)
            .OrderByDescendingCase(e => e[0] == "department" && e[1] == "desc", e => e.Department.Name)
            .OrderByDefault(e => e.Name);

        var entities = await query.ToPageAsync(currentPage, pageSize);
        return entities;
    }

    /// <inheritdoc />
    public async Task<Category> GetWithOnlineStores(long id)
    {
        return await _repository.GetEntity<Category>()
            .Include(x => x.OnlineCategories)
            .ThenInclude(x => x.OnlineStore)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetWithOnlineStores(List<long> ids)
    {
        return await _repository.GetEntity<Category>()
            .Include(x => x.OnlineCategories)
            .ThenInclude(x => x.OnlineStore)
            .Where(x => ids.Contains(x.Id) )
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(long id)
    {
        return await _repository.GetEntity<Category>()
            .Include(e => e.Products)
            .AnyAsync(e => e.Id == id && !e.Products.Any());
    }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(List<long> ids)
    {
        foreach (var id in ids)
            if (!await CanDeleted(id))
                return false;
        return true;
    }

    /// <inheritdoc />
    public async Task<Category> GetCategoryWithProduct(long id)
    {
        return await _repository.GetEntity<Category>()
            .Include(e => e.Products).ThenInclude(x => x.StoreProducts)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <inheritdoc />
    public async Task<bool> ChangeLiquor(long id, bool isLiquorCategory)
    {
        var entity = await _repository.GetEntity<Category>()
            .Where(x => x.Id == id).FirstOrDefaultAsync();

        if (entity != null)
        {
            entity.IsLiquorCategory = isLiquorCategory;

            return await Put(id, entity);
        }

        return false;
    }
}