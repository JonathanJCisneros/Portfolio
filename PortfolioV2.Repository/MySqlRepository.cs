using MySqlConnector;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class MySqlRepository : IMySqlRepository
    {
        #region Fields

        protected readonly string connection;
        private readonly Serilog.ILogger _logger;

        #endregion Fields

        #region Constructors

        public MySqlRepository(string connectionString, Serilog.ILogger logger)
        {
            connection = connectionString;
            _logger = logger;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods

        public async Task<bool> ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                await using MySqlConnection conn = new(connection); 
                await conn.OpenAsync();
                await using MySqlCommand cmd = new(query, conn);

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "There was a problem connecting to the database");
            }

            return false;
        }

        public async Task<DataTable> ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            DataTable table = new();

            try
            {
                await using MySqlConnection conn = new(connection);
                await conn.OpenAsync();
                await using MySqlCommand cmd = new(query, conn);

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                table.Load(await cmd.ExecuteReaderAsync());

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "There was a problem connecting to the database");
            }

            return table;
        }

        #endregion Public Methods
    }
}