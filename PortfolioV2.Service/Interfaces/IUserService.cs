using PortfolioV2.Core;

namespace PortfolioV2.Service.Interfaces
{
    public interface IUserService : IBaseInterface<User>
    {
        Task<User> CheckByEmail(string email);

        Task<AuthorizeResult> Authorize(string email, string password);
    }
}
