#pragma warning disable CS8603
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

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

        private string UserId => HttpContext.Session.GetString("UserId");

        private bool LoggedIn => UserId != null;

        public IActionResult Login()
        {
            if (UserId != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (UserId != null)
            {
                return RedirectToAction("Dashboard");
            }

            model = LoginModel.Format(model);

            if(!ModelState.IsValid)
            {
                return Login();
            }

            User dbUser = await _userService.CheckByEmail(model.Email);

            if(dbUser == null)
            {
                ModelState.AddModelError("Email", "not found");
                return Login();
            }

            HttpContext.Session.SetString("UserId", dbUser.Id.ToString());

            return RedirectToAction("Dashboard");
        }

        public IActionResult Register()
        {
            if (UserId != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (UserId != null)
            {
                return RedirectToAction("Dashboard");
            }

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
                ViewBag.Error = "Oops, an error occurred";
                return Register();
            }

            HttpContext.Session.SetString("UserId", saved.ToString());

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            if (UserId == null)
            {
                return Login();
            }

            return View();
        }
    }
}
