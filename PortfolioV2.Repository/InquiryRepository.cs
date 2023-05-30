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

        public async Task<bool> CheckById(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT COUNT(1) AS inquiries FROM inquiries WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                int count = 0;

                while (await reader.ReadAsync())
                {
                    count = (int)reader["inquiries"];
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

        public  async Task<Inquiry?> Get(string id)
        {
            Inquiry? inquiry = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("SELECT * FROM inquiries WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    inquiry = new()
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Type = reader["Type"].ToString(),
                        Details = reader["Details"].ToString(),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"])
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
                await using MySqlCommand cmd = new("SELECT * FROM inquiries ORDER BY CreatedDate DESC;", conn);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    inquiries.Add(new Inquiry
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Type = reader["Type"].ToString(),
                        Details = reader["Details"].ToString(),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"])
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
                await using MySqlCommand cmd = new("INSERT INTO inquiries(Id, Name, Email, Type, Details, Status, CreatedDate, UpdatedDate) VALUES(@id, @name, @email, @type, @details, @status, @createddate, @updateddate);", conn);

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

        public async Task<string?> Update(Inquiry inquiry)
        {
            string? id = null;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("UPDATE inquiries SET Status = @status, UpdatedDate = @updateddate WHERE Id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", inquiry.Id.ToString());
                cmd.Parameters.AddWithValue("@status", inquiry.Status);
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

        public async Task<bool> Delete(string id)
        {
            bool check = false;

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new("DELETE FROM inquiries WHERE Id = @id;", conn);

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
