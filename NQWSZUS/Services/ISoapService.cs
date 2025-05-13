
namespace NQWSZUS.Services
{
        public interface ISoapService
        {
            Task<bool> IsServiceTypeStatusActiveAsync(int serviceType, string host, int port);

            Task<bool> ActivateServiceTypeAsync(int serviceType, bool status, string host, int port);

            Task<List<ServiceReference.ServiceType>> GetServiceTypeListAsync(string host, int port);
        }
}
