using Microsoft.AspNetCore.Mvc.Razor;

namespace PortfolioV2.Web
{
    public abstract class RazorExtensions<TModel> : RazorPage<TModel>
    {
        public static bool IsDeployed 
        { 
            get 
            {
                return App.IsDeployed;
            } 
        }

        public string Minified()
        {
            return IsDeployed ? ".min" : "";
        }
    }
}