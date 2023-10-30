using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto.Search;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleHomeFav;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleHomeFav;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;
    [Route("api/[controller]")]
    public class ScaleHomeFavController : BaseController
    {
        private readonly IMediator _mediator;

        public ScaleHomeFavController(IMediator mediator, IMapper mapper) :
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
            return Ok(await _mediator.Send(new ScaleHomeFavGetAll.Query()));
        }

        /// <summary>
        ///     Get filter and paginate ScaleHomeFav
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter Object</param>
        /// <returns>Returns the paginated ScaleHomeFav</returns>
        [HttpPost]
        [Route("{currentPage}/{pageSize}")]
        // [ProducesResponseType(typeof(ScaleHomeFavFilter.Response), 200)]
        public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] ScaleHomeFavSearchDto filter)
        {
            return Ok(await _mediator.Send(new ScaleHomeFavFilter.Query(currentPage, pageSize,
                _mapper.Map<ScaleHomeFavSearchModel>(filter))));
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
            var data = await _mediator.Send(new ScaleHomeFavGetById.Query(entityId));
            return data != null ? Ok(data) : NotFound();
        }

        /// <summary>
        ///     Create a new ScaleHomeFav
        /// </summary>
        /// <param name="entitydto">Entity to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        // [ProducesResponseType(typeof(ProfilesCreate.Response), 200)]
        public async Task<IActionResult> Post([FromBody] ScaleHomeFavModel entitydto)
        {
            return Ok(await _mediator.Send(new ScaleHomeFavCreate.Command(entitydto)));
        }

        /// <summary>
        ///     Update a profile by Id
        /// </summary>
        /// <param name="entityId">ScaleHomeFav Id</param>
        /// <param name="entitydto">ScaleHomeFav data to update</param>
        /// <returns>Return true or false</returns>
        [HttpPut]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(ScaleHomeFavUpdate.Response), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] ScaleHomeFavModel entitydto)
        {
            return entityId < 1
                ? NotFound()
                : Ok(await _mediator.Send(new ScaleHomeFavUpdate.Command(entityId,
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
            return Ok(await _mediator.Send(new ScaleHomeFavChangeState.Command(entityId, state)));
        }

        /// <summary>
        ///     Delete a ScaleHomeFav by Id
        /// </summary>
        /// <param name="entityId">ScaleHomeFav Id</param>
        /// <returns>Return true or false</returns>
        [HttpDelete]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(ScaleHomeFavDelete.Response), 200)]
        public async Task<IActionResult> Delete(long entityId)
        {
            return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new ScaleHomeFavDelete.Command(entityId)));
        }

        /// <summary>
        ///     Delete list of ScaleHomeFav
        /// </summary>
        /// <param name="ids">List of ids of ScaleHomeFav</param>
        [HttpPost]
        [Route("[action]")]
        // [ProducesResponseType(typeof(ScaleHomeFavDeleteRange.Response), 200)]
        public async Task<IActionResult> DeleteRange([FromBody] List<long> ids)
        {
            await _mediator.Send(new ScaleHomeFavDeleteRange.Command(ids));
            return Ok();
        }
    }