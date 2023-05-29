#pragma warning disable CS8618
using PortfolioV2.Core;
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class InquiryModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(70, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        [MaxLength(320, ErrorMessage = "Email is too long")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [MaxLength(8, ErrorMessage = "Type is too long")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Details are required")]
        [MinLength(4, ErrorMessage = "Details must be at least 4 characters long")]
        [MaxLength(1000, ErrorMessage = "Details are too long")]
        public string Details { get; set; }

        public string Status { get; set; } = "Unresolved";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public static InquiryModel Format(InquiryModel model)
        {
            model.Id = Guid.NewGuid();
            model.Name = CustomAttributes.FormatFull(model.Name);
            model.Email = CustomAttributes.FormatString(model.Email);
            model.Details = model.Details.Trim();

            return model;
        }

        public static Inquiry ToInquiry(InquiryModel inquiry)
        {
            return new Inquiry
            {
                Id = Guid.NewGuid(),
                Name = inquiry.Name,
                Email = inquiry.Email,
                Type = inquiry.Type,
                Details = inquiry.Details,
                Status = inquiry.Status,
                CreatedDate = inquiry.CreatedDate,
                UpdatedDate = inquiry.UpdatedDate
            };
        }
    }
}
