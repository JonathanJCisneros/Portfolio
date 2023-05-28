#pragma warning disable CS8602
#pragma warning disable CS8619
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.API
{
    [Area("api")]
    public class InquiryController : Controller
    {
        private IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
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

            Inquiry inquiry = InquiryModel.ToInquiry(model);

            string? result = await _inquiryService.Create(inquiry);

            if (result == null)
            {
                response.Errors.Add("serverError", "Oops, we have encountered a problem");
            }

            response.Success = true;

            return Json(response);
        }
    }
}
