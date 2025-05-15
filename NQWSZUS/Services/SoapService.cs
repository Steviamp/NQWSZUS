
using ServiceReference;

namespace NQWSZUS.Services
{
    public class SoapService : ISoapService
    {
        private readonly NQWSZUSSoapClient _client;
        private readonly ILogger<SoapService> _logger;

        public SoapService(ILogger<SoapService> logger)
        {
            _client = new NQWSZUSSoapClient(NQWSZUSSoapClient.EndpointConfiguration.NQWSZUSSoap);
            _logger = logger;
        }
        public async Task<bool> IsServiceTypeStatusActiveAsync(int serviceType, string host, int port)
        {
            try
            {
                _logger.LogInformation("Calling IsServiceTypeStatusActiveAsync(host={Host}, port={Port})", host, port);
                var response = await _client.IsServiceTypeStatusActiveAsync(serviceType, host, port);
                return response.Body?.IsServiceTypeStatusActiveResult ?? false;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "IsServiceTypeStatusActiveAsync failed for host={Host}, port={Port}", host, port);
                throw;
            }
        }

        public async Task<bool> ActivateServiceTypeAsync(int serviceType, bool status, string host, int port)
        {
            try
            {
                _logger.LogDebug("Checking status for serviceType={Type}", serviceType);
                var response = await _client.ActivateServiceTypeAsync(serviceType, status, host, port);
                return response.Body?.ActivateServiceTypeResult ?? false;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "ActivateServiceTypeAsync failed for serviceType={Type}", serviceType);
                throw;
            }
        }

        public async Task<List<ServiceType>> GetServiceTypeListAsync(string host, int port)
        {
            try
            {
                _logger.LogInformation("Calling GetServiceTypeListAsync(host={Host}, port={Port})", host, port);
                var response = await _client.GetServiceTypeListAsync(host, port);
                return response.Body?.GetServiceTypeListResult?.ToList()
                        ?? new List<ServiceType>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetServiceTypeListAsync failed for host={Host}, port={Port}", host, port);
                throw;
            }
        }
    }
}
