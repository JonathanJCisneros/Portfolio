using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Service.Interfaces;

namespace PortfolioV2.Service
{
    public class InquiryService : IInquiryService
    {
        private readonly IInquiryRepository _inquiryRepository;

        public InquiryService(IInquiryRepository inquiryRepository)
        {
            _inquiryRepository = inquiryRepository;
        }


        public async Task<bool> CheckById(string id)
        {
            return await _inquiryRepository.CheckById(id);
        }

        public async Task<Inquiry?> Get(string id)
        {
            return await _inquiryRepository.Get(id);
        }

        public async Task<List<Inquiry>> GetAll()
        {
            return await _inquiryRepository.GetAll();
        }

        public async Task<string?> Create(Inquiry inquiry)
        {
            return await _inquiryRepository.Create(inquiry);
        }

        public async Task<string?> Update(Inquiry inquiry)
        {
            return await _inquiryRepository.Update(inquiry);
        }

        public async Task<bool> Delete(string id)
        {
            return await _inquiryRepository.Delete(id);
        }
    }
}
