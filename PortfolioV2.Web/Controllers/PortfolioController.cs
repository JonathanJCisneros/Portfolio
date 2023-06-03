using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Web.Models;
using System.Diagnostics;

namespace PortfolioV2.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(ILogger<PortfolioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}