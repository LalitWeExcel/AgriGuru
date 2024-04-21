using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Models
{
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private UserModal.DataModal result;

        private Boolean IsUserValid(Dictionary<string, string> credentials)
        {
            ILoginBAL _loginBal = new LoginBAL();
            if (_loginBal.Authenticate(credentials["UserName"], credentials["Password"]) == "true")
            {
                //if (credentials["UserName"].Equals("joydip") && credentials["Password"].Equals("joydip123"))
                return true;
            }
            return false;
        }

        private Dictionary<string, string> ParseRequestHeaders(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Dictionary<string, string> credentials = new Dictionary<string, string>();
            var httpRequestHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            httpRequestHeader = httpRequestHeader.Substring("Authorization".Length);
            string[] httpRequestHeaderValues = httpRequestHeader.Split(':');
            string username = Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeaderValues[0]));
            //string password = Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeaderValues[1]));
            string password = Convert.ToString(httpRequestHeaderValues[1]);
            credentials.Add("UserName", username);
            credentials.Add("Password", password);
            //credentials.Add("Password", password);

            return credentials;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                using (SwarajTestEntities _databaseContext = new SwarajTestEntities())
                {
                    Exception ex = new Exception();

                    string RoleName = string.Empty;
                    if (actionContext.Request.Headers.Authorization == null)
                    {
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        var httpRequestHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                        var httpRequestHeader_Role = actionContext.Request.Headers.GetValues("Role").FirstOrDefault();
                        httpRequestHeader = httpRequestHeader.Substring("Basic".Length);
                        RoleName = Convert.ToString(httpRequestHeader_Role);
                        if (RoleName == Roles.CUST.ToString())
                            RoleName = Convert.ToString(AppType.CUST);
                        if (RoleName == Roles.PRHT.ToString())
                            RoleName = Convert.ToString(AppType.PRHT);
                        if (RoleName == Roles.AST.ToString())
                            RoleName = Convert.ToString(AppType.AST);
                        if (RoleName == Roles.ADMIN.ToString())
                            RoleName = Convert.ToString(AppType.ADMIN);
                        if (RoleName == Roles.PPRHT.ToString())
                            RoleName = Convert.ToString(AppType.PPRHT);
                        if (RoleName == Roles.PAST.ToString())
                            RoleName = Convert.ToString(AppType.PAST);

                        string username = Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeader));
                        string actionName = actionContext.ActionDescriptor.ActionName;
                        string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                        UserLoginEntity objUser = CommonDBMethods.GetUserAuthObject(username, httpRequestHeader_Role);
                        if (objUser != null && objUser.UserID != 0)
                        {
                            MyPrincipal principal = new MyPrincipal(new GenericIdentity(username), new string[] { "Affiliate" });
                            principal.UserDetails = objUser;
                            Thread.CurrentPrincipal = principal;
                            HttpContext.Current.User = principal;
                        }
                        else
                        {
                            //  actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                            actionContext.Response = actionContext.Request.CreateResponse(
                                System.Net.HttpStatusCode.Unauthorized,
                                new UserModal.DataModal()
                                {
                                    Data = null,
                                    Response = CommonActions.Unauthorized.ToString(),
                                    Message = Extensions.Unauthorized(),
                                    Status = false,
                                },
                                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                APIExceptionHandler.ExceptionLogging(ex, "MarketRecovery/SearchDetails");
                if (ex.Message.ToString() == "The given header was not found.")
                {
                    var httpRequestHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                    var httpRequestHeader_Role = actionContext.Request.Headers.GetValues("Role").FirstOrDefault();
                    httpRequestHeader = httpRequestHeader.Substring("Basic".Length);
                    string[] httpRequestHeaderValues = httpRequestHeader.Split(':');

                    string username = Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeaderValues[0]));
                    string password = Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeaderValues[1]));

                    string actionName = actionContext.ActionDescriptor.ActionName;
                    string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    UserLoginEntity objUser = CommonDBMethods.GetUserAuthObject(username, httpRequestHeader_Role);
                    if (objUser != null && objUser.UserID != 0)
                    {
                        //if (httpRequestHeader_Version == "1.3")
                        //{
                        MyPrincipal principal = new MyPrincipal(new GenericIdentity(username), new string[] { "Affiliate" });
                        principal.UserDetails = objUser;
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;

                        Int32 userID = objUser.UserID;
                    }
                }
                else
                {
                    APIExceptionHandler.ExceptionLogging(ex, "MarketRecovery/SearchDetails");
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public class MyPrincipal : GenericPrincipal
        {
            public MyPrincipal(IIdentity identity, string[] roles) : base(identity, roles)
            { }
            public UserLoginEntity UserDetails { get; set; }
        }
    }
}