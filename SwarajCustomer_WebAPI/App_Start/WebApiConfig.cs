﻿using System.Web.Http;

namespace SwarajCustomer_WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
