using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IInquiryService _inquiryService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IInquiryService inquiryService, ILogger<DashboardController> logger)
        {
            _inquiryService = inquiryService;
            _logger = logger;
        }

        public async Task<IActionResult> Dashboard()
        {
            List<Inquiry> dbInquiries = await _inquiryService.GetAll();

            List<InquiryModel> inquiries = new();

            foreach (Inquiry inquiry in dbInquiries)
            {
                inquiries.Add(new InquiryModel(inquiry));
            }

            ViewData["SiteClass"] = "dashboard";

            return View(inquiries);
        }

        public async Task<IActionResult> ViewOne(string id)
        {
            return View();
        }

        public async Task<IActionResult> Resolve(string id)
        {
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Delete(string id)
        {
            return RedirectToAction("Daschboard");
        }
    }
}
