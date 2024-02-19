using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace PortfolioV2.Controllers
{
    public class PortfolioController : Controller
    {
        #region Fields

        private readonly ILogger<PortfolioController> _logger;

        #endregion Fields

        #region Constructors

        public PortfolioController(ILogger<PortfolioController> logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods/Actions

        public IActionResult Home()
        {
            return View();
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }

        #endregion Public Methods/Actions
    }
}