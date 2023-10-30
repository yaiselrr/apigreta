using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.Scalendar;
using Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class ScalendarController : BaseController
{
    private readonly IMediator _mediator;

    public ScalendarController(IMediator mediator, IMapper mapper) :
        base(mapper)
    {
        _mediator = mediator;
    }

    // /// <summary>
    // ///     Get all the Entities
    // /// </summary>
    // /// <returns>Return list of entities</returns>
    // [HttpGet]
    // [Route("")]
    // public async Task<IActionResult> Get()
    // {
    //     return Ok(await _mediator.Send(new ScalendarGetAll.Query()));
    // }

    // /// <summary>
    // ///     Get filter and paginate Scalendar
    // /// </summary>
    // /// <param name="currentPage">Current page</param>
    // /// <param name="pageSize">Page size</param>
    // /// <param name="filter">Filter Object</param>
    // /// <returns>Returns the paginated Scalendar</returns>
    // [HttpPost]
    // [Route("{currentPage}/{pageSize}")]
    // // [ProducesResponseType(typeof(ScalendarFilter.Response), 200)]
    // public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] ScalendarSearchDto filter)
    // {
    //     return Ok(await _mediator.Send(new ScalendarFilter.Query(currentPage, pageSize,
    //         _mapper.Map<ScalendarSearchModel>(filter))));
    // }


    // /// <summary>
    // ///     Get Entity by id
    // /// </summary>
    // /// <param name="entityId">Entity Id</param>
    // /// <returns></returns>
    // [HttpGet]
    // [Route("{entityId}")]
    // [ProducesResponseType(404)]
    // // [ProducesResponseType(typeof(ProfilesGetById.Response), 200)]
    // public async Task<IActionResult> Get(long entityId)
    // {
    //     var data = await _mediator.Send(new ScalendarGetById.Query(entityId));
    //     return data != null ? Ok(data) : NotFound();
    // }

    // /// <summary>
    // ///     Create a new Scalendar
    // /// </summary>
    // /// <param name="entitydto">Entity to create</param>
    // /// <returns></returns>
    // [HttpPost]
    // [Route("")]
    // // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    // public async Task<IActionResult> Post([FromBody] ScalendarModel entitydto)
    // {
    //     return Ok(await _mediator.Send(new ScalendarCreate.Command(entitydto)));
    // }

    // /// <summary>
    // ///     Update a profile by Id
    // /// </summary>
    // /// <param name="entityId">Scalendar Id</param>
    // /// <param name="entitydto">Scalendar data to update</param>
    // /// <returns>Return true or false</returns>
    // [HttpPut]
    // [Route("{entityId}")]
    // [ProducesResponseType(typeof(ScalendarUpdateResponse), 200)]
    // [ProducesResponseType(404)]
    // public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] ScalendarModel entitydto)
    // {
    //     return entityId < 1
    //         ? NotFound()
    //         : Ok(await _mediator.Send(new ScalendarUpdateCommand(entityId,
    //             entitydto)));
    // }

    // /// <summary>
    // ///     Change State of entity
    // /// </summary>
    // /// <param name="entityId">Entity Id</param>
    // /// <param name="state"></param>
    // /// <returns>Return true or false</returns>
    // [HttpPut]
    // [Route("[action]/{entityId}/{state}")]
    // [ProducesResponseType(typeof(bool), 200)]
    // [ProducesResponseType(404)]
    // public async Task<IActionResult> ChangeState(long entityId, bool state)
    // {
    //     if (entityId < 1)
    //         return NotFound();
    //     return Ok(await _mediator.Send(new ScalendarChangeState.Command(entityId, state)));
    // }

    // /// <summary>
    // ///     Delete a Scalendar by Id
    // /// </summary>
    // /// <param name="entityId">Scalendar Id</param>
    // /// <returns>Return true or false</returns>
    // [HttpDelete]
    // [Route("{entityId}")]
    // // [ProducesResponseType(typeof(ScalendarDelete.Response), 200)]
    // public async Task<IActionResult> Delete(long entityId)
    // {
    //     return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new ScalendarDelete.Command(entityId)));
    // }

    // /// <summary>
    // ///     Delete list of Scalendar
    // /// </summary>
    // /// <param name="ids">List of ids of Scalendar</param>
    // [HttpPost]
    // [Route("[action]")]
    // // [ProducesResponseType(typeof(ScalendarDeleteRange.Response), 200)]
    // public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    // {
    //     await _mediator.Send(new ScalendarDeleteRange.Command(ids));
    //     return Ok();
    // }
}