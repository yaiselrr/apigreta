using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers
{
    // [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [Authorize]
    
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}