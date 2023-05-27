using PortfolioV2.Core;

namespace PortfolioV2.Service.Interfaces
{
    public interface IUserService : IInterface<User>
    {
        Task<User?> CheckByEmail(string email);

        Task<AuthorizeResult> Authorize(string email, string password);
    }
}
