#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8618
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;
using System.Security.Claims;

namespace PortfolioV2.Web.Controllers
{
    public class UserController : Controller
    {
        protected string AuthCode;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;     
        

        public UserController(IUserService userService, IConfiguration configuration, ILogger<UserController> logger)
        {
            AuthCode = configuration.GetConnectionString("AdminAuth");
            _userService = userService;
            _logger = logger;
        }

        private async Task SetAuthCookie(AuthorizeResult result)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, result.Id),
                new Claim(ClaimTypes.Name, result.Name),
                new Claim(ClaimTypes.Email, result.Email)
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principle = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(3)
            };

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(principle, authenticationProperties);
        }

        public IActionResult Login(string? error = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            ViewBag.Error = error;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            model = LoginModel.Format(model);

            if(!ModelState.IsValid)
            {
                return Login();
            }

            AuthorizeResult status = await _userService.Authorize(model.Email, model.Password);

            if(!status.IsAuthorized)
            {                
                return Login("Your login cridentials are invalid");
            }

            await SetAuthCookie(status);

            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            model = RegisterModel.Format(model);

            if (!ModelState.IsValid)
            {
                return Register();
            }
            else if (model.AdminPass.ToString() != AuthCode)
            {
                ModelState.AddModelError("AdminPass", "is invalid");
                return Register();
            }

            User? dbUser = await _userService.CheckByEmail(model.Email);

            if(dbUser != null)
            {
                ModelState.AddModelError("Email", "is already in use");
                return Register();
            }

            User user = RegisterModel.ToUser(model);

            string? saved = await _userService.Create(user);

            if (saved == null) 
            {
                return Register();
            }

            await SetAuthCookie(new()
            {
                Id = user.Id.ToString(),
                Name = user.FirstName,
                Email = user.Email
            });

            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }        
    }
}
