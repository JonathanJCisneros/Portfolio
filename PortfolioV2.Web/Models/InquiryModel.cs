#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class InquiryModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "is requried")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters long")]
        [MaxLength(70, ErrorMessage = "is too long")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "is required")]
        [EmailAddress(ErrorMessage = "must be a valid Email address")]
        [MaxLength(320, ErrorMessage = "is too long")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "is required")]
        [MaxLength(8, ErrorMessage = "is too long")]
        [Display(Name = "Inquiry Type")]
        public string InquiryType { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(4, ErrorMessage = "details must be at least 4 characters long")]
        [MaxLength(1000, ErrorMessage = "is too long")]
        [Display(Name = "Details")]
        public string Details { get; set; }

        public string Status { get; set; } = "Unresolved";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public static InquiryModel Format(InquiryModel model)
        {
            model.Id = Guid.NewGuid();
            model.Name = model.Name.Trim();
            model.Email = model.Email.Trim().ToLower();
            model.Details = model.Details.Trim();

            return model;
        }
    }
}
