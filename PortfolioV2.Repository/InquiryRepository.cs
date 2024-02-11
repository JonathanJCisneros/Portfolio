#pragma warning disable CS8601
#pragma warning disable CS8604
using PortfolioV2.Core;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class InquiryRepository : IInquiryRepository
    {
        #region Fields

        private readonly IMySqlRepository db;

        #endregion Fields

        #region Constructors

        public InquiryRepository(IMySqlRepository mySqlRepository)
        {
            db = mySqlRepository;
        }

        #endregion Constructors

        #region Private Methods

        private static Dictionary<string, object> BuildParameters(Inquiry inquiry)
        {
            return new Dictionary<string, object>()
            {
                { "@id", inquiry.Id },
                { "@name", inquiry.Name },
                { "@email", inquiry.Email },
                { "@type", inquiry.Type.GetDisplayName() },
                { "@details", inquiry.Details },
                { "@status", inquiry.Status.GetDisplayName() },
                { "@created_date", inquiry.CreatedDate },
                { "@updated_date", inquiry.UpdatedDate }
            };
        }

        private static List<Inquiry> ConvertTable(DataTable dt)
        {
            return dt.AsEnumerable().Select(x => new Inquiry
            {
                Id = x.Field<Guid>("id"),
                Name = x.Field<string>("name"),
                Email = x.Field<string>("email"),
                Type = EnumHelper.GetValueFromName<InquiryType>(x.Field<string>("type")),
                Details = x.Field<string>("details"),
                Status = EnumHelper.GetValueFromName<Status>(x.Field<string>("status")),
                CreatedDate = x.Field<DateTime>("created_date"),
                UpdatedDate = x.Field<DateTime>("updated_date")
            }).ToList();
        }

        #endregion Private Methods

        #region Public Methods

        public async Task<Inquiry?> Get(Guid id)
        {
            string query = @"SELECT * 
                             FROM inquiries 
                             WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id }
            };

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return dt.Rows.Count == 0 ? null : ConvertTable(dt)[0];
        }

        public async Task<List<Inquiry>> GetAll()
        {
            string query = @"SELECT * 
                             FROM inquiries 
                             ORDER BY created_date DESC;";

            Dictionary<string, object> parameters = new();

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return ConvertTable(dt);
        }

        public async Task<bool> Create(Inquiry inquiry)
        {
            string query = @"INSERT INTO inquiries(id, name, email, type, details, status, created_date, updated_date) 
                                            VALUES(@id, @name, @email, @type, @details, @status, @created_date, @updated_date);";

            return await db.ExecuteNonQuery(query, BuildParameters(inquiry));
        }

        public async Task<bool> Resolve(Guid id)
        {
            string query = @"UPDATE inquiries 
                             SET status = @status, updated_date = @updated_date 
                             WHERE id = @id;";

            Dictionary<string, object> parameters = new()
            {
                { "@id", id },
                { "@status", "Resolved" },
                { "@updated_date", DateTime.Now }
            };            

            return await db.ExecuteNonQuery(query, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            string query = @"DELETE FROM inquiries 
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