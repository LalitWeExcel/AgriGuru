using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.PurohitCustomerLinking
{
    public class PurohitCustomerLinkingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PurohitCustomerLinking";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PurohitCustomerLinking_default",
                "PurohitCustomerLinking/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}