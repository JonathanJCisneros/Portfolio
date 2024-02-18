#pragma warning disable CS8602
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Core;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Web.Areas.Models;
using PortfolioV2.Web.Models;

namespace PortfolioV2.Web.Areas
{
    [Area("api")]
    public class InquiryController : Controller
    {
        #region Fields

        private readonly IInquiryService _inquiryService;
        private readonly IUserService _userService;

        #endregion Fields

        #region Constructors

        public InquiryController(IInquiryService inquiryService, IUserService userService)
        {
            _inquiryService = inquiryService;
            _userService = userService;
        }

        #endregion Constructors

        #region Private Methods

        private static string TechnicalError(string message)
        {
            return $"Oops, we are having technical issues {message}, please try again later";
        }

        #endregion Private Methods

        #region Public Methods/Actions

        [HttpPost]
        public async Task<JsonResult> NewInquiry(InquiryModel model)
        {
            ValidationResponseModel response = new()
            {
                Errors = new Dictionary<string, string>()
            };

            model = model.Format();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors[0].ErrorMessage.ToString()
                    );

                if (response.Errors.ContainsKey("Type"))
                {
                    response.Errors["Type"] = "Inquiry type is required";
                }

                return Json(response);
            }

            if (User.Identity.IsAuthenticated || (await _userService.CheckByEmail(model.Email)) != null)
            {
                response.Errors.Add("userError", "You can't sumbit inquiries to your own site");

                return Json(response);
            }

            if (!await _inquiryService.Create(model.ToInquiry()))
            {
                response.Errors.Add("serverError", TechnicalError("submitting your inquiry"));

                return Json(response);
            }

            return Json(response);
        }

        public async Task<JsonResult> GetInquiries()
        {
            List<Inquiry> inquiries = await _inquiryService.GetAll();

            return Json(inquiries.Select(x => new InquiryModel(x)).ToList());
        }

        public async Task<JsonResult> Resolve(Guid id)
        {
            ValidationResponseModel response = new()
            {
                Errors = new Dictionary<string, string>()
            };

            if (!await _inquiryService.Resolve(id))
            {
                response.Errors.Add("serverError", TechnicalError("resolving this inquiry"));
            }

            return Json(response);
        }

        public async Task<JsonResult> Delete(Guid id)
        {
            ValidationResponseModel response = new()
            {
                Errors = new Dictionary<string, string>()
            };

            if (!await _inquiryService.Delete(id))
            {
                response.Errors.Add("serverError", TechnicalError("deleting this inquiry"));
            }

            return Json(response);
        }

        #endregion Public Methods/Actions
    }
}