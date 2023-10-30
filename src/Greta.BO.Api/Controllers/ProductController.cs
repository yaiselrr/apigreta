using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Handlers.Command.Products;
using Greta.BO.BusinessLogic.Handlers.Queries.Products;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ProductController : BaseController
{
    private readonly IMediator _mediator;
    SqlServerContext context;

    public ProductController(
        IMediator mediator
        , IMapper mapper,
SqlServerContext context) : base(mapper)
    {
        _mediator = mediator;
        this.context = context;        
    }

    /// <summary>
    ///     Get filter and paginate products
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated products</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ProductFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] ProductSearchModel filter,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ProductFilter.Query(currentPage, pageSize, filter), cancellationToken));
    }

    /// <summary>
    ///     Get filter and paginate products
    /// </summary>
    /// <param name="store">store</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated products</returns>
    [HttpPost]
    [Route("[action]/{store}/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ProductFilter.Response), 200)]
    public async Task<IActionResult> FilterByStore(long store, int currentPage, int pageSize,
        [FromBody] ProductSearchModel filter)
    {
        return Ok(await _mediator.Send(new ProductFilterByStore.Query(store, currentPage, pageSize,
            filter)));
    }

    /// <summary>
    ///     Get filter and paginate products permit for batch
    /// </summary>
    /// <param name="batch">batch id</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated products</returns>
    [HttpPost]
    [Route("[action]/{batch}/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ProductFilter.Response), 200)]
    public async Task<IActionResult> FilterByBatch(long batch, int currentPage, int pageSize,
        [FromBody] ProductSearchModel filter)
    {
        return Ok(await _mediator.Send(new ProductFilterByBatch.Query(batch, currentPage, pageSize,
            filter)));
    }

    /// <summary>
    ///     Get filter and paginate products permit for family
    /// </summary>
    /// <param name="family">family id</param>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated products</returns>
    [HttpPost]
    [Route("[action]/{family}/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ProductFilter.Response), 200)]
    public async Task<IActionResult> FilterByFamily(long family, int currentPage, int pageSize,
        [FromBody] ProductSearchModel filter)
    {
        return Ok(await _mediator.Send(new ProductFilterByFamily.Query(family, currentPage, pageSize,
            filter)));
    }

    /// <summary>
    ///     Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get(int storeId)
    {
        return Ok(await _mediator.Send(new ProductGetAll.Query()));
    }

    /// <summary>
    ///     Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("getByStoreId/{storeId}")]
    public async Task<IActionResult> GetByStoreId(long storeId)
    {
        return Ok(await _mediator.Send(new ProductGetAllByIdStore.Query(storeId)));
    }

    /// <summary>
    ///     Get by Upc
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("GetByUpc/{upc}")]
    public async Task<IActionResult> GetByUpc(string upc)
    {
        return Ok(await _mediator.Send(new ProductGetByUpcQuery(upc)));
    }
   
    /// <summary>
    ///     Get Filter and paginate Scale Products by Upc, PluNumber, ProductName anyone field is required
    /// </summary>
    /// <returns>Return list of ScaleProductLiteModel</returns>
    [HttpPost]
    [Route("ScaleProducts/{cutTemplateId}/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllScaleProduct(long cutTemplateId,int currentPage, int pageSize, [FromBody] ScaleProductSearchModel filter)
    {
        return Ok(await _mediator.Send(new ScaleProductGetByUpcPluProductQuery(cutTemplateId, currentPage, pageSize, filter)));
    }

    /// <summary>
    ///     Get Scale Products by Upc or Plu 
    /// </summary>
    /// <returns>Return list of ScaleProductLiteModel</returns>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> ScaleProductByUpcOrPlu(string filter)
    {
        return Ok(await _mediator.Send(new ScaleProductGetByUpcOrPluQuery(filter)));
    }

    /// <summary>
    ///     Get Scale Products by CutListTemplate Id
    /// </summary>
    /// <returns>Return list of ScaleProductLiteModel</returns>
    [HttpGet]
    [Route("ScaleProducts/{templateId}")]
    public async Task<IActionResult> ScaleProductByTemplate(long templateId)
    {
        return Ok(await _mediator.Send(new ScaleProductGetByCutListTemplateQuery(templateId)));
    }

    [HttpGet]
    [Route("{productType}/{productId}")]
    // [ProducesResponseType(typeof(ScaleProductGetById.Response), 200)]
    public async Task<IActionResult> GetById(ProductType productType, long productId)
    {
        switch (productType)
        {
            case ProductType.SLP:
                var queryScale = new ScaleProductGetById.Query(productId);
                var dataScale = await _mediator.Send(queryScale);
                return Ok(dataScale);
            case ProductType.KPT:
                var queryKit = new KitProductGetById.Query(productId);
                var dataKit = await _mediator.Send(queryKit);
                return Ok(dataKit);
            default:
                var query = new ProductGetById.Query(productId);
                var data = await _mediator.Send(query);
                return Ok(data);
        }
    }

    /// <summary>
    ///     Change State of entity
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <param name="state"></param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}/{state}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChangeState(long entityId, bool state)
    {
        if (entityId < 1)
            return NotFound();
        return Ok(await _mediator.Send(new ProductChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Create a new Product
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ProductCreate.ProductResult), 200)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductModel entitydto)
    {
        var command = new ProductCreate.ProductCommand(entitydto);
        var product = await _mediator.Send(command);
        return Ok(product);
    }

    /// <summary>
    ///     Create a new Scale Product
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ProductCreate.ScaleProductResult), 200)]
    public async Task<IActionResult> CreateScaleProduct([FromBody] ScaleProductModel entitydto)
    {
        var command = new ProductCreate.ScaleProductCommand(entitydto);
        var product = await _mediator.Send(command);
        return Ok(product);
    }

    /// <summary>
    ///     Create a new Kit Product
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ProductCreate.KitProductResult), 200)]
    public async Task<IActionResult> CreateKitProduct([FromBody] KitProductModel entitydto)
    {
        var command = new ProductCreate.KitProductCommand(entitydto);
        var product = await _mediator.Send(command);
        return Ok(product);
    }


    /// <summary>
    ///     Update a product by Id
    /// </summary>
    /// <param name="entityId">Product Id</param>
    /// <param name="entitydto">Product data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(ProductUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] ProductModel entitydto)
    {
        if (entityId < 1)
            return NotFound();

        var command = new ProductUpdate.ProductCommand(entityId, entitydto);
        var success = await _mediator.Send(command);
        return Ok(success);
    }

    /// <summary>
    ///     Update a scale product by Id
    /// </summary>
    /// <param name="entityId">Product Id</param>
    /// <param name="entitydto">Product data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(ProductUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateScaleProduct(long entityId, [FromBody] ScaleProductModel entitydto)
    {
        if (entityId < 1)
            return NotFound();

        var command = new ProductUpdate.ScaleProductCommand(entityId, entitydto);
        var success = await _mediator.Send(command);
        return Ok(success);
    }

    /// <summary>
    ///     Update a kit product by Id
    /// </summary>
    /// <param name="entityId">Product Id</param>
    /// <param name="entitydto">Product data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(ProductUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateKitProduct(long entityId, [FromBody] KitProductModel entitydto)
    {
        if (entityId < 1)
            return NotFound();

        var command = new ProductUpdate.KitProductCommand(entityId, entitydto);
        var success = await _mediator.Send(command);
        return Ok(success);
    }


    /// <summary>
    ///     Delete a Product by Id
    /// </summary>
    /// <param name="entityId">Product Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(ProductDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        var command = new ProductDelete.Command(entityId);
        var success = await _mediator.Send(command);
        return Ok(success);
    }

    /// <summary>
    ///     Delete list of product
    /// </summary>
    /// <param name="ids">List of ids of products</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ProductDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        var command = new ProductDeleteRange.Command(ids);
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    ///     Create a Rapid Product with Vendor and Store
    /// </summary>
    /// <param name="RapidProduct">RapidProductModel</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ProductDeleteRange.Response), 200)]
    public async Task<IActionResult> RapidProduct([FromBody] RapidProductModel RapidProduct)
    {
        var command = new RapidProductCreateCommand(RapidProduct);   
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}