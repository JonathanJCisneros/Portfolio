using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioV2.Repository
{
    public class InquiryRepository : IInquiryRepository
    {
        public async Task<bool> CheckById(string id)
        {
            throw new NotImplementedException();
        }

        public  async Task<Inquiry> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inquiry>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Create(Inquiry entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Update(Inquiry entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
