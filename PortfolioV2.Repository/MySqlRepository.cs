#pragma warning disable CS8603
using MySqlConnector;
using PortfolioV2.Repository.Interfaces;

namespace PortfolioV2.Repository
{
    public class MySqlRepository : IMySqlRepository
    {
        protected string connection;

        public MySqlRepository(string connectionString)
        {
            connection = connectionString;
        }

        public async Task<string> ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {           
            string? id = null;

            try
            {
                await using MySqlConnection con = new(connection);

                await con.OpenAsync();

                await using MySqlCommand command = con.CreateCommand();

                command.CommandText = query;

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                await command.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return id;
        }

        public async Task<List<object>> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            List<object> results = new();

            try
            {
                await using MySqlConnection con = new(connection);

                await con.OpenAsync();

                await using MySqlCommand command = con.CreateCommand();

                command.CommandText = query;

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                await using MySqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    results.Add(reader);
                }

                await con.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return results; 
        } 
    }
}
