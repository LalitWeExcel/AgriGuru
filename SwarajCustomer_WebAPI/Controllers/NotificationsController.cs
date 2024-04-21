using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("Notifications")]
    public class NotificationsController : ApiController
    {
        private INotificationsBAL _notifications;
        UserModal.DataModal result = null;

        [HttpGet]
        [Route("GetNotificationsByUser")]
        public UserModal.DataModal GetNotificationsByUser()
        {
            try
            {
                _notifications = new NotificationsBAL();
                BasicAuthenticationAttribute.MyPrincipal prin = System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = _notifications.GetNotificationsByUser(user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Notifications/GetNotificationsByUser");
            }
        }
    }
}
