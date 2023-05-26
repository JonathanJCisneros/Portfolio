using PortfolioV2.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using MySqlConnector;

namespace PortfolioV2.Web.Areas
{
    [Area("api")]
    public class TestController : Controller
    {
        public async Task<JsonResult> TestDb()
        {

            return Json("sucess");
        }
    }
}
