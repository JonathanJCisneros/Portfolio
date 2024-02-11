#pragma warning disable CS8602
using Microsoft.AspNetCore.Mvc;
using PortfolioV2.Service.Interfaces;
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



        #endregion Private Methods

        #region Public Methods/Actions

        [HttpPost]
        public async Task<JsonResult> NewInquiry(InquiryModel model)
        {
            ValidationResponseModel response = new()
            {
                Success = false,
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

                return Json(response);
            }

            if (User.Identity.IsAuthenticated || (await _userService.CheckByEmail(model.Email)) != null)
            {
                response.Errors.Add("userError", "You can't sumbit inquiries to your own site");

                return Json(response);
            }

            if (!await _inquiryService.Create(model.ToInquiry()))
            {
                response.Errors.Add("serverError", "Oops, we have encountered a problem");

                return Json(response);
            }

            response.Success = true;

            return Json(response);
        }

        #endregion Public Methods/Actions
    }
}