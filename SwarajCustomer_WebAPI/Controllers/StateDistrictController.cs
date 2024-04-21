using SwarajCustomer_BAL.Interface.DashBoard;
using SwarajCustomer_Common;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Controllers
{
    [RoutePrefix("StateDistrict")]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class StateDistrictController : Controller
    {
         
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDashBoardBAL _dashBoardService = null;

        [HttpGet]
        public JsonResult GetState()
        {
            GetBaseUrl();
            _dashBoardService = new DashBoardBAL();
            var Get = _dashBoardService.GetState();
            return Json(Get, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDistrict(int Ids)
        {
            GetBaseUrl();
            _dashBoardService = new DashBoardBAL();
            var Get = _dashBoardService.GetDistrict(Ids);
            return Json(Get, JsonRequestBehavior.AllowGet);
        }

        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}