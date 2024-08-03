namespace PortfolioV2.Repository.Interfaces
{
    public interface IIPAddressRepository
    {
        Task<List<string>> GetIPAddresses();
    }
}