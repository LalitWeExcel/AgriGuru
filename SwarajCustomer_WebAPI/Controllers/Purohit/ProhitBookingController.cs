using System;
using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_WebAPI.Models;
using System.Web.Http;
using SwarajCustomer_Common.Purohit;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common;
using static SwarajCustomer_Common.Utility.MessageModel;
using System.Web.Configuration;

namespace SwarajCustomer_WebAPI.Controllers.Purohit
{
    [BasicAuthentication]
    [RoutePrefix("ProhitBooking")]
    public class ProhitBookingController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ApplicationIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdCust"));
        public static string SenderIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdCust"));

        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));
        private INotificationsBAL _notifications;
        private IBookingBAL objBooking;
        UserModal.DataModal result = null;


        [HttpPost]
        [Route("MyBookings")]
        public UserModal.DataModal MyBookings(MyBookingsReq objBookings)
        {
            try
            {
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;

                objBooking = new BookingBAL();
                result = new UserModal.DataModal()
                {
                    Data = objBooking.GetProhitBookings(objBookings.UserID),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "ProhitBooking/MyBookings");
            }
        }


        [HttpPost]
        [Route("BookingVerification")]
        public UserModal.DataModal BookingVerification(OTPRequest request)
        {
            try
            {
                objBooking = new BookingBAL();
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;
                result = new UserModal.DataModal()
                {
                    Data = "",
                    Response = CommonActions.Successful.ToString(),
                    Message = objBooking.BookingVerification(request),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "ProhitBooking/BookingVerification");
            }
        }

        [HttpPost]
        [Route("BookingCancel")]
        public UserModal.DataModal BookingCancel(Responce request)
        {
            try
            {
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;
                request.RejectByProhit = "Y";

                var responce = objBooking.BookingCancel(request, user_Id);
                var GeBookings = objBooking.GetBookings(request.order_number);

                request.Message = Extensions.BookingCancelContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);

                var notification = _notifications.GetPuchNotification(GeBookings.UserID, ContentType.MBC.ToString());

                CommonMethods.OrderNotification(notification, request, ApplicationIdCust, SenderIdCust);
                _notifications.SaveNotifications(GeBookings.PujaName, responce, notification.user_id, notification.contentsId);

   
                 result = new UserModal.DataModal()
                {
                    Data = responce,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "ProhitBooking/BookingCancel");
            }
        }
        [HttpPost]
        [Route("BookingConfirm")]
        public UserModal.DataModal BookingConfirm(Responce request)
        {
            try
            {
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;

                var responce = objBooking.BookingConfirm(request, user_Id);
                var GeBookings = objBooking.GetBookings(request.order_number);

                request.Message = Extensions.BookingConfirmContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);

                var notification = _notifications.GetPuchNotification(GeBookings.UserID, ContentType.BC.ToString());

                CommonMethods.OrderNotification(notification, request, ApplicationIdCust, SenderIdCust);
                _notifications.SaveNotifications(GeBookings.PujaName, responce, notification.user_id, notification.contentsId);


                result = new UserModal.DataModal()
                {
                    Data = responce,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "ProhitBooking/BookingConfirm");
            }
        }


        private static BasicAuthenticationAttribute.MyPrincipal identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}
