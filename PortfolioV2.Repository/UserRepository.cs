#pragma warning disable CS8601
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMySqlRepository db;

        public UserRepository(IMySqlRepository mySqlRepository)
        {
            db = mySqlRepository;
        }        

        public async Task<User?> CheckByEmail(string email)
        {
            string query = "SELECT * FROM users WHERE email = @email;";

            Dictionary<string, object> parameters = new()
            {
                { "@email", email}
            };

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return dt.Rows.Count == 0 ? null : new()
            {
                Id = dt.Rows[0].Field<Guid>("id"),
                FirstName = dt.Rows[0].Field<string>("first_name"),
                LastName = dt.Rows[0].Field<string>("last_name"),
                Email = dt.Rows[0].Field<string>("email"),
                Password = dt.Rows[0].Field<string>("password")
            };
        }

        public async Task Login(string id)
        {
            string query = "UPDATE users SET last_logged_in = @time WHERE id = @id";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id },
                { "@time", DateTime.Now }
            };
            
            await db.ExecuteNonQuery(query, parameters);
        }

        public async Task<string?> Create(User user)
        {
            string query = "INSERT INTO users(id, first_name, last_name, email, password, created_date, updated_date, last_logged_in) VALUES(@id, @first_name, @last_name, @email, @password, @created_date, @updated_date, @last_logged_in);";

            Dictionary<string, object> parameters = new()
            {
                { "@id", user.Id.ToString() },
                { "@first_name", user.FirstName },
                { "@last_name", user.LastName },
                { "@email", user.Email },
                { "@password", user.Password },
                { "@created_date", user.CreatedDate },
                { "@updated_date", user.UpdatedDate },
                { "@last_logged_in", user.LastLoggedIn }
            };

            bool check = await db.ExecuteNonQuery(query, parameters);

            if (!check)
            {
                return null;
            }

            return user.Id.ToString();
        }

        public async Task<bool> Delete(string id)
        {
            string query = "DELETE FROM users WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            };

            return await db.ExecuteNonQuery(query, parameters);
        }
    }
}
