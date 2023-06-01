#pragma warning disable CS8604
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            List<Inquiry> dbInquiries = await _inquiryService.GetAll();

            List<InquiryModel> inquiries = dbInquiries.Select(x => new InquiryModel(x)).ToList();

            ViewData["SiteClass"] = "dashboard";

            return View(inquiries);
        }

        [Authorize]
        public async Task<IActionResult> ViewOne(string id)
        {
            InquiryModel inquiry = new(await _inquiryService.Get(id));

            ViewData["SiteClass"] = "inquiry";

            return View(inquiry);
        }

        [Authorize]
        public async Task<IActionResult> Resolve(string id)
        {
            bool status = await _inquiryService.Resolve(id);

            if(!status)
            {
                return await ViewOne(id);
            }

            return RedirectToAction("Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            bool status = await _inquiryService.Delete(id);

            if(!status)
            {
                return await ViewOne(id);
            }

            return RedirectToAction("Dashboard");
        }
    }
}
