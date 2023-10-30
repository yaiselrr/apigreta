using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Command.Download;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Auth;
using Greta.BO.BusinessLogic.Models.Dto;

namespace Greta.BO.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DownloadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="hash">base64path</param>
        /// <returns>File</returns>
        [HttpGet, DisableRequestSizeLimit]
        [Route("[action]/{hash}")]
        public async Task<IActionResult> DownloadFile(string hash)
        {
            var data = await _mediator.Send(new DownloadFile.Query(hash));
            //var memory = new MemoryStream();
            //await memory.WriteAsync(data);
            //memory.Position = 0;
            //return File(memory, "application/octet-stream");
            var base64 = Convert.ToBase64String(data);// memory.ToArray());

            return Ok(base64);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]/{hash}")]
        public async Task<IActionResult> DownloadFileWPass(string hash, [FromBody] DownloadPasswordModel model)
        {
            var data = await _mediator.Send(new DownloadFile.Query(hash, model.Password));
            //var memory = new MemoryStream();
            //await memory.WriteAsync(data);
            //memory.Position = 0;
            //return File(memory, "application/octet-stream");
            var base64 = Convert.ToBase64String(data);// memory.ToArray());

            return Ok(base64);
        }
        
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<IActionResult> GetHideLinkAclassVideo([FromBody] DownloadPasswordModel model)
        {
            var checkPass = await _mediator.Send(new CheckAdminPasswordCommand(model.Password ));
            if (checkPass.Data)
            {
                return Ok(new CQRSResponse<string>() { Data = "https://app.screencast.com/Jgpa0eHhBiDqx/e" });
            }

            throw new BusinessLogicException("Access Denied.");
        }
    }
}
