#pragma warning disable CS8602
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.Areas
{
    [Area("api")]
    public class InquiryController : Controller
    {
        private readonly IInquiryService _inquiryService;
        private readonly IUserService _userService;

        public InquiryController(IInquiryService inquiryService, IUserService userService)
        {
            _inquiryService = inquiryService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<JsonResult> NewInquiry([FromBody] InquiryModel model)
        {
            ValidationResponseModel response = new()
            {
                Success = false,
                Errors = new Dictionary<string, string>()
            };

            model = InquiryModel.Format(model);

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors[0].ErrorMessage.ToString()
                    );

                return Json(response);
            }

            if(User.Identity.IsAuthenticated || (await _userService.CheckByEmail(model.Email)) != null)
            {
                response.Errors.Add("userError", "You can't sumbit inquiries to your own site");
                
                return Json(response);
            }

            Inquiry inquiry = InquiryModel.ToInquiry(model);

            string? result = await _inquiryService.Create(inquiry);

            if (result == null)
            {
                response.Errors.Add("serverError", "Oops, we have encountered a problem");

                return Json(response);
            }

            response.Success = true;

            return Json(response);
        }
    }
}
