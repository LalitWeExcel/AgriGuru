using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Customer;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("DashBoard")]
    [SessionFilterAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DashBoardController : Controller
    {
       private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private INotificationsBAL _notifications = null;
        private IBookingBAL _bookingService = null;
        private IUserBAL _userService = null; 

        [HttpGet]
        public ActionResult Index(string Language = "")
        {
            var model = new CustomerDashBoardContent();
            _notifications = new NotificationsBAL();
            _bookingService = new BookingBAL();
            _userService = new UserBAL();
            GetBaseUrl();

            double Latitude =    (double)Session[SystemVariables.Latitude];
            double Longitude =   (double)Session[SystemVariables.Longitude];

            var request = new MasterRequest(); request.Latitude = Latitude; request.Longitude = Longitude;
            request.Language = Language;

            int user_Id = Convert.ToInt32(Session[SystemVariables.UserId]);
            Session[SystemVariables.M_Notifications] = _notifications.GetNotificationsByUser(user_Id);
            Session[SystemVariables.MyCart] = _bookingService.MyCart(user_Id);
            Session[SystemVariables.Masters] = _userService.GetMasters(user_Id, request);

            model.GetDashBoardAdvertisements = _userService.GetDashBoardAdvertisements(Latitude, Longitude, "");
            model.GetTopAstrologersPurohits = _userService.GetTopAstrologersPurohits(Latitude, Longitude);

            return View(model);
        }

        [HttpGet]
        public ActionResult Notification()
        {
            GetBaseUrl();
            var model = new List<NotificationsEntity>();
            model = (List<NotificationsEntity>)Session[SystemVariables.M_Notifications];
            return View(model);
        }


        [HttpGet]
        public ActionResult MyCart()
        {
            GetBaseUrl();
            var model = new MyCartResponce();
            model = (MyCartResponce)Session[SystemVariables.MyCart];
            return View(model);
        }

        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}