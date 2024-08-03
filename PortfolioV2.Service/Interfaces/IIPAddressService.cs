namespace PortfolioV2.Service.Interfaces
{
    public interface IIPAddressService
    {
        Task<bool> IsValidIPAddress(string ipAddress);
    }
}