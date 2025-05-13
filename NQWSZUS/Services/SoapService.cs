
using ServiceReference;

namespace NQWSZUS.Services
{
    public class SoapService : ISoapService
    {
        private readonly NQWSZUSSoapClient _client;

        public SoapService()
        {
            _client = new NQWSZUSSoapClient(NQWSZUSSoapClient.EndpointConfiguration.NQWSZUSSoap);
        }
        public async Task<bool> IsServiceTypeStatusActiveAsync(int serviceType, string host, int port)
        {
            var response = await _client.IsServiceTypeStatusActiveAsync(serviceType, host, port);
            return response.Body?.IsServiceTypeStatusActiveResult ?? false;
        }

        public async Task<bool> ActivateServiceTypeAsync(int serviceType, bool status, string host, int port)
        {
            var response = await _client.ActivateServiceTypeAsync(serviceType, status, host, port);
            return response.Body?.ActivateServiceTypeResult ?? false;
        }
    }
}
