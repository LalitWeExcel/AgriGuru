using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SwarajCustomer_WebAPI.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase objHttpSessionStateBase = filterContext.HttpContext.Session;
                var userSession = objHttpSessionStateBase["UserId"];
                var RoleSession = Convert.ToString(objHttpSessionStateBase["RoleName"]);

                if (((userSession == null || RoleSession != "Super Admin") && (!objHttpSessionStateBase.IsNewSession)) || (objHttpSessionStateBase.IsNewSession))
                {
                    objHttpSessionStateBase.RemoveAll();
                    objHttpSessionStateBase.Clear();
                    objHttpSessionStateBase.Abandon();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.Result = new JsonResult
                        {
                            Data = new { status = "401" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary
                                   {
                                       { "action", "SessionExpire" },
                                       { "controller", "User" },
                                        {"area", "Account" }
                                   });
                    }
                }
            }
            //base.OnActionExecuting(filterContext);
        }
    }
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpSessionStateBase objHttpSessionStateBase = filterContext.HttpContext.Session;
                var userSession = objHttpSessionStateBase["UserId"];
                var RoleSession = Convert.ToString(objHttpSessionStateBase["RoleName"]);

                if (((userSession == null || RoleSession != "CUST") && (!objHttpSessionStateBase.IsNewSession)) || (objHttpSessionStateBase.IsNewSession))
                {
                    objHttpSessionStateBase.RemoveAll();
                    objHttpSessionStateBase.Clear();
                    objHttpSessionStateBase.Abandon();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.Result = new JsonResult
                        {
                            Data = new { status = "401" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary
                                   {
                                       { "action", "SessionExpire" },
                                       { "controller", "Account" },
                                        {"area", "Customer" }
                                   });
                    }
                }
            }
            //base.OnActionExecuting(filterContext);
        }
    }
}