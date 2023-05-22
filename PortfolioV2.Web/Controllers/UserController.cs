#pragma warning disable CS8603
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;
using System.Security.Claims;

namespace PortfolioV2.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;      
        

        public UserController(IUserService userService, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
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

            await HttpContext.SignInAsync(principle, authenticationProperties);
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model = LoginModel.Format(model);

            if(!ModelState.IsValid)
            {
                return Login();
            }

            AuthorizeResult status = await _userService.Authorize(model.Email, model.Password);

            if(!status.IsAuthorized)
            {
                ModelState.AddModelError("Email", "not found");
                return Login();
            }

            await SetAuthCookie(status);

            return RedirectToAction("Dashboard");
        }

        public IActionResult Register()
        {
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
            else if (model.AdminPass.ToString() != _configuration.GetConnectionString("Auth"))
            {
                ModelState.AddModelError("AdminPass", "is invalid");
                return Register();
            }

            User dbUser = await _userService.CheckByEmail(model.Email);

            if(dbUser != null)
            {
                ModelState.AddModelError("Email", "is already in use");
                return Register();
            }

            User user = RegisterModel.ToUser(model);

            string saved = await _userService.Create(user);

            if (saved == null) 
            {
                ViewBag.Error = "Oops, we could not save your account at this time";
                return Register();
            }

            return RedirectToAction("Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return Login();
        }

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
    }
}
