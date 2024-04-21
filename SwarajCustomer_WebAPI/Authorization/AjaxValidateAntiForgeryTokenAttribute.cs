using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
namespace SwarajCustomer_WebAPI.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AjaxValidateAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        public const string TOKEN_NAME = "AjaxToken";
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            //  Only validate POSTs
            if (request.IsAjaxRequest() && request.HttpMethod == WebRequestMethods.Http.Post)
            {
                var headerTokenValue = request.Headers[TOKEN_NAME];
                // Ajax POSTs using jquery have a header set that defines the token.
                // However using unobtrusive ajax the token is still submitted normally in the form.
                // if the header is present then use it, else fall back to processing the form like normal
                if (headerTokenValue != null)
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;
                    AntiForgery.Validate(cookieValue, headerTokenValue);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
}