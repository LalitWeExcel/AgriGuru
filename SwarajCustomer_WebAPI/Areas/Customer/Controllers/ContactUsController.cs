using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_WebAPI.Authorization;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("BookAstrologer")]
    [SessionFilterAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ContactUsController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserBAL _userService;

        [HttpGet]
        public ActionResult Index()
        {
            GetBaseUrl();
            return View();
        }
        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}