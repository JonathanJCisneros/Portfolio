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

        public async Task<bool> CheckById(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT COUNT(1) AS users FROM users WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                int count = 0;

                while (await reader.ReadAsync())
                {
                    count = (int)reader["users"];
                }

                if (count > 0) 
                {
                    check = true;
                }

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }

        public async Task Login(string id)
        {
            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("UPDATE users SET LastLoggedIn = @time", conn);

                cmd.Parameters.AddWithValue("@time", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<User?> CheckByEmail(string email)
        {
            User? user = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM users WHERE Email = @email;", conn);

                cmd.Parameters.AddWithValue("@email", email);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    user = new()
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString()
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

        public async Task<User?> Get(string id)
        {
            User? user = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM users WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    user = new()
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString()
                    };
                }

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return user;
        }

        public async Task<List<User>> GetAll()
        {
            List<User> users = new();

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM users", conn);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new User()
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString()
                    });
                }

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return users;
        }

        public async Task<string?> Create(User user)
        {
            string? id = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("INSERT INTO users(Id, FirstName, LastName, Email, Password, CreatedDate, UpdatedDate, LastLoggedIn) VALUES(@id, @firstname, @lastname, @email, @password, @createddate, @updateddate, @lastloggedin);", conn);

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

        public async Task<string?> Update(User user)
        {
            string? id = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("UPDATE users SET FirstName = @firstname, LastName = @lastname, Email = @email, UpdatedDate = @updateddate WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", user.Id.ToString());
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@updateddate", user.UpdatedDate);
                cmd.Parameters.AddWithValue("@lastloggedin", user.LastLoggedIn);

                object? result = await cmd.ExecuteScalarAsync();

                if (result != null)
                {
                    id = result.ToString();
                }

                await conn.CloseAsync();
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
                await using MySqlCommand cmd = new("DELETE FROM users WHERE Id = @id;", conn);

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
