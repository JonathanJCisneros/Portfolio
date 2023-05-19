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

        public static LoginModel Format(LoginModel model)
        {
            model.Email = model.Email.Trim().ToLower();
            model.Password = model.Password.Trim();

            return model;
        }
    }
}
