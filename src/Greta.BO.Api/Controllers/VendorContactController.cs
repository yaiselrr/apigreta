using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.VendorContact;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorContact;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class VendorContactController : BaseController
{
    private readonly IMediator _mediator;

    public VendorContactController(IMediator mediator, IMapper mapper) :
        base(mapper)
    {
        _mediator = mediator;
    }


    /// <summary>
    ///     Get Vendor contact by vendor
    /// </summary>
    /// <param name="vendorId"> Vendor ID</param>
    /// <returns>Return list of vendor contacts for params vendor</returns>
    [HttpGet]
    [Route("[action]/{vendorId}")]
    [ProducesResponseType(typeof(List<VendorContactModel>), 200)]
    public async Task<IActionResult> GetByVendor(long vendorId)
    {
        var data = await _mediator.Send(new VendorContactGetByVendor.Query(vendorId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Get filter and paginate VendorContact
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated VendorContact</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(VendorContactFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] VendorContactSearchDto filter)
    {
        return Ok(await _mediator.Send(new VendorContactFilter.Query(currentPage, pageSize,
            _mapper.Map<VendorContactSearchModel>(filter))));
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
        var data = await _mediator.Send(new VendorContactGetById.Query(entityId));
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
    public async Task<IActionResult> Post([FromBody] VendorContactModel entitydto)
    {
        return Ok(await _mediator.Send(
            new VendorContactCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">VendorContact Id</param>
    /// <param name="entitydto">VendorContact data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(VendorContactUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] VendorContactModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new VendorContactUpdate.Command(entityId,
                entitydto)));
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
        return Ok(await _mediator.Send(new VendorContactChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Delete a Vendor by Id
    /// </summary>
    /// <param name="entityId">Vendor Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(VendorContactDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new VendorContactDelete.Command(entityId)));
    }

    /// <summary>
    ///     Delete list of Vendor
    /// </summary>
    /// <param name="ids">List of ids of Vendor</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(VendorContactDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new VendorContactDeleteRange.Command(ids));
        return Ok();
    }
}