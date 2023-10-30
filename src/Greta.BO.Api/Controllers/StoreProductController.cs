using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;
using Greta.BO.BusinessLogic.Handlers.Queries.StoreProduct;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;
    public class StoreProductController
        : BaseController
    {
        private readonly IMediator _mediator;

        public StoreProductController(IMediator mediator, IMapper mapper) : base(mapper)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        ///     Get Entity by id
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{entityId}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(long entityId)
        {
            var data = await _mediator.Send(new StoreProductGetById.Query(entityId));
            return data != null ? Ok(data) : NotFound();
        }
            
        /// <summary>
        ///     Get Entity by id
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{entityId}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetElementParent(long entityId)
        {
            var data = await _mediator.Send(new StoreProductParentGetById.Query(entityId));
            return data != null ? Ok(data) : NotFound();
        }

        /// <summary>
        ///     Get filter and paginate Region
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter Object</param>
        /// <returns>Returns the paginated Region</returns>
        [HttpPost]
        [Route("{productId}/{currentPage}/{pageSize}")]
        // [ProducesResponseType(typeof(RegionFilter.Response), 200)]
        public async Task<IActionResult> Filter(long productId, int currentPage, int pageSize,
            [FromBody] StoreProductSearchModel filter)
        {
            return Ok(await _mediator.Send(new StoreProductFilter.Query(productId, currentPage, pageSize, filter)));
        }

        /// <summary>
        ///     Create a new Entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] StoreProductCreateModel entity)
        {
            return Ok(
                await _mediator.Send(new StoreProductCreateCommand(entity)));
        }

        /// <summary>
        ///     Update a Store Product
        /// </summary>
        /// <param name="entityId">Store Product Id</param>
        /// <param name="entitydto">Store Product data to update</param>
        /// <returns>Return true or false</returns>
        [HttpPut]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(RegionUpdate.Response), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProduct(long entityId, [FromBody] StoreProductModel entitydto)
        {
            return entityId < 1
                ? NotFound()
                : Ok(await _mediator.Send(new StoreProductUpdateCommand(entityId,
                    entitydto)));
        }


        /// <summary>
        ///     Delete a Store Product by Id
        /// </summary>
        /// <param name="entityId">Region Id</param>
        /// <returns>Return true or false</returns>
        [HttpDelete]
        [Route("{entityId}")]
        // [ProducesResponseType(typeof(RegionDelete.Response), 200)]
        public async Task<IActionResult> Delete(long entityId)
        {
            return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new StoreProductDelete.Command(entityId)));
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
            await _mediator.Send(new StoreProductDeleteRange.Command(ids));
            return Ok();
        }

        /// <summary>
        ///     Set Parent To Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Return true or false</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SetParentToProduct([FromBody]SetParentProductModel model)
        {
            return Ok(await _mediator.Send(new SetParentProduct.Command(model)));
        }
        
        /// <summary>
        /// Get StoreProduct by store and UPC
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="upc">Product Upc</param>
        /// <returns>Return true or false</returns>
        [HttpGet]
        [Route("[action]/{storeId}/{upc}")]
        public async Task<IActionResult> GetStoreProductByUPC(long storeId, string upc)
            => Ok(await _mediator.Send(new GetStoreProductByUPCAndStore.Query(storeId, upc)));

        
        /// <summary>
        ///     Delete a Store Product Parent by id
        /// </summary>
        /// <param name="entityId">child Id</param>
        /// <returns>Return true or false</returns>
        [HttpDelete]
        [Route("[action]/{entityId}")]
        public async Task<IActionResult> DeleteParent(long entityId)
        {
            return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new DeleteParent.Command(entityId)));
        }
        
    }