using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleReasonCodes;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleReasonCodes;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class ScaleReasonCodesController : BaseController
{
    private readonly IMediator _mediator;

    public ScaleReasonCodesController(IMediator mediator, IMapper mapper) :
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
        return Ok(await _mediator.Send(new ScaleReasonCodesGetAll.Query()));
    }

    /// <summary>
    /// Get all the Entities to export
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Csv()
    {
        return Ok(await _mediator.Send(new ScaleReasonCodesGetAll.Query(true)));
    }

    /// <summary>
    ///     Get filter and paginate ScaleReasonCodes
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated ScaleReasonCodes</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ScaleReasonCodesFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize,
        [FromBody] ScaleReasonCodesSearchModel filter)
    {
        return Ok(await _mediator.Send(new ScaleReasonCodesFilter.Query(currentPage, pageSize,
            _mapper.Map<ScaleReasonCodesSearchModel>(filter))));
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
        var data = await _mediator.Send(new ScaleReasonCodesGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new ScaleReasonCodes
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] ScaleReasonCodesModel entitydto)
    {
        return Ok(await _mediator.Send(
            new ScaleReasonCodesCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">ScaleReasonCodes Id</param>
    /// <param name="entitydto">ScaleReasonCodes data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(ScaleReasonCodesUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long entityId, [FromBody] ScaleReasonCodesModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new ScaleReasonCodesUpdate.Command(entityId,
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
        return Ok(await _mediator.Send(new ScaleReasonCodesChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Delete a ScaleReasonCodes by Id
    /// </summary>
    /// <param name="entityId">ScaleReasonCodes Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(ScaleReasonCodesDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new ScaleReasonCodesDelete.Command(entityId)));
    }

    /// <summary>
    ///     Delete list of ScaleReasonCodes
    /// </summary>
    /// <param name="ids">List of ids of ScaleReasonCodes</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(ScaleReasonCodesDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new ScaleReasonCodesDeleteRange.Command(ids));
        return Ok();
    }
}