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
        #region Fields

        protected string AuthCode;
        protected string RegistrationIP;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        #endregion Fields

        #region Constructors

        public UserController(IUserService userService, IConfiguration configuration, ILogger<UserController> logger)
        {
            AuthCode = configuration.GetConnectionString("AdminAuth");
            RegistrationIP = configuration.GetConnectionString("RegistrationIP");
            _userService = userService;
            _logger = logger;
        }

        #endregion Constructors

        #region Private Methods

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

        private async Task<bool> IsValidIP()
        {
            bool isValid;

            try
            {
                HttpClient client = new();

                HttpResponseMessage response = await client.GetAsync("https://wtfismyip.com/text");                

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = (await response.Content.ReadAsStringAsync()).Replace("\n", "");

                    isValid = responseBody == RegistrationIP;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                //TODO: Add Cloud Watch Logging

                Console.WriteLine(ex.Message.ToString());

                isValid = false;
            }  
            
            return isValid;
        }

        #endregion Private Methods

        #region Public Methods/Actions

        public IActionResult Login(string? error = null)
        {
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
            model = model.Format(); 
            
            if (!ModelState.IsValid)
            {
                return Login();
            }

            AuthorizeResult status = await _userService.Authorize(model.Email, model.Password);

            if (!status.IsAuthorized)
            {                
                return Login("Your login cridentials are invalid");
            }

            await SetAuthCookie(status);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else if (!await IsValidIP())
            {
                return RedirectToAction("Error", "Portfolio");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
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