#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Core
{
    public class Inquiry : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public InquiryType Type { get; set; }

        public string Details { get; set; }

        public Status Status { get; set; }
    }

    public enum InquiryType
    {
        [Display(Name = "Question")]
        Question,
        [Display(Name = "Project")]
        Project,
        [Display(Name = "Hire")]
        Hire
    }

    public enum Status
    {
        [Display(Name = "Unresolved")]
        Unresolved,
        [Display(Name = "Resolved")]
        Resolved
    }
}