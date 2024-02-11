using PortfolioV2.Core;

namespace PortfolioV2.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> CheckByEmail(string email);

        Task Login(Guid id);

        Task<bool> Create(User user);

        Task<bool> Delete(Guid id);
    }
}
