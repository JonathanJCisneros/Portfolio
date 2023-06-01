using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IInquiryRepository 
    {
        Task<Inquiry?> Get(string id);

        Task<List<Inquiry>> GetAll();

        Task<string?> Create(Inquiry inquiry);

        Task<bool> Resolve(string id);

        Task<bool> Delete(string id);
    }
}
