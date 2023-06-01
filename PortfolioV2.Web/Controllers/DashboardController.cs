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

        public DashboardController(IInquiryService inquiryService, ILogger<DashboardController> logger)
        {
            _inquiryService = inquiryService;
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
        public async Task<IActionResult> Resolve(string id, string redirect)
        {
            bool status = await _inquiryService.Resolve(id);

            if(!status)
            {
                ViewBag.Error = "We could not update your request, please try again later";
            }

            if (redirect == "ViewOne")
            {
                return RedirectToAction(redirect, new { id });
            }

            return RedirectToAction(redirect);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id, string errorRedirect)
        {
            bool status = await _inquiryService.Delete(id);

            if(!status)
            {
                ViewBag.Error = "We could not update your request, please try again later";

                if (errorRedirect == "ViewOne")
                {
                    return RedirectToAction(errorRedirect, new { id });
                }

                return RedirectToAction(errorRedirect);
            }

            return RedirectToAction("Dashboard");
        }
    }
}
