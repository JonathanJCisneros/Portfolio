using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IUserRepository : IInterface<User>
    {
        Task<User?> CheckByEmail(string email);

        Task Login(string id);
    }
}
