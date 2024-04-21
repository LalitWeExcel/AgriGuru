﻿using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Masters
{
    public class MastersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Masters";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Masters_default",
                "Masters/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}