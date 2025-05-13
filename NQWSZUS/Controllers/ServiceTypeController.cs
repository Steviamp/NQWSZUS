using Microsoft.AspNetCore.Mvc;
using NQWSZUS.Services;
using ServiceReference;

namespace NQWSZUS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypeController : ControllerBase
    {
        private readonly ISoapService _soapService;
        public ServiceTypeController(ISoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceType>>> GetAll(
        [FromQuery] string host,
        [FromQuery] int port)
        {
            var list = await _soapService.GetServiceTypeListAsync(host, port);
            return Ok(list);
        }
    }
}

