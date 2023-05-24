using Microsoft.AspNetCore.Identity;
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Service.Interfaces;

namespace PortfolioV2.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CheckByEmail(string email)
        {
            User user = await _userRepository.CheckByEmail(email);

            return user;
        }

        public async Task<AuthorizeResult> Authorize(string email, string password)
        {
            User user = await CheckByEmail(email);

            AuthorizeResult result = new();
            
            if (user == null)
            {
                return result;
            }

            PasswordHasher<User> hashBrowns = new();
            PasswordVerificationResult pwCheck = hashBrowns.VerifyHashedPassword(user, password, user.Password);

            if (pwCheck == 0)
            {
                return result;
            }

            result.Id = user.Id.ToString();
            result.Name = user.FirstName;
            result.Email = user.Email;

            return result;
        }

        public async Task<User> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Create(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Update(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

    }
}
