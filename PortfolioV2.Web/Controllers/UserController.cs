#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8618
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;
using System.Net;
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
            RegistrationIP = configuration.GetConnectionString(App.IsDeployed ? "ProductionRegistrationIP" : "LocalRegistrationIP");
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

        private bool IsValidIP()
        {
            string? ipAddress = Request.Headers["X_FORWARDED_FOR"];

            ipAddress ??= Request.Headers["HTTP_X_FORWARDED_FOR"];

            ipAddress ??= HttpContext.Request.Headers["REMOTE_ADDR"];

            if (String.IsNullOrEmpty(ipAddress))
            {
                string hostName = Dns.GetHostName();

                IPHostEntry ipHostEntries = Dns.GetHostEntry(hostName);

                IPAddress[] ipAddresses = ipHostEntries.AddressList;

                if (ipAddresses == null || ipAddresses.Length == 0)
                {
                    ipAddresses = Dns.GetHostAddresses(hostName);
                }

                return ipAddresses.Any(x => x.ToString() == RegistrationIP);
            }

            return ipAddress == RegistrationIP;
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

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else if (!IsValidIP())
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
                return Register();
            }
            else if (model.AdminPass.ToString() != AuthCode)
            {
                ModelState.AddModelError("AdminPass", "is invalid");

                return Register();
            }

            User? dbUser = await _userService.CheckByEmail(model.Email);

            if (dbUser != null)
            {
                ModelState.AddModelError("Email", "is already in use");

                return Register();
            }

            User user = model.ToUser();

            if (!await _userService.Create(user)) 
            {
                return Register();
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