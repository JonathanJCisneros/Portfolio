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

        public async Task<User?> CheckByEmail(string email)
        {
            return await _userRepository.CheckByEmail(email);
        }

        public async Task<AuthorizeResult> Authorize(string email, string password)
        {
            User? user = await CheckByEmail(email);

            AuthorizeResult result = new();
            
            if (user == null)
            {
                return result;
            }

            PasswordHasher<User> hashBrowns = new();
            PasswordVerificationResult pwCheck = hashBrowns.VerifyHashedPassword(user, user.Password, password);

            if (pwCheck == 0)
            {
                return result;
            }

            result.Id = user.Id.ToString();
            result.Name = user.FirstName;
            result.Email = user.Email;

            await _userRepository.Login(user.Id.ToString());

            return result;
        }

        public async Task<string?> Create(User user)
        {
            PasswordHasher<User> hashBrowns = new();
            user.Password = hashBrowns.HashPassword(user, user.Password);            

            return await _userRepository.Create(user);;
        }

        public async Task<bool> Delete(string id)
        {
            return await _userRepository.Delete(id);
        }
    }
}
