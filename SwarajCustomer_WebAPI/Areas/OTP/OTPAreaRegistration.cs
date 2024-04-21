using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.OTP
{
    public class OTPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OTP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OTP_default",
                "OTP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}