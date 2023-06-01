using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> CheckByEmail(string email);

        Task Login(string id);

        Task<string?> Create(User user);

        Task<bool> Delete(string id);
    }
}
