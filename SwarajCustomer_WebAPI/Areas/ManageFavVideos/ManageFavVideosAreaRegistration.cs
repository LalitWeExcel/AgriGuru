using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManageFavVideos
{
    public class ManageFavVideosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManageFavVideos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManageFavVideos_default",
                "ManageFavVideos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}