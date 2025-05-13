using NQWSZUS.Services;

namespace NQWSZUS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServiceReference;
    using System.Threading.Tasks;

    namespace NQWSReception.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ServiceTypeStatusController : ControllerBase
        {
            private readonly ISoapService _soapService;
            public ServiceTypeStatusController(ISoapService soapService)
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

            [HttpGet("{serviceType}")]
            public async Task<ActionResult<bool>> GetStatus(
                int serviceType,
                [FromQuery] string host,
                [FromQuery] int port
            )
            {
                var isActive = await _soapService.IsServiceTypeStatusActiveAsync(serviceType, host, port);
               return Ok(isActive);
            }

            [HttpPost("{serviceType}")]
            public async Task<IActionResult> SetStatus(
                int serviceType,
                [FromQuery] bool status,
                [FromQuery] string host,
                [FromQuery] int port
            )
            {
                var success = await _soapService.ActivateServiceTypeAsync(serviceType, status, host, port);
                if (!success)
                    return StatusCode(500, "Failed to toggle service type status.");
                return Ok(new { serviceType, status });
            }
        }
    }
}
