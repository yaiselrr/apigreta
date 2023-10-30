using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Queries.ClientApplication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ClientApplicationController : BaseController
{
    private readonly IMediator _mediator;

    public ClientApplicationController(IMediator mediator,
        IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("")]
    // [ProducesResponseType(typeof(ClientApplicationGetAll.Response), 200)]
    public virtual async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new ClientApplicationGetAll.Query()));
    }

    /// <summary>
    ///     Get Entity by id
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{entityId}")]
    [ProducesResponseType(404)]
    // [ProducesResponseType(typeof(ClientApplicationById.Response), 200)]
    public async Task<IActionResult> Get(long entityId)
    {
        var data = await _mediator.Send(new ClientApplicationById.Query(entityId));
        return data == null ? NotFound() : Ok(data);
    }
}