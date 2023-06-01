using PortfolioV2.Core;

namespace PortfolioV2.Service.Interfaces
{
    public interface IInquiryService
    {
        Task<Inquiry?> Get(string id);

        Task<List<Inquiry>> GetAll();

        Task<string?> Create(Inquiry entity);

        Task<bool> Resolve(string id);

        Task<bool> Delete(string id);
    }
}
