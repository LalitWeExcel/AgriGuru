using ImageResizer;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_BAL.Interface.ManageUser;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.IO;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("MyProfile")]
    [SessionFilterAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class MyProfileController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IManageUserBAL _manageUserService;

        [HttpGet]
        public ActionResult Index()
        {
            GetBaseUrl();
            _manageUserService = new ManageUserBAL();
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";

            int user_id = Convert.ToInt32(Session[SystemVariables.UserId]);
            var model = _manageUserService.GetManageUserDetails(user_id);
 
            Session[SystemVariables.IsActive] = model.is_active;
            Session[SystemVariables.ImageName] = model.ImageName;
            Session[SystemVariables.UserDistrictId] = model.mst_district_id;
            Session[SystemVariables.UserStateId] = model.mst_state_id;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int user_id)
        {
            GetBaseUrl();
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Edit";
            _manageUserService = new ManageUserBAL();
            var model = _manageUserService.GetManageUserDetails(user_id);

            Session[SystemVariables.IsActive] = model.is_active;
            Session[SystemVariables.ImageName] = model.ImageName;
            Session[SystemVariables.UserDistrictId] = model.mst_district_id;
            Session[SystemVariables.UserStateId] = model.mst_state_id;

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(M_ManageUserDetails model)
        {
            GetBaseUrl();
            _manageUserService = new ManageUserBAL();
            string errorMessage = string.Empty;
            if (Request.Files.Count > 0)
            {

                string msg = FileUploadHelper.ValidateFile(System.Web.HttpContext.Current, CommonMethods.ImageExtensions(), CommonMethods.ImageSizeMaximum, CommonMethods.ImageSizeLimitText);
                var postedFile = Request.Files[0];
                if (string.IsNullOrEmpty(msg))
                {
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                        string filename = DateTime.Now.Ticks.ToString() + extension;
                        model.ImageName = filename;

                        string folder_path = Server.MapPath(string.Format(CommonMethods.CustomerSavePath));

                        if (CommonMethods.BaseUrl.Contains("netsmartz"))
                        {
                            // use only when buid for local
                            string imgpath = CommonMethods.CustomerSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("bcone"))
                        {
                            //AgriGuru  use only when buid for QA  PujaGuru
                            string imgpath = "/AgriGuru" + CommonMethods.CustomerSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("swarajcdms"))
                        {
                            //AgriGuru  use only when buid for live PujaGuru
                            string imgpath = "/AgriGuru" + CommonMethods.CustomerSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }

                        string file_path = folder_path + model.ImageName;

                        Directory.CreateDirectory(folder_path);
                        if (System.IO.File.Exists(file_path))
                            System.IO.File.Delete(file_path);
                        Request.Files[0].SaveAs(file_path);

                        // image croper 
                        string fileName = AutoImageCropper(extension, folder_path, file_path);
                        model.ImageName = fileName;
                    }
                }
                else
                {
                    errorMessage = "Invalid image uploaded";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (!string.IsNullOrWhiteSpace(model.ImageName))
                    model.ImageName = System.IO.Path.GetFileName(model.ImageName);

                errorMessage = _manageUserService.Update(model, Convert.ToInt32(Session[SystemVariables.UserId]));

            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        private static string AutoImageCropper(string extension, string folder_path, string file_path)
        {
            ResizeSettings settings = new ResizeSettings("quality=100&width=200&height=200&format=" + extension + "&crop=auto");
            string fileName = Path.Combine(folder_path, DateTime.Now.Ticks.ToString());
            fileName = ImageBuilder.Current.Build(file_path, fileName, settings, false, true);
            return fileName;
        }

        private void GetBaseUrl()
        {
            CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }
    }
}