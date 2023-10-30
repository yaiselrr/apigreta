using System;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Hubs;
using Greta.Sdk.Hangfire.MediatR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace Greta.BO.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<CloudHub, ICloudHub> hub;
        private readonly IMediator _mediator;

        // private readonly IEmailSender eSender;
        public TestController(IHubContext<CloudHub, ICloudHub> hub, IMediator mediator) //, IEmailSender eSender)
        {
            _mediator = mediator;
            this.hub = hub;
            // this.eSender = eSender;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Index(string connection)
        { 
            return Ok("ok");
        }
        
        /// <summary>
        ///     sssss
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/fullbackup/{connection}")]
        [AllowAnonymous]
        public IActionResult Fullbackup(string connection)
        { 
            // _mediator.Send(
            //     //"FromTestingFullBackup", 
            //     new SynchroFullBackup.Command(Guid.Parse("68e02f35-af24-4a88-9b06-4884e7470cf1"),"oFMUh4uv4ekwZxvWEP7n0g"));
            
            _mediator.EnqueueNew(
                //"FromTestingFullBackup", 
                new SynchroFullBackupCommand(Guid.Parse("68e02f35-af24-4a88-9b06-4884e7470cf1"),connection));

            return Ok();
        }

        /// <summary>
        ///     sssss
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/hub")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            await hub.Clients.All.TestPrint("zpl",
                "^XA" +
                "^LH0,0" +
                "^CI28" +
                "^XZ"
            );

            return Ok();
        }
        
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> TestUpdateTag()
        {
            await _mediator.Send(
                //"FromTestingFullBackup", 
                new DeviceUpdateTagVersionCommand(
                    "UwB5AHMAdABlAG0ALgBSAGEAbgBkAG8AbQAxADcAOABCAEYAQgBGAEYAMAAwADgANgAwAEYAOAAxAEEAOAA5ADMANABBADMAQgA5AEIAMgA3AEIAQwA4ADcAMABGAEQAOABTAHkAcwB0AGUAbQAuAFIAYQBuAGQAbwBtAA==",
                    1));
            return Ok();
        }
        
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> TestSendVendorOrder()
        {
            await _mediator.Send(
                //"FromTestingFullBackup", 
                new VendorOrderSendEmailCommand(6));
            return Ok();
        }

        // [HttpGet]
        // [Route("[action]")]
        // public async Task<IActionResult> Email()
        // {

        //     await eSender.SendEmailAsync("chenryhabana205@gmail.com", "test", "testing email sendgrid");

        //     return Ok();
        // }

        // [HttpPost]
        // [Route("[action]")]
        // public async Task<IActionResult> Upload()
        // {
        //     var data = HttpContext.Request.Form.Files;

        //     foreach (IFormFile f in data)
        //     {
        //         await storage.UploadImage("test", f);
        //     }

        //     return Ok();
        // }
    }
}