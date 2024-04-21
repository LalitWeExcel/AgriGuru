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
    [RoutePrefix("Contact")]
    public class ContactController : ApiController
    {
        private IContactBAL objContactBAL;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("Contact")]
        public UserModal.DataModal Contact(ContactEntity objContact)
        {
            try
            {
                objContactBAL = new ContactBAL();
            
                int response = objContactBAL.ContactUs(objContact);

                if (response == 1)
                {
                    CommonMethods.SendHtmlMail(new string[] { "rajwinder.singh@netsmartz.net", }
                    , new string[] { "venus.saini@netsmartz.net", "lalit.kumar@netsmartz.net" }
                    , Extensions.ContactEmailSubject().ToString(),
                      Extensions.ContactEmailBody(objContact.Name, objContact.Email, objContact.Phone, objContact.Remarks).ToString(), 0).ToString();
                }

                result = new UserModal.DataModal()
                {
                    Data = null,
                    Response = CommonActions.Successful.ToString(),
                    Message = Extensions.ContactRequestReceived().ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetPujaBookedSlots");
            }
        }
    }
}
