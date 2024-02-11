using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IInquiryRepository 
    {
        Task<Inquiry?> Get(Guid id);

        Task<List<Inquiry>> GetAll();

        Task<bool> Create(Inquiry inquiry);

        Task<bool> Resolve(Guid id);

        Task<bool> Delete(Guid id);
    }
}