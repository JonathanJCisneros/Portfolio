using PortfolioV2.Core;

namespace PortfolioV2.Service.Interfaces
{
    public interface IUserService
    {
        Task<User?> CheckByEmail(string email);

        Task<AuthorizeResult> Authorize(string email, string password);

        Task<bool> Create(User user);

        Task<bool> Delete(Guid id);
    }
}