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
            List<Inquiry> inquiries = await _inquiryRepository.GetAll();

            List<Inquiry> newList = inquiries.Where(x => x.Status == "Unresolved").ToList();

            newList.AddRange(inquiries.Where(x => x.Status == "Resolved").OrderBy(x => x.UpdatedDate).ToList());

            return newList;
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
