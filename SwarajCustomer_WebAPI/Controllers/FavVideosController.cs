using SwarajCustomer_BAL.Interface.Advertising;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [RoutePrefix("FavVideos")]
    public class FavVideosController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAdvertisingBAL _advertisingservcie;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("GetFavVideos")]
        public UserModal.DataModal GetFavVideos(int LanguageId = 0)
        {
            try
            {
                return new UserModal.DataModal()
                {
                    Data = new AdvertisingBAL().GetFavVideos(LanguageId),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetFavVideos");
            }
        }
    }
}
