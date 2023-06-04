#pragma warning disable CS8601
#pragma warning disable CS8604
using MySqlConnector;
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;

namespace PortfolioV2.Repository
{
    public class UserRepository : IUserRepository
    {
        protected string connection;

        public UserRepository(string connectionString)
        {
            connection = connectionString;
        }

        public async Task<User?> CheckByEmail(string email)
        {
            User? user = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM users WHERE email = @email;", conn);

                cmd.Parameters.AddWithValue("@email", email);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    user = new()
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        Email = reader["email"].ToString(),
                        Password = reader["password"].ToString()
                    };
                }

                await conn.CloseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return user;
        }

        public async Task Login(string id)
        {
            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("UPDATE users SET last_logged_in = @time WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@time", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", id);

                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string?> Create(User user)
        {
            string? id = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("INSERT INTO users(id, first_name, last_name, email, password, created_date, updated_date, last_logged_in) VALUES(@id, @firstname, @lastname, @email, @password, @createddate, @updateddate, @lastloggedin);", conn);

                cmd.Parameters.AddWithValue("@id", user.Id.ToString());
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@createddate", user.CreatedDate);
                cmd.Parameters.AddWithValue("@updateddate", user.UpdatedDate);
                cmd.Parameters.AddWithValue("@lastloggedin", user.LastLoggedIn);

                await cmd.ExecuteNonQueryAsync();    
                
                await conn.CloseAsync();

                id = user.Id.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }

        public async Task<bool> Delete(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("DELETE FROM users WHERE id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);
                                
                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

                check = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }
    }
}
