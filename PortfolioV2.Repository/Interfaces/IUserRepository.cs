using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IUserRepository : IBaseInterface<User>
    {
        Task<User> CheckByEmail(string email);
    }
}
