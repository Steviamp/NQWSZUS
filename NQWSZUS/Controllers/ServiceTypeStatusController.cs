using Microsoft.AspNetCore.Mvc;
using NQWSZUS.Services;

namespace NQWSZUS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypeStatusController : ControllerBase
    {
        private readonly ISoapService _soapService;
        private readonly ILogger<ServiceTypeStatusController> _logger;

        public ServiceTypeStatusController(ISoapService soapService, ILogger<ServiceTypeStatusController> logger)
        {
            _soapService = soapService;
            _logger = logger;
        }

        [HttpGet("{serviceType}")]
        public async Task<ActionResult<bool>> GetStatus(
            int serviceType,
            [FromQuery] string host,
            [FromQuery] int port
        )
        {
            try
            {
                var isActive = await _soapService.IsServiceTypeStatusActiveAsync(serviceType, host, port);
                return Ok(isActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET status failed for serviceType={Type}", serviceType);
                return StatusCode(500, "Unable to fetch service status");
            }
        }

        [HttpPost("{serviceType}")]
        public async Task<IActionResult> SetStatus(
            int serviceType,
            [FromQuery] bool status,
            [FromQuery] string host,
            [FromQuery] int port
        )
        {
            try
            {
                var success = await _soapService.ActivateServiceTypeAsync(serviceType, status, host, port);
                if (!success)
                    return StatusCode(500, "Failed to toggle service type status.");
                return Ok(new { serviceType, status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST toggle failed for serviceType={Type}", serviceType);
                return StatusCode(500, "Unable to update service status");
            }
        }
    }
}
