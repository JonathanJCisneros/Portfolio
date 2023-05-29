using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IInquiryService _inquiryService;
        private readonly ILogger<UserController> _logger;

        public DashboardController(IInquiryService inquiryService, ILogger<UserController> logger)
        {
            _inquiryService = inquiryService;
            _logger = logger;
        }

        public async Task<IActionResult> Dashboard()
        {
            List<InquiryModel> inquiries = new()
            {
                new InquiryModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Jonathan Cisneros",
                    Email = "jonathan.cisneros@grizzly.com",
                    Type = "Project",
                    Details = "This is a test scenario one",
                    Status = "Unresolved",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new InquiryModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Alex Miller",
                    Email = "alex@miller.com",
                    Type = "Hire",
                    Details = "This is a test scenario two",
                    Status = "Unresolved",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new InquiryModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "John Smith",
                    Email = "johns@grizzly.com",
                    Type = "Project",
                    Details = "This is a test scenario three",
                    Status = "Resolved",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }
            };

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
