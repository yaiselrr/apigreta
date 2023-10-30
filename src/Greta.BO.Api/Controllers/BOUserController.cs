using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Dto.Search;
using Greta.BO.Api.Responses;
using Greta.BO.BusinessLogic.Handlers.Command.BOUser;
using Greta.BO.BusinessLogic.Handlers.Queries.BOUser;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using Greta.BO.BusinessLogic.Handlers.Queries.User;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class BOUserController : BaseController
{
    private readonly IMediator _mediator;

    public BOUserController(IMediator mediator, IMapper mapper) : base(mapper)
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
        return Ok(await _mediator.Send(new BOUserGetAll.Query()));
    }

    /// <summary>
    ///     Get filter and paginate BOUser
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated BOUser</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(BOUserFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] BOUserSearchDto filter)
    {
        return Ok(await _mediator.Send(new BOUserFilterQuery(currentPage, pageSize,
            _mapper.Map<BOUserSearchModel>(filter))));
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
        var data = await _mediator.Send(new BOUserGetById.Query(entityId));
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    ///     Create a new BOUser
    /// </summary>
    /// <param name="entitydto">Entity to create</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
    public async Task<IActionResult> Post([FromBody] BOUserModel entitydto)
    {
        return Ok(await _mediator.Send(new BOUserCreate.Command(entitydto)));
    }

    /// <summary>
    ///     Update a profile by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <param name="entitydto">BOUser data to update</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(BOUserUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Put(long entityId, [FromBody] BOUserModel entitydto)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new BOUserUpdate.Command(entityId, entitydto)));
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
        return Ok(await _mediator.Send(new BOUserChangeState.Command(entityId, state)));
    }

    /// <summary>
    ///     Delete a BOUser by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(BOUserDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BOUserDelete.Command(entityId)));
    }

    /// <summary>
    ///     Delete list of BOUser
    /// </summary>
    /// <param name="ids">List of ids of BOUser</param>
    [HttpPost]
    [Route("[action]")]
    // [ProducesResponseType(typeof(BOUserDeleteRange.Response), 200)]
    public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
    {
        await _mediator.Send(new BOUserDeleteRange.Command(ids));
        return Ok();
    }

    /// <summary>
    ///     Email confirm by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(BOUserUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> EmailConfirm(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BOUserEmailConfirm.Command(entityId)));
    }

    /// <summary>
    ///     Phone confirm by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(BOUserUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PhoneConfirm(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BOUserPhoneConfirm.Command(entityId)));
    }

    /// <summary>
    ///     Lockout enable by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(BOUserUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> LockoutEnable(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BOUserLockoutEnable.Command(entityId)));
    }

    /// <summary>
    ///     Second Factor reset by Id
    /// </summary>
    /// <param name="entityId">BOUser Id</param>
    /// <returns>Return true or false</returns>
    [HttpPut]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(BOUserUpdate.Response), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> TwoFactorReset(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new BOUserSecondFactorReset.Command(entityId)));
    }

    /// <summary>
    ///     Get Entity by application id
    /// </summary>
    /// <param name="applicationId">Application Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]/{applicationId}")]
    [ProducesResponseType(404)]
    // [ProducesResponseType(typeof(ProfilesGetById.Response), 200)]
    public async Task<IActionResult> GetByApplication(long applicationId)
    {
        return Ok(await _mediator.Send(new ProfilesGetByApplicationQuery(applicationId)));
    }

    /// <summary>
    /// Reset Pin for POS access
    /// </summary>
    /// <param name="entitydto">User pin data</param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> ResetPasswordPin([FromBody] BOUserResetPinModel entitydto)
        => Ok(await _mediator.Send(new BoUserResetPinCommand(entitydto)));
}