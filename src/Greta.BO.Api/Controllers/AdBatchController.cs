using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.AdBatch;
using Greta.BO.BusinessLogic.Handlers.Queries.AdBatch;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class AdBatchController : BaseController
{
    private readonly IMediator _mediator;

    public AdBatchController(IMediator mediator, IMapper mapper) : base(mapper)
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
        return Ok(await _mediator.Send(new AdBatchGetAll.Query()));
    }

    /// <summary>
    ///     Get filter and paginate AdBatch
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated AdBatch</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(AdBatchFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] AdBatchSearchDto filter)
    {
        return Ok(await _mediator.Send(new AdBatchFilter.Query(currentPage, pageSize,
            _mapper.Map<AdBatchSearchModel>(filter))));
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
        var data = await _mediator.Send(new AdBatchGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new AdBatch
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] AdBatchCreateModel entitydto)
    {
        return Ok(await _mediator.Send(new AdBatchCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">AdBatch Id</param>
    /// <param name="entitydto">AdBatch data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(RegionUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long entityId, [FromBody] AdBatchModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new AdBatchUpdate.Command(entityId, entitydto)));
    }

    //
    // /// <summary>
    // ///     Delete a AdBatch by Id
    // /// </summary>
    // /// <param name="entityId">AdBatch Id</param>
    // /// <returns>Return true or false</returns>
    // [HttpDelete]
    // [Route("{entityId}")]
    // // [ProducesResponseType(typeof(AdBatchDelete.Response), 200)]
    // public async Task<IActionResult> Delete(long entityId)
    // {
    //     return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new AdBatchDelete.Command(entityId)));
    // }

    /// <summary>
    ///     Delete list of AdBatch
    /// </summary>
    /// <param name="ids">List of ids of AdBatch</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(AdBatchDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new AdBatchDeleteRange.Command(ids));
        return Ok();
    }
}