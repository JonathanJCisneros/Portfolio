#pragma warning disable CS8618
using PortfolioV2.Core;
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class RegisterModel : BaseModel
    {
        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [MaxLength(35, ErrorMessage = "is too long")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [MaxLength(35, ErrorMessage = "is too long")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "is required")]
        [EmailAddress(ErrorMessage = "must be a valid Email address")]
        [MaxLength(320, ErrorMessage = "is too long")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(8, ErrorMessage = "must be at least 8 characters")]
        [MaxLength(100, ErrorMessage = "is too long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "is required")]
        [Compare("Password", ErrorMessage = "doesn't match Password")]
        [MaxLength(100, ErrorMessage = "is too long")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "is required")]
        [Display(Name = "Passcode")]
        public int AdminPass { get; set; } = 00000;      

        public User ToUser()
        {
            return new User()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                LastLoggedIn = DateTime.Now,
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate
            };
        }

        public RegisterModel Format()
        {
            Id = Guid.NewGuid();
            FirstName = FirstName.FormatWord();
            LastName = LastName.FormatWord();
            Email = Email.FormatString();
            Password = Password.TrimString();
            ConfirmPassword = ConfirmPassword.TrimString();

            return this;
        }
    }
}