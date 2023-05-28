namespace PortfolioV2.Web.Models
{
    public class ValidationResponseModel
    {
        public bool Success { get; set; }

        public required Dictionary<string, string> Errors { get; set; }
    }
}
