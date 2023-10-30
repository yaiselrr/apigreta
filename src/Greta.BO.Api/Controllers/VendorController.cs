using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.Vendor;
using Greta.BO.BusinessLogic.Handlers.Queries.Vendor;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class VendorController : BaseController
{
    private readonly IMediator _mediator;

    public VendorController(IMediator mediator, IMapper mapper) : base(mapper)
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
        return Ok(await _mediator.Send(new VendorGetAll.Query()));
    }

    /// <summary>
    ///     Get filter and paginate Vendor
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated Vendor</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(VendorFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] VendorSearchDto filter)
    {
        return Ok(await _mediator.Send(new VendorFilter.Query(currentPage, pageSize,
            _mapper.Map<VendorSearchModel>(filter))));
    }


    /// <summary>
    ///     Get Entity by id
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{entityId}")]
    [ProducesResponseType(404)]
    // [ProducesResponseType(typeof(ProfilesGetById.Response), 200)]
    public async Task<IActionResult> Get(long entityId)
    {
        var data = await _mediator.Send(new VendorGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new Vendor
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] VendorModel entitydto)
    {
        return Ok(await _mediator.Send(new VendorCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">Vendor Id</param>
    /// <param name="entitydto">Vendor data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(VendorUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] VendorModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new VendorUpdate.Command(entityId, entitydto)));
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
        return Ok(await _mediator.Send(new VendorChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Delete a Vendor by Id
    /// </summary>
    /// <param name="entityId">Vendor Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(VendorDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new VendorDelete.Command(entityId)));
    }

    /// <summary>
    ///     Delete list of Vendor
    /// </summary>
    /// <param name="ids">List of ids of Vendor</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(VendorDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new VendorDeleteRange.Command(ids));
        return Ok();
    }
}