using NQWSZUS.Services;

namespace NQWSZUS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
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

            /// Επιστρέφει true αν η υπηρεσία είναι ενεργή
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

            /// Ενεργοποιεί/Απενεργοποιεί την υπηρεσία
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
