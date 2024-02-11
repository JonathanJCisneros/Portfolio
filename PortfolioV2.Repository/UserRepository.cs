#pragma warning disable CS8601
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly IMySqlRepository db;

        #endregion Fields

        #region Constructors

        public UserRepository(IMySqlRepository mySqlRepository)
        {
            db = mySqlRepository;
        }

        #endregion Constructors

        #region Private Methods

        private static Dictionary<string, object> BuildParameters(User user)
        {
            return new Dictionary<string, object>()
            {
                { "@id", user.Id },
                { "@first_name", user.FirstName },
                { "@last_name", user.LastName },
                { "@email", user.Email },
                { "@password", user.Password },
                { "@last_logged_in", user.LastLoggedIn },
                { "@created_date", user.CreatedDate },
                { "@updated_date", user.UpdatedDate }
            };
        }

        private static List<User> ConvertTable(DataTable dt)
        {
            return dt.AsEnumerable().Select(x => new User
            {
                Id = x.Field<Guid>("id"),
                FirstName = x.Field<string>("first_name"),
                LastName = x.Field<string>("last_name"),
                Email = x.Field<string>("email"),
                Password = x.Field<string>("password"),
                LastLoggedIn = x.Field<DateTime>("last_logged_in"),
                CreatedDate = x.Field<DateTime>("created_date"),
                UpdatedDate = x.Field<DateTime>("updated_date")
            }).ToList();
        }

        #endregion Private Methods

        #region Public Methods

        public async Task<User?> CheckByEmail(string email)
        {
            string query = @"SELECT * 
                             FROM users 
                             WHERE email = @email;";

            Dictionary<string, object> parameters = new()
            {
                { "@email", email}
            };

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return dt.Rows.Count == 0 ? null : ConvertTable(dt)[0];
        }

        public async Task Login(Guid id)
        {
            string query = @"UPDATE users 
                             SET last_logged_in = @time 
                             WHERE id = @id";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id },
                { "@time", DateTime.Now }
            };
            
            await db.ExecuteNonQuery(query, parameters);
        }

        public async Task<bool> Create(User user)
        {
            string query = @"INSERT INTO users(id, first_name, last_name, email, password, created_date, updated_date, last_logged_in) 
                                        VALUES(@id, @first_name, @last_name, @email, @password, @created_date, @updated_date, @last_logged_in);";

            return await db.ExecuteNonQuery(query, BuildParameters(user));
        }

        public async Task<bool> Delete(Guid id)
        {
            string query = @"DELETE FROM users 
                             WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            };

            return await db.ExecuteNonQuery(query, parameters);
        }

        #endregion Public Methods
    }
}