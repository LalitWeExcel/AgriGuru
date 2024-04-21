using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManagePayment
{
    public class ManagePaymentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManagePayment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManagePayment_default",
                "ManagePayment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}