#pragma warning disable CS8601
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class InquiryRepository : IInquiryRepository
    {
        private readonly IMySqlRepository db;

        public InquiryRepository(IMySqlRepository mySqlRepository)
        {
            db = mySqlRepository;
        }

        public  async Task<Inquiry?> Get(string id)
        {
            string query = "SELECT * FROM inquiries WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            };

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return dt.Rows.Count == 0 ? null : new()
            {
                Id = dt.Rows[0].Field<Guid>("id"),
                Name = dt.Rows[0].Field<string>("name"),
                Email = dt.Rows[0].Field<string>("email"),
                Type = dt.Rows[0].Field<string>("type"),
                Details = dt.Rows[0].Field<string>("details"),
                Status = dt.Rows[0].Field<string>("status"),
                CreatedDate = dt.Rows[0].Field<DateTime>("created_date"),
                UpdatedDate = dt.Rows[0].Field<DateTime>("updated_date")
            };
        }

        public async Task<List<Inquiry>> GetAll()
        {
            string query = "SELECT * FROM inquiries ORDER BY created_date DESC;";

            Dictionary<string, object> parameters = new();

            DataTable dt = await db.ExecuteQuery(query, parameters); 

            return dt.AsEnumerable().Select(x => new Inquiry
            {
                Id = x.Field<Guid>("id"),
                Name = x.Field<string>("name"),
                Email = x.Field<string>("email"),
                Type = x.Field<string>("type"),
                Details = x.Field<string>("details"),
                Status = x.Field<string>("status"),
                CreatedDate = x.Field<DateTime>("created_date"),
                UpdatedDate = x.Field<DateTime>("updated_date")
            }).ToList();
        }

        public async Task<string?> Create(Inquiry inquiry)
        {
            string query = "INSERT INTO inquiries(id, name, email, type, details, status, created_date, updated_date) VALUES(@id, @name, @email, @type, @details, @status, @created_date, @updated_date);";

            Dictionary<string, object> parameters = new()
            {
                { "@id", inquiry.Id.ToString() },
                { "@name", inquiry.Name },
                { "@email", inquiry.Email },
                { "@type", inquiry.Type },
                { "@details", inquiry.Details },
                { "@status", inquiry.Status },
                { "@created_date", inquiry.CreatedDate },
                { "@updated_date", inquiry.UpdatedDate }
            };

            bool check = await db.ExecuteNonQuery(query, parameters);

            if (!check) 
            {
                return null;
            }

            return inquiry.Id.ToString();
        }

        public async Task<bool> Resolve(string id)
        {
            string query = "UPDATE inquiries SET status = @status, updated_date = @updated_date WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id },
                { "@status", "Resolved" },
                { "@updated_date", DateTime.Now }
            };            

            return await db.ExecuteNonQuery(query, parameters);
        }

        public async Task<bool> Delete(string id)
        {
            string query = "DELETE FROM inquiries WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            }; 

            return await db.ExecuteNonQuery(query, parameters);
        }
    }
}
