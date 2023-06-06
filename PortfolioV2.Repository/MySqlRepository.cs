#pragma warning disable CS8602
using MySqlConnector;
using PortfolioV2.Repository.Interfaces;
using System.Data;

namespace PortfolioV2.Repository
{
    public class MySqlRepository : IMySqlRepository
    {
        protected readonly string connection;

        public MySqlRepository(string connectionString)
        {
            connection = connectionString;
        }

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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
            }

            return table;
        }        
    }
}
