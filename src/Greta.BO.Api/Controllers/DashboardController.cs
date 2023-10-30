using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator, IMapper mapper) : base(mapper)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Get all the Entities by store id
        /// </summary>
        /// <returns>Return list of entities</returns>
        [HttpGet]
        [Route("[action]/{storeId}")]
        public async Task<IActionResult> GetSalesByHour(long storeId)
        {
            return Ok(await _mediator.Send(new GetSalesByHourQuery(storeId)));
        }

        /// <summary>
        ///    Get sales by hour stream
        /// </summary>
        /// <returns>Return list of entities</returns>
        [HttpGet]
        [Route("[action]/{storeId}")]
        public IAsyncEnumerable<GetSalesByHourResponse> GetSalesByHourStream([FromRoute] long storeId,
            CancellationToken cancellationToken)
        {
            var streamRequest = new GetSalesByHourStream(storeId);
            return _mediator.CreateStream(streamRequest, cancellationToken);
        }

        /// <summary>
        ///     Get all the Entities by store id
        /// </summary>
        /// <returns>Return list of entities</returns>
        [HttpGet]
        [Route("[action]/{storeId}")]
        public async Task<IActionResult> GetSalesByWeek(long storeId)
        {
            return Ok(await _mediator.Send(new GetSalesByWeek.Query(storeId)));
        }
    }
}