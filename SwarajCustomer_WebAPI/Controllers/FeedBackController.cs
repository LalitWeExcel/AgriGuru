using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("FeedBack")]
    public class FeedBackController : ApiController
    {
        private IFeedBackBAL _objFeedBack;
        UserModal.DataModal result = null;

        [HttpGet]
        [Route("GetFeedBack")]
        public UserModal.DataModal GetFeedBack()
        {
            try
            {
                _objFeedBack = new FeedBackBAL();
                BasicAuthenticationAttribute.MyPrincipal prin = System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = _objFeedBack.GetFeedBack(user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "FeedBack/GetFeedBack");
            }
        }

        [HttpPost]
        [Route("SaveFeedBack")]
        public UserModal.DataModal SaveFeedBack(SaveFeedbackRequest request)
        {
            try
            {
                _objFeedBack = new FeedBackBAL();
                BasicAuthenticationAttribute.MyPrincipal prin = System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = _objFeedBack.SaveFeedBack(request.Feedback, user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "FeedBack/SaveFeedBack");
            }
        }

        [HttpPost]
        [Route("SaveRating")]
        public UserModal.DataModal SaveRating(RatingEntity request)
        {
            try
            {
                _objFeedBack = new FeedBackBAL();
                BasicAuthenticationAttribute.MyPrincipal prin = System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = _objFeedBack.SaveRating(request),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "FeedBack/SaveRating");
            }
        }
    }
}
