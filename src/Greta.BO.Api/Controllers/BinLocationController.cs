using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Dto.Search;
using Greta.BO.Api.Responses;
using Greta.BO.BusinessLogic.Handlers.Command.BinLocation;
using Greta.BO.BusinessLogic.Handlers.Queries.BinLocation;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class BinLocationController : BaseController
{
    private readonly IMediator _mediator;

    public BinLocationController(IMediator mediator, IMapper mapper) :
        base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new BinLocationGetAll.Query()));
    }

    /// <summary>
    ///     Get all the Entities by store id
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("[action]/{storeId}")]
    public async Task<IActionResult> GetByStore(long storeId)
    {
        return Ok(await _mediator.Send(new BinLocationGetAll.Query(storeId)));
    }

    /// <summary>
    ///     Get filter and paginate BinLocation
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated BinLocation</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] BinLocationSearchDto filter)
    {
        return Ok(await _mediator.Send(new BinLocationFilter.Query(currentPage, pageSize,
            _mapper.Map<BinLocationSearchModel>(filter))));
    }

    /// <summary>
    ///     Get Entity by id
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{entityId}")]
    [ProducesResponseType(404)]
    // [ProducesResponseType(typeof(BinLocationGetById.Response), 200)]
    public async Task<IActionResult> Get(long entityId)
    {
        var data = await _mediator.Send(new BinLocationGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new BinLocation
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] BinLocationModel entitydto)
    {
        return Ok(await _mediator.Send(new BinLocationCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a BinLocation by Id
    /// </summary>
    /// <param name="entityId">BinLocation Id</param>
    /// <param name="entitydto">BinLocation data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(BinLocationUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateBinLocationById(long entityId, [FromBody] BinLocationModel entitydto)
    {
        if (entityId < 1)
        {
            return NotFound();
        }
        else
        {
            return Ok(await _mediator.Send(new BinLocationUpdate.Command(entityId,
                entitydto)));
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
        return Ok(await _mediator.Send(new BinLocationChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Delete a BinLocation by Id
    /// </summary>
    /// <param name="entityId">BinLocation Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(BinLocationDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BinLocationDelete.Command(entityId)));
    }

    /// <summary>
    ///     Delete list of BinLocation
    /// </summary>
    /// <param name="ids">List of ids of BinLocation</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(BinLocationDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new BinLocationDeleteRange.Command(ids));
        return Ok();
    }

    /// <summary>
    ///     Add bin location to a list a product
    /// </summary>
    /// <param name="model">data</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> AddUPCToBinLocation([FromBody] BinLocationUPCModel model)
        => Ok(await _mediator.Send(new AddUPCToBinLocation.Command(model)));


    /// <summary>
    ///     Get filter and paginate Region
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated Region</returns>
    [HttpPost]
    [Route("[action]/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
    public async Task<IActionResult> BinLocationProducts(int currentPage, int pageSize,
        [FromBody] InventorySearchModel filter)
        => Ok(await _mediator.Send(new BinLocationProducts.Query(currentPage, pageSize,
            filter)));

    /// <summary>
    ///     Get filter all products 
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated Region</returns>
    [HttpPost]
    [Route("[action]/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
    public async Task<IActionResult> AllProductsByStore(int currentPage, int pageSize,
        [FromBody] AllProductsByStoreRequestModel filter)
        => Ok(await _mediator.Send(new AllProductsByStore.Query(currentPage, pageSize,
            filter)));


    /// <summary>
    ///     Delete a BinLocation by Id
    /// </summary>
    /// <param name="entityId">BinLocation Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(BinLocationDelete.Response), 200)]
    public async Task<IActionResult> DeleteProduct(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BinLocationDeleteProduct.Command(entityId)));
    }

    /// <summary>
    ///     Delete a BinLocation by Id
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <param name="upc">Product Upc</param>
    /// <returns>Return true or false</returns>
    [HttpGet]
    [Route("[action]/{storeId}/{upc}")]
    public async Task<IActionResult> GetProductByUPC(long storeId, string upc)
        => Ok(await _mediator.Send(new GetProductByUPC.Query(storeId, upc)));
}