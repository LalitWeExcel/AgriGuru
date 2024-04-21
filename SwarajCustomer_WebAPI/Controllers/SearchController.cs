using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("Search")]
    public class SearchController : ApiController
    {
        private ISearchBAL objUser;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("Search")]
        public UserModal.DataModal Search(SearchReq objSearch)
        {
            try
            {
                objUser = new SearchBAL();
                BasicAuthenticationAttribute.MyPrincipal prin = System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;

                List<SearchRes> dash = new List<SearchRes>();
                dash = objUser.GetSearchResult(objSearch.Latitude, objSearch.Longitude, objSearch.Text);
                //dash.purohits_astrologers = objUser.GetTopAstrologersPurohits(objUserLogin.Latitude, objUserLogin.Longitude);

                result = new UserModal.DataModal()
                {
                    Data = dash,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Search/Search");
            }
        }
    }
}
