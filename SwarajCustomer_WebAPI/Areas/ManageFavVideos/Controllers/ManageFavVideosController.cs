using SwarajCustomer_BAL.Interface.Advertising;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManageFavVideos.Controllers
{

    [RoutePrefix("ManageFavVideos")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ManageFavVideosController : Controller
    {
  
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAdvertisingBAL _advertisingservcie;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            return View();
        }

        [HttpGet]
        public ActionResult GetManageFavVideostGridView(int page = 1, int noofrecords = 10, string search = "",int languageId=0)
        {
            _advertisingservcie = new AdvertisingBAL();
            int totalRecords;
            var model = new ManageFavVideosViewModel();
            model.ManageFavVideosList = _advertisingservcie.ManageFavVideosList(page, noofrecords, search, languageId, out totalRecords);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;
            return PartialView("_Index", model);
        }

        [HttpPost]
        public JsonResult SaveManageFavVideos(M_ManageFavVideos model)
        {
            _advertisingservcie = new AdvertisingBAL();
            var result = new M_ResponceResult();
            string errorMessage = string.Empty;

            if (Request.Files.Count > 0)
            {
                string vedio_msg = FileUploadHelper.ValidateVedioFile(System.Web.HttpContext.Current, CommonMethods.VideoExtensions(), CommonMethods.VideoSizeMaximum, CommonMethods.VideoSizeLimitText,0);
                var vediofile = Request.Files[0];

                if (string.IsNullOrEmpty(vedio_msg))
                {
                    if (!string.IsNullOrEmpty(vediofile.FileName))
                    {
                        string extension = vediofile.FileName.Substring(vediofile.FileName.LastIndexOf('.')).ToLower();
                        string filename = DateTime.Now.Ticks.ToString() + extension;
                        model.VideoName = filename;

                        string folder_path = Server.MapPath(string.Format(CommonMethods.AdvertismentVideosSavePath));
                  
                        if (CommonMethods.BaseUrl.Contains("netsmartz"))
                        {
                            // use only when buid for local
                            string imgpath = CommonMethods.AdvertismentVideosSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("bcone"))
                        {
                            //AgriGuru  use only when buid for QA
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentVideosSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("swarajcdms"))
                        {
                            //AgriGuru  use only when buid for live
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentVideosSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }

                        string file_path = folder_path + model.VideoName;

                        Directory.CreateDirectory(folder_path);
                        if (System.IO.File.Exists(file_path))
                            System.IO.File.Delete(file_path);
                        Request.Files[0].SaveAs(file_path);

                    }
                }
                else
                {
                    errorMessage = "Invalid vedio uploaded";
                }
            }


            if (string.IsNullOrEmpty(errorMessage))
            {
                if (!string.IsNullOrWhiteSpace(model.VideoName))
                    model.VideoName = System.IO.Path.GetFileName(model.VideoName);
                result = _advertisingservcie.SaveManageFavVideos(model, Convert.ToInt32(Session[SystemVariables.UserId]));

                if (result.Result > 0 && model.ManageFavVideosId == 0)
                    errorMessage = "Manage fav vedios saved successfully.";
                else if (result.Result > 0 && model.ManageFavVideosId > 0)
                    errorMessage = "Manage fav vedios updated successfully.";
                else if (result.Result == -1)
                    errorMessage = "Manage fav vedios already exists.";
                else
                    errorMessage = "Error occured while saving Independent ads Record.";
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivateDeactivate(List<M_ActiveDisActive> advertising_ids, string IsActive)
        {
            _advertisingservcie = new AdvertisingBAL();
            int admin_id = Convert.ToInt32(Session[SystemVariables.UserId]);

            string[] the_array = advertising_ids.Select(i => i.Id.ToString()).ToArray();
            string Ids = String.Join(", ", the_array).Trim();

            var result = _advertisingservcie.ActivateDeactivate(Ids, IsActive, admin_id,"F");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _advertisingservcie = new AdvertisingBAL();
            int result = _advertisingservcie.Delete(id,"F");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLanguages()
        {
            _advertisingservcie = new AdvertisingBAL();
            var Get = _advertisingservcie.GetLanguages();
            return Json(Get, JsonRequestBehavior.AllowGet);
        }
    }
}