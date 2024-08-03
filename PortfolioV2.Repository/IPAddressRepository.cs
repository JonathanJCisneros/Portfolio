#pragma warning disable CS8619
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class IPAddressRepository : IIPAddressRepository
    {
        #region Fields

        private readonly IMySqlRepository db;

        #endregion Fields

        #region Constructors

        public IPAddressRepository(IMySqlRepository mySqlRepository)
        {
            db = mySqlRepository;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods

        public async Task<List<string>> GetIPAddresses()
        {
            string query = @"SELECT * FROM ip_addresses;";

            Dictionary<string, object> parameters = new();

            DataTable dt = await db.ExecuteQuery(query, parameters);

            return dt.AsEnumerable().Select(x => x.Field<string>("ip_address")).ToList();
        }

        #endregion Public Methods
    }
}