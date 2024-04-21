
using ImageResizer;
using Newtonsoft.Json;
using SwarajCustomer_BAL.Interface.Advertising;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Advertising.Controllers
{
    [RoutePrefix("Advertising")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AdvertisingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAdvertisingBAL _advertisingservcie;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";// CommonMethods.AdvertismentIcon;
            return View();
        }

        [HttpGet]
        public ActionResult GetAdvertisementGridView(int page = 1, int noofrecords = 10, string search = "", string type = "", int state_id = 0, int district_id = 0)
        {
            _advertisingservcie = new AdvertisingBAL();
            int totalRecords;
            var model = new AdvertisementViewModel();
            model.AdvertisementList = _advertisingservcie.GetAdvertisementList(page, noofrecords, search, type, state_id, district_id, out totalRecords);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;
            return PartialView("_Index", model);
        }

        [HttpGet]
        public ActionResult AddEditAds(int Id)
        {
            ViewBag.AdvertisementId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _advertisingservcie = new AdvertisingBAL();
            int result = _advertisingservcie.Delete(id, "A");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCategorys()
        {
            _advertisingservcie = new AdvertisingBAL();
            var Get = _advertisingservcie.GetCategorys();
            return Json(Get, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterCategorys(int Ids)
        {
            _advertisingservcie = new AdvertisingBAL();
            var Get = _advertisingservcie.GetMasterCategorys(Ids);
            return Json(Get, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTimeSlot()
        {
            _advertisingservcie = new AdvertisingBAL();
            var Get = _advertisingservcie.GetTimeSlot();
            return Json(Get, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get(int ids)
        {
            _advertisingservcie = new AdvertisingBAL();
            var model = _advertisingservcie.Get(ids);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUpdate(M_SaveAdvertisement model)
        {
            _advertisingservcie = new AdvertisingBAL();
            var result = new M_ResponceResult();
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

                        string folder_path = Server.MapPath(string.Format(CommonMethods.AdvertismentSavePath));
                        //!CommonMethods.BaseUrl.Contains("localhost")
                        if (CommonMethods.BaseUrl.Contains("netsmartz"))
                        {
                            // use only when buid for local
                            string imgpath = CommonMethods.AdvertismentSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("bcone"))
                        {
                            //AgriGuru  use only when buid for QA  PujaGuru
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("swarajcdms"))
                        {
                            //AgriGuru  use only when buid for live PujaGuru
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentSavePath;
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


                model.MuiltipleDistrictList = JsonConvert.DeserializeObject<List<MuiltipleDistrict>>(Request.Form["MuiltipleDistrictList"]);

                result = _advertisingservcie.SaveUpdate(model, Convert.ToInt32(Session[SystemVariables.UserId]));

                if (result.Result > 0 && model.AdvertisementId == 0)
                    errorMessage = "Advertising saved successfully.";
                else if (result.Result > 0 && model.AdvertisementId > 0)
                    errorMessage = "Advertising updated successfully.";
                else if (result.Result == -1)
                    errorMessage = "Advertising already exists.";
                else
                    errorMessage = "Error occured while saving Advertising Record.";
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveUpdateIndependentads(M_SaveIndependentads model)
        {
            _advertisingservcie = new AdvertisingBAL();
            var result = new M_ResponceResult();
            string errorMessage = string.Empty;

            //HttpFileCollectionBase files = Request.Files;
            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFileBase file = files[i];

            //}

            if (Request.Files.Count > 0)
            {

                string msg = FileUploadHelper.ValidateFile(System.Web.HttpContext.Current, CommonMethods.ImageExtensions(), CommonMethods.ImageSizeMaximum, CommonMethods.ImageSizeLimitText);

                string vedio_msg = FileUploadHelper.ValidateVedioFile(System.Web.HttpContext.Current, CommonMethods.VideoExtensions(), CommonMethods.VideoSizeMaximum, CommonMethods.VideoSizeLimitText, 1);

                //var postedFile = Request.Files[0];
                //var vediofile = Request.Files[1];
                var postedFile = Request.Files["fileImage"];
                var vediofile = Request.Files["fileVideo"];

                if (string.IsNullOrEmpty(msg))
                {
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                        string filename = DateTime.Now.Ticks.ToString() + extension;
                        model.ImageName = filename;

                        string folder_path = Server.MapPath(string.Format(CommonMethods.AdvertismentSavePath));
                        //!CommonMethods.BaseUrl.Contains("localhost")
                        if (CommonMethods.BaseUrl.Contains("netsmartz"))
                        {
                            // use only when buid for local
                            string imgpath = CommonMethods.AdvertismentSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("bcone"))
                        {
                            //AgriGuru  use only when buid for QA AgriGuru
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentSavePath;
                            folder_path = Server.MapPath(imgpath);
                        }
                        else if (CommonMethods.BaseUrl.Contains("swarajcdms"))
                        {
                            //AgriGuru  use only when buid for live AgriGuru
                            string imgpath = "/AgriGuru" + CommonMethods.AdvertismentSavePath;
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

                if (string.IsNullOrEmpty(vedio_msg))
                {
                    if (!string.IsNullOrEmpty(vediofile.FileName))
                    {
                        string extension = vediofile.FileName.Substring(vediofile.FileName.LastIndexOf('.')).ToLower();
                        string filename = DateTime.Now.Ticks.ToString() + extension;
                        model.VideoName = filename;

                        string folder_path = Server.MapPath(string.Format(CommonMethods.AdvertismentVideosSavePath));
                        //!CommonMethods.BaseUrl.Contains("localhost")
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
                        Request.Files[1].SaveAs(file_path);

                    }
                }
                else
                {
                    errorMessage = "Invalid vedio uploaded";
                }
            }


            if (string.IsNullOrEmpty(errorMessage))
            {
                if (!string.IsNullOrWhiteSpace(model.ImageName))
                    model.ImageName = System.IO.Path.GetFileName(model.ImageName);

                if (!string.IsNullOrWhiteSpace(model.VideoName))
                    model.VideoName = System.IO.Path.GetFileName(model.VideoName);

                model.MRP = decimal.Round(Convert.ToDecimal(model.MRP), 2);
                result = _advertisingservcie.SaveUpdateIndependentads(model, Convert.ToInt32(Session[SystemVariables.UserId]));

                if (result.Result > 0 && model.AdvertisementId == 0)
                {
                    errorMessage = "Independent ads saved successfully.";
                }

                else if (result.Result > 0 && model.AdvertisementId > 0)
                    errorMessage = "Independent ads updated successfully.";

                else if (result.Result == -1)
                    errorMessage = "Independent ads already exists.";
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

            var result = _advertisingservcie.ActivateDeactivate(Ids, IsActive, admin_id, "A");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private static string AutoImageCropper(string extension, string folder_path, string file_path)
        {
            ResizeSettings settings = new ResizeSettings("quality=100&width=200&height=200&format=" + extension + "&crop=auto");
            string fileName = Path.Combine(folder_path, DateTime.Now.Ticks.ToString());
            fileName = ImageBuilder.Current.Build(file_path, fileName, settings, false, true);
            return fileName;
        }
        private static BasicAuthenticationAttribute.MyPrincipal Identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}