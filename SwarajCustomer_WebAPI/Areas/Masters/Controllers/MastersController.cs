using ClosedXML.Excel;
using SwarajCustomer_BAL.Interface.Master;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Masters.Controllers
{
    [RoutePrefix("Masters")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class MastersController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IMastersBAL _masterService;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile, BulkUpload obj)
        {
            _masterService = new MastersBAL();
            string file_path = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {

                    string fileExtension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                    if (fileExtension == CommonMethods.EXCEL_2003_EXT || fileExtension == CommonMethods.EXCEL_2007_EXT)
                    {
                        string fileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                        string folder_path = Server.MapPath(CommonMethods.TempExcelUploadPath);

                        if (CommonMethods.BaseUrl.Contains("netsmartz"))
                        {
                            // use only when buid for local
                            string path = CommonMethods.TempExcelUploadPath;
                            folder_path = Server.MapPath(path);
                        }
                        else if (CommonMethods.BaseUrl.Contains("bcone"))
                        {
                            //CDMSQA  use only when buid for QA
                            string path = "/AgriGuru" + CommonMethods.TempExcelUploadPath;
                            folder_path = Server.MapPath(path);
                        }
                        else if (CommonMethods.BaseUrl.Contains("swarajcdms"))
                        {
                            //CDMS  use only when buid for live
                            string path = "/AgriGuru" + CommonMethods.TempExcelUploadPath;

                            log.Info("STEP 1 path : - " + path);
                            folder_path = Server.MapPath(path);
                        }

                        file_path = folder_path + fileName;
                        Directory.CreateDirectory(folder_path);
                        if (System.IO.File.Exists(file_path))
                            System.IO.File.Delete(file_path);
                        Request.Files[0].SaveAs(file_path);

                        string excelConnString = CommonMethods.GetExcelConnectionString(file_path);
                        string[] columns = new string[4];

                        columns = new string[] { "Name"
                            , "MRP"
                            , "Description"
                            , "Duration"
                        };

                        ds = CommonMethods.ImportExcelToDataSet(excelConnString, columns, "Services", 0);

                        if (System.IO.File.Exists(file_path))
                            System.IO.File.Delete(file_path);

                        string xmlString = string.Empty;

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            using (TextWriter writer1 = new StringWriter())
                            {
                                //Here we are remove  totally null rows
                                var dataTable = CommonMethods.RmoveEmptyRows(ds.Tables[0]);
                                dataTable.WriteXml(writer1);
                                xmlString = writer1.ToString();
                                log.Info("STEP 2");
                            }

                            xmlString = Regex.Replace(xmlString, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                            xmlString = Regex.Replace(xmlString, @"\r\n\s+", "");
                            xmlString = Regex.Replace(xmlString, @"\r\n", "");

                            System.Web.HttpContext.Current.Session["Services"] = xmlString;
                            int user_id = Convert.ToInt32(Session[SystemVariables.UserId]);
                            var result = _masterService.SaveServicesBulkUpload(xmlString, obj.CategoryID, user_id);
                            log.Info("STEP 3");
                            if (result.Count > 0)
                            {
                                ViewBag.WrongData = result;
                                ViewBag.ErrorMessage = "Data not valid.";
                                log.Info("STEP 4");
                            }
                        }
                        else
                            ViewBag.ErrorMessage = "No Record(s) Found in the uploaded file.";
                    }
                    else
                        ViewBag.ErrorMessage = "Invalid file. Please upload an excel file with '.xls' (or) '.xlsx' extension.";
                }
                else
                    ViewBag.ErrorMessage = "Please upload a file.";
            }
            catch (Exception ex)
            {
                log.Info("STEP 4 Error   " +   ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                if (!string.IsNullOrEmpty(file_path) && System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
            }
            return View("Index");
        }


        [HttpGet]
        public virtual ActionResult DownloadSampleExcel()
        {
            string fullPath = Server.MapPath(CommonMethods.BulkUpload_Template);
            return File(fullPath, "application/vnd.ms-excel", "BulkUpload_Template.xlsx");
        }

        [HttpGet]
        public ActionResult GetMastersGridView(int page = 1, int noofrecords = 10, int categoryId = 0)
        {
            _masterService = new MastersBAL();
            int totalRecords;
            var model = new MastersViewModel();
            model.MastersList = _masterService.GetMastersList(page, noofrecords, categoryId, out totalRecords);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;
            return PartialView("_Index", model);
        }


        public JsonResult update(M_MasterModel model)
        {
            _masterService = new MastersBAL();
            var result = _masterService.Update(model, Convert.ToInt32(Session[SystemVariables.UserId]));
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCategory()
        {
            _masterService = new MastersBAL();
            var Get = _masterService.GetCategory();
            return Json(Get, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(M_MasterModel model)
        {
            _masterService = new MastersBAL();
            var result = new M_ResponceResult();
            string errorMessage = string.Empty;
            int AdminUserId = Convert.ToInt32(Session[SystemVariables.UserId]);

            model.MRP = decimal.Round(Convert.ToDecimal(model.MRP), 2);
            result = _masterService.Save(model, AdminUserId);

            if (result.Result > 0)
                errorMessage = "Record saved successfully.";
            else if (result.Result == -1)
                errorMessage = "Record already exists.";
            else
                errorMessage = "Error occured while saving Master Record.";

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUpdateCombosPackages(M_CombosPackagesModel model)
        {
            _masterService = new MastersBAL();
            var result = new M_ResponceResult();
            string errorMessage = string.Empty;
            int AdminUserId = Convert.ToInt32(Session[SystemVariables.UserId]);

            model.MRP = decimal.Round(Convert.ToDecimal(model.MRP), 2);
            result = _masterService.SaveUpdateCombosPackages(model, AdminUserId);

            if (result.Result == 1)
                errorMessage = "Combos Package Record saved successfully.";
            else if (result.Result == 2)
                errorMessage = "Combos Package Record updated successfully.";
            else if (result.Result == -1)
                errorMessage = "Combos Package  Record already exists.";
            else
                errorMessage = "Error occured while saving Combos Package  Record.";

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExcelDownLoad(int Ids = 0, string ServiceName = "")
        {
            try
            {
                _masterService = new MastersBAL();
                FileContentResult robj;
                string fileName = "ManageMaster.xlsx";
                System.Data.DataTable dt = new DataTable();
                string _hdrReportTittle = "Manage Master";
                int totalRecords;

                var Result = _masterService.GetMastersExcel(Ids, out totalRecords);
                dt = ConverToDataTable.ToDataTable(Result);
                dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[8] {
                  new DataColumn("SrNo",typeof(string)),
                  new DataColumn("Category Name",typeof(string)),
                  new DataColumn("Service Name",typeof(string)),
                  new DataColumn("Description",typeof(string)),
                  new DataColumn("MRP",typeof(string)),
                  new DataColumn("Discount",typeof(string)),
                  new DataColumn("Duration",typeof(string)),
                  new DataColumn("Status",typeof(string))
                });

                foreach (var item in Result)
                {
                    dt.Rows.Add(item.SrNo.ToString(),
                        item.CategoryName,
                        item.Name,
                        item.Description,
                        item.MRP,
                        item.Discount,
                        item.Duration,
                        item.IsActive
                        );
                }

                using (XLWorkbook wb = new XLWorkbook())
                {

                    //https://www.99colors.net/dot-net-colors
                    var ws = wb.Worksheets.Add(dt, _hdrReportTittle);

                    int Row = dt.Rows.Count;
                    ws.Row(1).InsertRowsAbove(1);

                    #region 'Report Name'
                    ws.Range("A1").Value = _hdrReportTittle;
                    ws.Range("A1").Style.Font.FontSize = 16;
                    ws.Range("A1").Style.Font.Bold = true;
                    ws.Range("A1").Style.Font.FontColor = XLColor.White;
                    ws.Range("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("A1:E1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("A1:E1").Row(1).Merge();


                    ws.Range("F1").Value = "Total Master : " + totalRecords;
                    ws.Range("F1").Style.Font.FontSize = 16;
                    ws.Range("F1").Style.Font.Bold = true;
                    ws.Range("F1").Style.Font.FontColor = XLColor.White;
                    ws.Range("F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("F1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("F1:H1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("F1:H1").Row(1).Merge();

                    #endregion

                    #region 'Service Name'
                    ws.Row(2).InsertRowsAbove(1);

                    if (!string.IsNullOrEmpty(ServiceName))
                    {
                        ws.Range("A2").Value = "Service Name : " + ServiceName;
                    }
                    else
                    {
                        ws.Range("A2").Value = "Service Name : " + "All";

                    }
                    ws.Range("A2").Style.Font.FontSize = 14;
                    ws.Range("A2").Style.Font.Bold = true;
                    ws.Range("A2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("A2:H2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("A2:H2").Row(1).Merge();

                    #endregion


                    ws.Column(1).Width = 8;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 60;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;

                    ws.Tables.FirstOrDefault().ShowAutoFilter = true;
                    ws.Rows().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Columns(1, dt.Columns.Count).Style.Alignment.WrapText = true;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Json(robj, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Excel DownLoad", ex);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private static BasicAuthenticationAttribute.MyPrincipal Identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}