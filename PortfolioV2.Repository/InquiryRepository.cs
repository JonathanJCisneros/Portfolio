#pragma warning disable CS8601
#pragma warning disable CS8604
using MySqlConnector;
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;

namespace PortfolioV2.Repository
{
    public class InquiryRepository : IInquiryRepository
    {
        protected string connection;

        public InquiryRepository(string connectionString)
        {
            connection = connectionString;
        }

        public  async Task<Inquiry?> Get(string id)
        {
            Inquiry? inquiry = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM inquiries WHERE id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    inquiry = new()
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Email = reader["email"].ToString(),
                        Type = reader["type"].ToString(),
                        Details = reader["details"].ToString(),
                        Status = reader["status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["created_date"]),
                        UpdatedDate = Convert.ToDateTime(reader["updated_date"])
                    };
                }

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return inquiry;
        }

        public async Task<List<Inquiry>> GetAll()
        {
            List<Inquiry> inquiries = new();

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM inquiries ORDER BY created_date DESC;", conn);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    inquiries.Add(new Inquiry
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Email = reader["email"].ToString(),
                        Type = reader["type"].ToString(),
                        Details = reader["details"].ToString(),
                        Status = reader["status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["created_date"]),
                        UpdatedDate = Convert.ToDateTime(reader["updated_date"])
                    });
                }

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return inquiries;
        }

        public async Task<string?> Create(Inquiry inquiry)
        {
            string? id = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("INSERT INTO inquiries(id, name, email, type, details, status, created_date, updated_date) VALUES(@id, @name, @email, @type, @details, @status, @createddate, @updateddate);", conn);

                cmd.Parameters.AddWithValue("@id", inquiry.Id.ToString());
                cmd.Parameters.AddWithValue("@name", inquiry.Name);
                cmd.Parameters.AddWithValue("@email", inquiry.Email);
                cmd.Parameters.AddWithValue("@type", inquiry.Type);
                cmd.Parameters.AddWithValue("@details", inquiry.Details);
                cmd.Parameters.AddWithValue("@status", inquiry.Status);
                cmd.Parameters.AddWithValue("@createddate", inquiry.CreatedDate);
                cmd.Parameters.AddWithValue("@updateddate", inquiry.UpdatedDate);

                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

                id = inquiry.Id.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }

        public async Task<bool> Resolve(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("UPDATE inquiries SET status = @status, updated_date = @updateddate WHERE id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", "Resolved");
                cmd.Parameters.AddWithValue("@updateddate", DateTime.Now);

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

        public async Task<bool> Delete(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("DELETE FROM inquiries WHERE id = @id;", conn);

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
