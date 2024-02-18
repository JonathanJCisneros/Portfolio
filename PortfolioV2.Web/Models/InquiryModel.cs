#pragma warning disable CS8618
using PortfolioV2.Core;
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class InquiryModel : BaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(70, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        [MaxLength(320, ErrorMessage = "Email is too long")]
        public string Email { get; set; }

        [Required]
        public InquiryType Type { get; set; }

        public string TypeValue
        {
            get
            {
                return Type.ToString();
            }
        }

        [Required(ErrorMessage = "Details are required")]
        [MinLength(4, ErrorMessage = "Details must be at least 4 characters long")]
        [MaxLength(1000, ErrorMessage = "Details are too long")]
        public string Details { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Unresolved;

        public string StatusValue
        {
            get
            {
                return Status.ToString();
            }
        }

        public InquiryModel() { }

        public InquiryModel(Inquiry inquiry)
        {
            Id = inquiry.Id;
            Name = inquiry.Name;
            Email = inquiry.Email;
            Type = inquiry.Type;
            Details = inquiry.Details;
            Status = inquiry.Status;
            CreatedDate = inquiry.CreatedDate;
            UpdatedDate = inquiry.UpdatedDate;            
        }        

        public Inquiry ToInquiry()
        {
            return new Inquiry
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Type = Type,
                Details = Details,
                Status = Status,
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate
            };
        }

        public InquiryModel Format()
        {
            Id = Guid.NewGuid();
            Name = Name.FormatFull();
            Email = Email.FormatString();
            Details = Details.TrimString();

            return this;
        }
    }
}