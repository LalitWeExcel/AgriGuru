using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Web.Mvc;
namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("MyBooking")]
    [SessionFilterAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class MyBookingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IBookingBAL _bookingServcie=null;
        private ILoginBAL objLogin = null;

        [HttpGet]
        public ActionResult Index()
        {
            GetBaseUrl();
            return View();
        }

        [HttpGet]
        public ActionResult GetMyBookings()
        {
            GetBaseUrl();
            _bookingServcie = new BookingBAL();
            var _request = new MyBookingsReq();
            _request.UserID = Convert.ToInt32(Session[SystemVariables.UserId]);

            var model = _bookingServcie.GetMyBookings(_request);
            return PartialView("_Index", model);
        }


        [HttpPost]
        public JsonResult SaveReminder(RemimnderReq req)
        {
            GetBaseUrl();
            objLogin = new LoginBAL();
            var message = string.Empty;
            var getUser = objLogin.GetUserByEamilReferalCode(req.Email, null);

            if (!string.IsNullOrEmpty(getUser.Email))
            {
                getUser.ReferalCode = getUser.ReferalCode;
                var subject = "Puja Booking Reminder Request";
                var body = "Hi " + getUser.Username + ", <br/> You recently requested to Puja Booking Reminder Puja Name" + req.PujaName  + "<br/>" + "OrderNumber :" + req.OrderNumber + "<br/>" + "<br/>"+ "Date :" + req.Date + "<br/>"+ "Time :" + req.Time +  "<br/><br/>" +
                     "please ignore this email or reply to let us know.<br/><br/> Thank you";
                CommonMethods.SendHtmlMail(new string[] { getUser.Email }, new string[] { CommonMethods.EmailCC }, subject, body, 0);
                message = req.PujaName  + " Reminder sent successfully to your email id.";
            }
            else
            {
                message = "user email doesn't exists.";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}