using Microsoft.AspNetCore.Identity;
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Service.Interfaces;

namespace PortfolioV2.Service
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion Fields

        #region Constructors

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods

        public async Task<User?> CheckByEmail(string email)
        {
            return await _userRepository.CheckByEmail(email);
        }

        public async Task<AuthorizeResult> Authorize(string email, string password)
        {
            User? user = await CheckByEmail(email);            
            
            if (user == null)
            {
                return new();
            }

            PasswordHasher<User> hashBrowns = new();
            PasswordVerificationResult pwCheck = hashBrowns.VerifyHashedPassword(user, user.Password, password);

            if (pwCheck == 0)
            {
                return new();
            }

            AuthorizeResult result = new()
            {
                Id = user.Id.ToString(),
                Name = user.FirstName,
                Email = user.Email
            };            

            await _userRepository.Login(user.Id);

            return result;
        }

        public async Task<bool> Create(User user)
        {
            PasswordHasher<User> hashBrowns = new();
            user.Password = hashBrowns.HashPassword(user, user.Password);            

            return await _userRepository.Create(user);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _userRepository.Delete(id);
        }

        #endregion Public Methods
    }
}