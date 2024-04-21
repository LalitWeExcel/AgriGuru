using System.Web.Mvc;
using System.Web.Routing;

namespace SwarajCustomer_WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            name: "Customer",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }, namespaces: new[] { "SwarajCustomer_WebAPI.Areas.Customer.Account.Controllers" }
             ).DataTokens.Add("area", "Customer");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }, namespaces: new[] { "SwarajCustomer_WebAPI.Areas.Account.Controllers" }
            ).DataTokens.Add("area", "Account");

        }


    }
}
