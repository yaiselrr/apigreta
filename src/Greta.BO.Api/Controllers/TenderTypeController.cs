using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.TenderType;
using Greta.BO.BusinessLogic.Handlers.Queries.TenderType;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class TenderTypeController : BaseController
{
    private readonly IMediator _mediator;

    public TenderTypeController(IMediator mediator, IMapper mapper) :
        base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get filter and paginate TenderType
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated TenderType</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(TenderTypeFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] TenderTypeSearchDto filter)
    {
        return Ok(await _mediator.Send(new TenderTypeFilter.Query(currentPage, pageSize,
            _mapper.Map<TenderTypeSearchModel>(filter))));
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
        var data = await _mediator.Send(new TenderTypeGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new TenderType
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] TenderTypeModel entitydto)
    {
        return Ok(await _mediator.Send(new TenderTypeCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">TenderType Id</param>
    /// <param name="entitydto">TenderType data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(TenderTypeUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] TenderTypeModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new TenderTypeUpdate.Command(entityId,
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
        return Ok(await _mediator.Send(new TenderTypeChangeState.Command(entityId, state)));
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
    public async Task<IActionResult> ChangeCashDiscount(long entityId, bool state)
    {
        if (entityId < 1)
            return NotFound();
        return Ok(await _mediator.Send(new TenderTypeChangeCashDiscunt.Command(entityId, state)));
    }

    // /// <summary>
    // ///     Delete a TenderType by Id
    // /// </summary>
    // /// <param name="entityId">TenderType Id</param>
    // /// <returns>Return true or false</returns>
    // [HttpDelete]
    // [Route("{entityId}")]
    // // [ProducesResponseType(typeof(TenderTypeDelete.Response), 200)]
    // public async Task<IActionResult> Delete(long entityId)
    // {
    //     return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new TenderTypeDelete.Command(entityId)));
    // }
    //
    // /// <summary>
    // ///     Delete list of TenderType
    // /// </summary>
    // /// <param name="ids">List of ids of TenderType</param>
    // [HttpPost]
    // [Route("[action]")]
    // // [ProducesResponseType(typeof(TenderTypeDeleteRange.Response), 200)]
    // public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    // {
    //     await _mediator.Send(new TenderTypeDeleteRange.Command(ids));
    //     return Ok();
    // }
}