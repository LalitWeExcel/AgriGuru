using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_DAL.Common;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("Customer")]
    public class AccountController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ILoginBAL objLogin = null;
        private INotificationsBAL _notifications = null;
        private IBookingBAL _bookingService = null;

        [HttpGet]
        public ActionResult Login()
        {
            GetBaseUrl();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();

            var model = new CustomerLoginModel();
            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(CustomerLoginModel model, FormCollection collection)
        {
         
            objLogin = new LoginBAL();
            GetBaseUrl();

            var OTP_Decrypt = EncryptDecrypt.Decrypt("wjFCeY0II0U=");
            try
            {

                if (ModelState.IsValid)
                {

                    var Result = objLogin.CustomerLogin(model.PhoneNumber);

                    if (Result.UserId > 0)
                    {
                        Session[SystemVariables.UserId] = Result.UserId;
                        Session[SystemVariables.UserName] = Result.Email;
                        Session[SystemVariables.RoleId] = Result.RoleId;
                        Session[SystemVariables.RoleName] = Result.RoleName;

                        Session[SystemVariables.OTP] = "0000";//Result.OTP;
                        Session[SystemVariables.PhoneNumber] = Result.MobileNumber;
                        Session[SystemVariables.ImageName] = Result.ImageName;
                        
                        Session[SystemVariables.Authorization] = CommonMethods.Base64Encode(Result.MobileNumber);
                        SMSUtility.SendSMS(Result.MobileNumber, Extensions.OTP_SMS_CUST(Result.OTP).ToString());
                        string allLoginData = Result.UserId + "," + Result.UserName + "," + Result.Email
                            + "," + Result.RoleId + "," + Result.MobileNumber;

                        var ticket = new FormsAuthenticationTicket(1, allLoginData, DateTime.Now, DateTime.Now.AddHours(24), true, allLoginData);
                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                        return RedirectToAction("OTP", "Account", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("PhoneNumber", "Please enter valid Mobile Number");
                     
                    }
                }
                else
                {
                    var modelErrors = new System.Collections.Generic.List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
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

        [HttpGet]
        public ActionResult OTP()
        {
            var userId = Convert.ToInt32(Session[SystemVariables.UserId]);

            if (userId > 0)
            {
                GetBaseUrl();
				var model = new CustomerOTPModel
				{
					UserId = userId,
					PhoneNumber = Convert.ToString(Session[SystemVariables.PhoneNumber])
				};
				return View(model);
            }
            return RedirectToAction("Login", "Account", new { area = "Customer" });

        }

        [HttpPost]
        public ActionResult OTP(CustomerOTPModel model, FormCollection collection)
        {
            _notifications = new NotificationsBAL();
            _bookingService = new BookingBAL();
            GetBaseUrl();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.OTP == Convert.ToString(Session[SystemVariables.OTP]))
                    {
                        Session[SystemVariables.Latitude] = (double)model.Latitude;
                        Session[SystemVariables.Longitude] = (double)model.Longitude;

                        return RedirectToAction("Index", "DashBoard", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("OTP", "Please enter valid OTP Number !!");
                    }
                }
                else
                {
                    var modelErrors = new System.Collections.Generic.List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
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
        [Route("ResendOTP")]
        public JsonResult ResendOTP(string PhoneNumber)
        {
            GetBaseUrl();
            objLogin = new LoginBAL();
            string OTP = objLogin.ResendOTP(PhoneNumber, true);
            Session[SystemVariables.OTP] = OTP;

            return Json(OTP, JsonRequestBehavior.AllowGet);
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
            return RedirectToAction("Login", "Account", new { area = "Customer" });
        }
        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }

    }
}