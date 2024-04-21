using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_WebAPI.Areas.Account.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SwarajCustomer_WebAPI.Areas.Account.Controllers
{
    [RoutePrefix("User")]
    public class UserController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ILoginBAL objLogin;


        [AllowAnonymous]
        public ActionResult Login()
        {
            GetBaseUrl();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            LoginModel model = new LoginModel();
            return View(model);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model, FormCollection collection)
        {
            objLogin = new LoginBAL();
            try
            {

                if (ModelState.IsValid)
                {
                    GetBaseUrl();
                    var Result = objLogin.AdminLogin(model.Email, model.Password);

                    if (Result.UserId > 0)
                    {
                        Session[SystemVariables.UserId] = Result.UserId; ;
                        Session[SystemVariables.UserName] = Result.Email;
                        Session[SystemVariables.RoleId] = Result.RoleId;
                        Session[SystemVariables.RoleName] = Result.RoleName;
                        Session[SystemVariables.Password] = Result.Password;


                        string allLoginData = Result.UserId + "," + Result.UserName + "," + Result.Email
                            + "," + Result.RoleId + "," + Result.RoleName;

                        var ticket = new FormsAuthenticationTicket(1, allLoginData, DateTime.Now, DateTime.Now.AddHours(24), true, allLoginData);

                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                        return RedirectToActionPermanent("Index", "DashBoard", new { Area = "DashBoard" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", CommonMethods.InvalidLogin);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
            }


            return View(model);
        }

        [AllowAnonymous]
        public ActionResult UnAuthorized()
        {
            GetBaseUrl();
            return View("UnAuthorized");
        }
        [AllowAnonymous]
        public ActionResult SessionExpire()
        {
            GetBaseUrl();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            return View("SessionExpire");
        }
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            GetBaseUrl();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public JsonResult ForgotPassword(string EmailID)
        {
            objLogin = new LoginBAL();
            var message = string.Empty;
            GetBaseUrl();

            var getUser = objLogin.GetUserByEamilReferalCode(EmailID, null);

            if (!string.IsNullOrEmpty(getUser.Email))
            {
                DateTime expires = DateTime.Now + TimeSpan.FromMinutes(15);
                string hash = CommonMethods.MakeExpiryHash(expires);

                var verifyUrl = string.Format("/Account/User/ResetPassword?e={0}&k={1}&c={2}", expires.ToString("s"), hash, getUser.ReferalCode);
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);


                getUser.ReferalCode = getUser.ReferalCode;
                var subject = "Password Reset Request";
                var body = "Hi " + getUser.Username + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +
                     " <br/><br/><a href='" + link + "'>" + "click here !!" + "</a> <br/><br/>" +
                     "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                CommonMethods.SendHtmlMail(new string[] { getUser.Email }, new string[] { CommonMethods.EmailCC }, subject, body, 0);

                message = "Reset password link has been sent successfully to your email id.";
            }
            else
            {
                message = "user email doesn't exists.";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetPassword(string e, string k, string c)
        {
            GetBaseUrl();
            objLogin = new LoginBAL();

			var model = new UserDataModel
			{
				ReferalCode = c,
				expired = e,
				keys = k
			};

			DateTime expires = DateTime.Parse(e);
            string hash = CommonMethods.MakeExpiryHash(expires);

            if (k == hash)
            {
                if (expires < DateTime.UtcNow)
                {
                    model.IsVaild = "N";
                    model.Message = "Link has expired !!  Please forgot password again.  Thanks ";
                }
                else
                {
                    model.IsVaild = "Y";
                    model.Message = "success";
                }
            }
            else
            {
                model.IsVaild = "N";
                model.Message = "Invalid link !! Please select vaild link.  Thanks ";
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(UserDataModel model)
        {
            GetBaseUrl();
            objLogin = new LoginBAL();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = objLogin.GetUserByEamilReferalCode(null, model.ReferalCode);
                    if (user != null)
                    {
                        var restpassword = objLogin.UpdatePassword(user.UserID, model.Password, model.ReferalCode);
                        return RedirectToAction("Login", "User");
                    }
                }
                else 
                {
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            //modelErrors.Add(modelError.ErrorMessage);
                            ModelState.AddModelError("error", modelError.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangePassword(UserDataModel model)
        {
            GetBaseUrl();
            objLogin = new LoginBAL();
            var result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objLogin.UpdatePassword(model.UserID, model.Password, model.ReferalCode);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}