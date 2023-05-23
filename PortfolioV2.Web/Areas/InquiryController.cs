using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.API
{
    [Area("api")]
    public class InquiryController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> NewInquiry([FromBody] InquiryModel model)
        {
            model = InquiryModel.Format(model);

            if (!ModelState.IsValid)
            {
                return Json(new { status = "failure" });
            }

            return Json(new { status = "success"});
        }
    }
}
