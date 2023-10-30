using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelDefinition;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelDefinition;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

    [Route("api/[controller]")]
    public class ScaleLabelDefinitionController : BaseController
    {
        private readonly IMediator _mediator;

        public ScaleLabelDefinitionController(IMediator mediator,
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new ScaleLabelDefinitionGetAll.Query()));
        }

        /// <summary>
        ///     Get filter and paginate Region
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter Object</param>
        /// <returns>Returns the paginated Region</returns>
        [HttpPost]
        [Route("{currentPage}/{pageSize}")]
        // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
        public async Task<IActionResult> Filter(int currentPage, int pageSize,
            [FromBody] ScaleLabelDefinitionSearchModel filter)
            =>Ok(await _mediator.Send(new ScaleLabelDefinitionFilterQuery(currentPage, pageSize, filter)));


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
            var data = await _mediator.Send(new ScaleLabelDefinitionGetById.Query(entityId));
            return data != null ? Ok(data) : NotFound();
        }

        /// <summary>
        ///     Create a new Region
        /// </summary>
        /// <param name="entitydto">Entity to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
        public async Task<IActionResult> Post([FromBody] ScaleLabelDefinitionModel entitydto)
        {
            return Ok(await _mediator.Send(
                new ScaleLabelDefinitionCreate.Command((entitydto))));
        }

        /// <summary>
        ///     Update a profile by Id
        /// </summary>
        /// <param name="entityId">ScaleLabelDefinition Id</param>
        /// <param name="entitydto">ScaleLabelDefinition data to update</param>
        /// <returns>Return true or false</returns>
        [HttpPut]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(RegionUpdate.Response), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] ScaleLabelDefinitionModel entitydto)
        {
            return entityId < 1
                ? NotFound()
                : Ok(await _mediator.Send(new ScaleLabelDefinitionUpdate.Command(entityId,
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
            return Ok(await _mediator.Send(new ScaleLabelDefinitionChangeState.Command(entityId, state)));
        }

        /// <summary>
        ///     Delete a Region by Id
        /// </summary>
        /// <param name="entityId">Region Id</param>
        /// <returns>Return true or false</returns>
        [HttpDelete]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(RegionDelete.Response), 200)]
        public async Task<IActionResult> Delete(long entityId)
        {
            return entityId < 1
                ? NotFound()
                : Ok(await _mediator.Send(new ScaleLabelDefinitionDelete.Command(entityId)));
        }

        /// <summary>
        ///     Delete list of Region
        /// </summary>
        /// <param name="ids">List of ids of Region</param>
        [HttpPost]
        [Route("[action]")]
        // [ProducesResponseType(typeof(RegionDeleteRange.Response), 200)]
        public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
        {
            await _mediator.Send(new ScaleLabelDefinitionDeleteRange.Command(ids));
            return Ok();
        }
    }
