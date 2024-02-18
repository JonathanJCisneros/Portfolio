namespace PortfolioV2.Web.Areas.Models
{
    public class ValidationResponseModel
    {
        public bool Success
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public required Dictionary<string, string> Errors { get; set; }
    }
}