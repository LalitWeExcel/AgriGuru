using SwarajCustomer_BAL.Interface.DashBoard;
using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Common;
using SwarajCustomer_WebAPI.Authorization;
using SwarajCustomer_WebAPI.Models;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.DashBoard.Controllers
{
    [RoutePrefix("DashBoard")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DashBoardController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDashBoardBAL _dashBoardBAL;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDashboardData(string start_date, string end_date)
        {
            _dashBoardBAL = new DashBoardBAL();
            var model = new DashBoardViewModel();
            start_date = CommonMethods.FormatDate(start_date, "dd-MM-yyyy", "yyyy-MM-dd");
            end_date = CommonMethods.FormatDate(end_date, "dd-MM-yyyy", "yyyy-MM-dd");
            model = _dashBoardBAL.GetDashBoardUserCount(start_date, end_date);
            return View("_Index", model);
        }


        [HttpGet]
        public ActionResult SendSmsMessage(string mobileNumber, string message)
        {
            var result =   SMSUtility.SendSMS(mobileNumber, message);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetComeingBirthday()
        {
            var  result = new DashBoardBAL().GetComeingBirthday();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static BasicAuthenticationAttribute.MyPrincipal Identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}