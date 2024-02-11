using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        #region Fields

        private readonly IInquiryService _inquiryService;

        #endregion Fields

        #region Constructors

        public DashboardController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        #endregion Constructors

        #region Private Methods



        #endregion Private Methods

        #region Public Methods/Actions

        public async Task<IActionResult> Dashboard()
        {
            List<Inquiry> dbInquiries = await _inquiryService.GetAll();

            ViewData["SiteClass"] = "dashboard";

            return View(dbInquiries.Select(x => new InquiryModel(x)).ToList());
        }

        public async Task<IActionResult> ViewOne(Guid id)
        {
            Inquiry? inquiry = await _inquiryService.Get(id);

            if (inquiry == null)
            {
                return RedirectToAction("Dashboard");
            }

            ViewData["SiteClass"] = "inquiry";

            return View(new InquiryModel(inquiry));
        }

        public async Task<IActionResult> Resolve(Guid id, string redirect)
        {
            if (!await _inquiryService.Resolve(id))
            {
                TempData["Error"] = "We could not update your request, please try again later";
            }

            return redirect == "ViewOne" ? RedirectToAction(redirect, new { id }) : (IActionResult)RedirectToAction(redirect);
        }

        public async Task<IActionResult> Delete(Guid id, string errorRedirect)
        {
            if (!await _inquiryService.Delete(id))
            {
                TempData["Error"] = "We could not update your request, please try again later";

                return errorRedirect == "ViewOne" ? RedirectToAction(errorRedirect, new { id }) : (IActionResult)RedirectToAction(errorRedirect);
            }

            return RedirectToAction("Dashboard");
        }

        #endregion Public Methods/Actions
    }
}