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
        private readonly ILogger<ServiceTypeController> _logger;
        public ServiceTypeController(ISoapService soapService, ILogger<ServiceTypeController> logger)
        {
            _soapService = soapService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceType>>> GetAll(
        [FromQuery] string host,
        [FromQuery] int port)
        {
            try
            {
                var list = await _soapService.GetServiceTypeListAsync(host, port);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to GET /api/servicetype?host={Host}&port={Port}", host, port);
                return StatusCode(500, "Unable to load service types");
            }
        }
    }
}

