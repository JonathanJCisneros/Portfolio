#pragma warning disable CS8618

namespace PortfolioV2.Core
{
    public class Inquiry : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string InquiryType { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }
    }
}
