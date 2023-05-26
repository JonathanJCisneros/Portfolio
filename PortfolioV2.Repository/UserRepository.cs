using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;

namespace PortfolioV2.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMySqlRepository _db;

        public UserRepository(IMySqlRepository db)
        {
            _db = db;
        }        

        public async Task<bool> CheckById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CheckByEmail(string email)
        {
            string query = "SELECT * FROM users WHERE Email = @email;";

            Dictionary<string, object> parameters = new()
            {
                { "@email", email }
            };

            List<object> users = await _db.ExecuteQuery(query, parameters);

            if(users.Count == 0)
            {
                return null;
            }

            User user = new()
            {

            };

            return user;
        }

        public async Task<User> Get(string id)
        {
            string query = "SELECT * FROM users WHERE Id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            };

            List<object> users = await _db.ExecuteQuery(query, parameters);

            return new();
        }

        public async Task<List<User>> GetAll()
        {
            string query = "SELECT * FROM users";

            List<object> users = await _db.ExecuteQuery(query, null);

            return new();
        }

        public async Task<string> Create(User user)
        {
            string query = "INSERT INTO users(Id, FirstName, LastName, Email, Password, CreatedDate, UpdatedDate, LastLoggedIn) VALUES(@id, @firstname, @lastname, @email, @password, @createddate, @updateddate, @lastloggedin)";

            Dictionary<string, object> parameters = new()
            {
                { "@id", user.Id.ToString() },
                { "@firstname", user.FirstName },
                { "@lastname", user.LastName },
                { "@email", user.Email },
                { "@password", user.Password },
                { "@createddate", user.CreatedDate },
                { "@updateddate", user.UpdatedDate },
                { "@lastloggedin", user.LastLoggedIn }
            };

            string id = await _db.ExecuteNonQuery(query, parameters);

            return id;
        }

        public async Task<string> Update(User user)
        {
            string query = "UPDATE users SET FirstName = @firstname, LastName = @lastname, Email = @email, UpdatedDate = @updateddate WHERE Id = @id";

            Dictionary<string, object> parameters = new()
            {
                { "@firstname", user.FirstName },
                { "@lastname", user.LastName },
                { "@email", user.Email },
                { "@updateddate", user.UpdatedDate },
                { "@id", user.Id.ToString() }
            };

            string id = await _db.ExecuteNonQuery(query, parameters);

            return id;
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
