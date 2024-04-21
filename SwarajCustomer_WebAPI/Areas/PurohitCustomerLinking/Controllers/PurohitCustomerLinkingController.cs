using SwarajCustomer_BAL.Interface.ManageUser;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.PurohitCustomerLinking.Controllers
{
    [RoutePrefix("PurohitCustomerLinking")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PurohitCustomerLinkingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IManageUserBAL _manageUserService;

        [HttpGet]
        public ActionResult Index()
        {
            _manageUserService = new ManageUserBAL();

            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            ViewBag.GetAllUser = _manageUserService.GetAllUser();

            return View();
        }

        [HttpGet]
        public ActionResult GetLinkingGridView(int page = 1, int noofrecords = 10, string search = "", int user_id = 0)
        {
            _manageUserService = new ManageUserBAL();
            int totalRecords;
            int totalProhits;
            int totalAstrollgers;
            int totalCustomers;

            var model = new ManageUserViewModel();
            model.ManageUserList = _manageUserService.GetPurohitCustomerLinkingList(page, noofrecords, search, user_id, out totalRecords,out totalProhits, out totalAstrollgers, out totalCustomers);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;


            model.TotalProhits = totalProhits;
            model.TotalAstrollgers = totalAstrollgers;
            model.TotalCustomers = totalCustomers;

            return PartialView("_Index", model);
        }
    }
}