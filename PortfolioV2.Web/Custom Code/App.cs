using Microsoft.AspNetCore.Mvc.Razor;

namespace PortfolioV2.Web
{
    public abstract class App<TModel> : RazorPage<TModel>
    {
        public static bool IsDeployed { get; set; }

        public string Minified()
        {
            return IsDeployed ? ".min" : "";
        }
    }
}