using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Service.Interfaces;

namespace PortfolioV2.Service
{
    public class IPAddressService : IIPAddressService
    {
        #region Fields

        private readonly IIPAddressRepository _IPAddressRepository;

        #endregion Fields

        #region Constructors

        public IPAddressService(IIPAddressRepository IPAddressRepository)
        {
            _IPAddressRepository = IPAddressRepository;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods

        public async Task<bool> IsValidIPAddress(string ipAddress)
        {
            List<string> ipAddresses = await _IPAddressRepository.GetIPAddresses();

            return ipAddresses.Contains(ipAddress);
        }

        #endregion Public Methods
    }
}