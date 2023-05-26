#pragma warning disable CS8601
#pragma warning disable CS8603
#pragma warning disable CS8618
using PortfolioV2.Core;
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class RegisterModel
    {
        [Required]
        public required Guid Id { get; set; }

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

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "is required")]
        [Display(Name = "Passcode")]
        public int AdminPass { get; set; } = 00000;        

        public static RegisterModel Format(RegisterModel model)
        {
            model.Id = Guid.NewGuid();
            model.FirstName = CustomAttributes.FormatWord(model.FirstName);
            model.LastName = CustomAttributes.FormatWord(model.LastName);
            model.Email = CustomAttributes.FormatString(model.Email);
            model.Password = !string.IsNullOrEmpty(model.Password) ? model.Password.Trim() : model.Password;
            model.ConfirmPassword = !string.IsNullOrEmpty(model.ConfirmPassword) ? model.ConfirmPassword.Trim() : model.ConfirmPassword;

            return model;
        }

        public static User ToUser(RegisterModel model)
        {
            return new User()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                LastLoggedIn = DateTime.Now,
                CreatedDate = model.CreatedDate,
                UpdatedDate = model.UpdatedDate
            };
        }
    }
}
