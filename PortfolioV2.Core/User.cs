#pragma warning disable CS8618


namespace PortfolioV2.Core
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LastLoggedIn { get; set; }
    }
}