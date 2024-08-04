#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8618
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;
using System.Security.Claims;

namespace PortfolioV2.Web.Controllers
{
    public class UserController : Controller
    {
        #region Fields

        protected string AuthCode;
        private readonly IUserService _userService;
        private readonly IIPAddressService _ipAddressService;

        #endregion Fields

        #region Constructors

        public UserController(IConfiguration configuration, IUserService userService, IIPAddressService ipAddressService)
        {
            AuthCode = configuration.GetConnectionString("AdminAuth");
            _userService = userService;
            _ipAddressService = ipAddressService;
        }

        #endregion Constructors

        #region Private Methods

        private async Task SetAuthCookie(AuthorizeResult result)
        {
            List<Claim> claims = new()
            {
                new(ClaimTypes.Sid, result.Id),
                new(ClaimTypes.Name, result.Name),
                new(ClaimTypes.Email, result.Email)
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

        private async Task<bool> CheckIP()
        {
            string? cookie = Request.Cookies["user_ip"];

            if (String.IsNullOrWhiteSpace(cookie))
            {
                return false;
            }

            IPResponse response = JsonConvert.DeserializeObject<IPResponse>(cookie);

            return await _ipAddressService.IsValidIPAddress(response.IP);
        }

        #endregion Private Methods

        #region Public Methods/Actions

        public async Task<IActionResult> Login(string? error = null)
        {
            if (!await CheckIP())
            {
                return RedirectToAction("Error", "Home");
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            TempData["Error"] = error;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            if (!await CheckIP())
            {
                return RedirectToAction("Error", "Home");
            }

            model = model.Format(); 
            
            if (!ModelState.IsValid)
            {
                return await Login();
            }

            AuthorizeResult status = await _userService.Authorize(model.Email, model.Password);

            if (!status.IsAuthorized)
            {                
                return await Login("Your login cridentials are invalid");
            }

            await SetAuthCookie(status);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async Task<IActionResult> Register(string? error = null)
        {
            if (!await CheckIP())
            {
                return RedirectToAction("Error", "Home");
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            TempData["Error"] = error;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!await CheckIP())
            {
                return RedirectToAction("Error", "Home");
            }

            model = model.Format();
            
            if (!ModelState.IsValid)
            {
                return await Register();
            }
            else if (model.AdminPass.ToString() != AuthCode)
            {
                ModelState.AddModelError("AdminPass", "is invalid");

                return await Register();
            }

            User? dbUser = await _userService.CheckByEmail(model.Email);

            if (dbUser != null)
            {
                ModelState.AddModelError("Email", "is already in use");

                return await Register();
            }

            User user = model.ToUser();

            if (!await _userService.Create(user)) 
            {
                return await Register();
            }

            await SetAuthCookie(new AuthorizeResult(user));

            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        #endregion Public Methods/Actions
    }
}