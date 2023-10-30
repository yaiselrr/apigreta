using System;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Return optionable element Cashiers or Register in sales 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> GetCloseableElements([FromBody] GetCloseableElementRequestModel data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        return Ok(await _mediator.Send(new GetCloseableElementsByDateAndStore.Query(data)));
    }

    /// <summary>
    /// Return all data necesary for fill amount of bills and coins
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> ProcessEndOfDay([FromBody] ProcessEndOfDayRequest data)
    {
        if (data == null) throw new BusinessLogicException($"Please send data again.");
        return Ok(await _mediator.Send(new ProcessEndOfDay.Query(data)));
    }

    /// <summary>
    /// Return optionable element Chashiers or Register in sales 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> GetEndOfDayResume([FromBody] GetEndOfDayresumeRequest data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        return Ok(await _mediator.Send(new GetEndOfDayResume.Query(data)));
    }
}