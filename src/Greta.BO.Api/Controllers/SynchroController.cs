using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Handlers.Queries.Synchro;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Hangfire.MediatR;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class SynchroController : BaseController
{
    private readonly IMediator _mediator;

    public SynchroController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get filter and paginate Region
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated Syncro detail</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] SynchroSearchDto filter)
    {
        return Ok(await _mediator.Send(new SynchroFilter.Query(currentPage, pageSize,
            _mapper.Map<SynchroSearchModel>(filter))));
    }

    /// <summary>
    ///     Get Synchro detail
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>Returns the paginated Region</returns>
    [HttpGet]
    [Route("{storeId}")]
    // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
    public async Task<IActionResult> GetSyncroDetail(long storeId)
    {
        return Ok(await _mediator.Send(new SynchroGetByStoreId.Query(storeId)));
    }

    /// <summary>
    ///     Close one synchro
    /// </summary>
    /// <param name="entityId">Synchro Id</param>
    /// <returns>Return true or false</returns>
    [HttpPost]
    [Route("[action]/{entityId}")]
    // [ProducesResponseType(typeof(RegionDelete.Response), 200)]
    public IActionResult CloseSynchro(long entityId)
    {
        if (entityId < 1) return NotFound();

        _mediator.EnqueueNew("CloseSynchro", new SynchroCloseCommand(entityId));
        return Ok();
    }

    /// <summary>
    ///    Get SynchroStatistics for one store 
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <returns>Return true or false</returns>
    [HttpGet]
    [Route("[action]/{storeId}")]
    [ProducesResponseType(typeof(SynchroStatistics), 200)]
    public async Task<IActionResult> GetSynchroStatistics(long storeId)
    {
        return Ok(await _mediator.Send(new SynchroResumeByStoreId.Query(storeId)));
    }
}