using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Service.Interfaces;

namespace PortfolioV2.Service
{
    public class InquiryService : IInquiryService
    {
        #region Fields

        private readonly IInquiryRepository _inquiryRepository;

        #endregion Fields

        #region Constructors

        public InquiryService(IInquiryRepository inquiryRepository)
        {
            _inquiryRepository = inquiryRepository;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods

        public async Task<Inquiry?> Get(Guid id)
        {
            return await _inquiryRepository.Get(id);
        }

        public async Task<List<Inquiry>> GetAll()
        {
            List<Inquiry> inquiries = await _inquiryRepository.GetAll();

            return inquiries.OrderByDescending(x => x.Status)
                            .ThenBy(x => x.CreatedDate)
                            .ToList();
        }

        public async Task<bool> Create(Inquiry inquiry)
        {
            return await _inquiryRepository.Create(inquiry);
        }

        public async Task<bool> Resolve(Guid id)
        {
            return await _inquiryRepository.Resolve(id);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _inquiryRepository.Delete(id);
        }

        #endregion Public Methods
    }
}