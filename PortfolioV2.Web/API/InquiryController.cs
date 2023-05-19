using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioV2.Web.API
{
    [Route("api/[controller]")]
    public class InquiryController : Controller
    {
        public async Task<JsonResult> Test()
        {
            dynamic success = new
            {
                status = "success"
            };

            return Json(success);
        }
    }
}
