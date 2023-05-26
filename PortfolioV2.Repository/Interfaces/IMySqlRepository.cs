namespace PortfolioV2.Repository.Interfaces
{
    public interface IMySqlRepository
    {
        Task<string> ExecuteNonQuery(string query, Dictionary<string, object> parameters);
        Task<List<object>> ExecuteQuery(string query, Dictionary<string, object> parameters);
    }
}
