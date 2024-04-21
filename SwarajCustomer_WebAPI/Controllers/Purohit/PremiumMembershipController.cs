using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers.Purohit
{
    [BasicAuthentication]
    [RoutePrefix("PremiumMembership")]
    public class PremiumMembershipController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IBookingBAL objBooking;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("PremiumMembership")]
        public UserModal.DataModal PremiumMembership(MembershipRequest request)
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
                    Message = objBooking.PremiumMembership(request),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "PremiumMembership/PremiumMembership");
            }
        }

        private static BasicAuthenticationAttribute.MyPrincipal identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}
