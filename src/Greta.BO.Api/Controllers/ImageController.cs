using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Handlers.Command.Image;
using Greta.BO.BusinessLogic.Handlers.Queries.Image;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ImageController : BaseController
{
    private readonly IMediator _mediator;

    public ImageController(
        IMediator mediator,
        ILogger<ImageController> logger
        , IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     List all image for one entity
    /// </summary>
    /// <param name="imageType">Type</param>
    /// <param name="ownerId">Entity id </param>
    /// <returns></returns>
    [HttpGet]
    [Route("{imageType}/{ownerId}")]
    [ProducesResponseType(typeof(List<ImageModel>), 200)]
    public async Task<IActionResult> Get(ImageType imageType, long ownerId)
    {
        return Ok(await _mediator.Send(new ImageGetAll.Query(ownerId, imageType)));
    }


    /// <summary>
    ///     Upload image to a entity
    /// </summary>
    /// <param name="isPrimary">Is primary</param>
    /// <param name="imageType">Type</param>
    /// <param name="ownerId">Product id </param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]/{isPrimary}/{imageType}/{ownerId}")]
    [DisableRequestSizeLimit]
    [ProducesResponseType(typeof(Image), 200)]
    public async Task<IActionResult> UploadImage(int isPrimary, ImageType imageType, long ownerId)
    {
        try
        {
            var file = HttpContext.Request.Form.Files[0];
            return Ok(await _mediator.Send(new ImageAdd.Command(ownerId, isPrimary, imageType, file)));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>
    ///     Delete image of a product
    /// </summary>
    /// <param name="imageId">Image id </param>
    /// <returns></returns>
    [HttpDelete]
    [Route("[action]/{imageId}")]
    public async Task<IActionResult> RemoveImage(long imageId)
    {
        return Ok(await _mediator.Send(new ImageDelete.Command(imageId)));
    }
}