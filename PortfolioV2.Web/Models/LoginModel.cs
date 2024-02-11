#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public class LoginModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "is required")]
        [EmailAddress(ErrorMessage = "is invalid")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime LoginDateTime { get; set; } = DateTime.Now;

        public LoginModel Format()
        {
            Email = Email.FormatString();
            Password = Password.TrimString();

            return this;
        }
    }
}
