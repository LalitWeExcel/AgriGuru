using ClosedXML.Excel;
using SwarajCustomer_BAL.Interface.ManageUser;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManageUser.Controllers
{
    [RoutePrefix("ManageUser")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ManageUserController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IManageUserBAL _manageUserService;

        [HttpGet]
        public ActionResult Index()
        {
            _manageUserService = new ManageUserBAL();

            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            ViewBag.Roles = _manageUserService.GetRoleMaster();

            return View();
        }

        [HttpGet]
        public ActionResult GetUsersGridView(int page = 1, int noofrecords = 10, string search = "", int role_id = 0)
        {
            _manageUserService = new ManageUserBAL();
            int totalRecords;
            var model = new ManageUserViewModel();
            model.ManageUserList = _manageUserService.GetManageUserList(page, noofrecords, search, role_id, out totalRecords);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;
            return PartialView("_Index", model);
        }

        [HttpPost]
        public JsonResult ActivateDeactivate(List<M_ActiveDisActive> user_ids, string IsActive)
        {
            _manageUserService = new ManageUserBAL();
            int admin_id = Convert.ToInt32(Session[SystemVariables.UserId]);

            string[] the_array = user_ids.Select(i => i.Id.ToString()).ToArray();
            string Ids = String.Join(", ", the_array).Trim();

             var result = _manageUserService.ActivateDeactivate(Ids, IsActive, admin_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int user_id)
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Edit";
            _manageUserService = new ManageUserBAL();
            var model = _manageUserService.GetManageUserDetails(user_id);

            Session[SystemVariables.IsActive] = model.is_active;
            Session[SystemVariables.ImageName] = model.ImageName;
            Session[SystemVariables.UserDistrictId] = model.mst_district_id;
            Session[SystemVariables.UserStateId] = model.mst_state_id;
            Session[SystemVariables.UserRolesList] = model.ManageUserRoles;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(M_ManageUserDetails model)
        {
            if (ModelState.IsValid)
            {
                _manageUserService = new ManageUserBAL();
                int admin_id = Convert.ToInt32(Session[SystemVariables.UserId]);

                _manageUserService.Update(model, admin_id);
                return RedirectToAction("index", "ManageUser");
            }
            else
            {
                model.is_active = Convert.ToString(Session[SystemVariables.IsActive]);
                model.ImageName = Convert.ToString(Session[SystemVariables.ImageName]);
                model.mst_district_id = Convert.ToInt32(Session[SystemVariables.UserDistrictId]);
                model.mst_state_id = Convert.ToInt32(Session[SystemVariables.UserStateId]);
                model.ManageUserRoles = (List<M_ManageUserRoles>)Session[SystemVariables.UserRolesList];

                return View("Edit", model);
            }
        }


        [HttpGet]
        public ActionResult Details(int user_id)
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Details";
            _manageUserService = new ManageUserBAL();
            var model = _manageUserService.GetManageUserDetails(user_id);
            return View(model);
        }

        [HttpGet]
        public ActionResult ExcelDownLoad(string search = "", int role_id = 0, string role_name ="")
        {
            try
            {
                _manageUserService = new ManageUserBAL();
                FileContentResult robj;
                string fileName = "ManageUser.xlsx";
                System.Data.DataTable dt = new DataTable();
                string _hdrReportTittle = "Manage User";
                int totalRecords;

                var Result = _manageUserService.GetManageUserExcel(search, role_id, out totalRecords);
                dt = ConverToDataTable.ToDataTable(Result);
                dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[26] {
                  new DataColumn("SrNo",typeof(string)),
                  new DataColumn("User Name",typeof(string)),
                  new DataColumn("Mobile Number",typeof(string)),
                  new DataColumn("Email",typeof(string)),
                  new DataColumn("User Type",typeof(string)),
                  new DataColumn("Description",typeof(string)),
                  new DataColumn("Referal Code",typeof(string)),
                  new DataColumn("Status",typeof(string)),
                  new DataColumn("First Name",typeof(string)),
                  new DataColumn("Last Name",typeof(string)),
                  new DataColumn("WhatsApp No",typeof(string)),
                  new DataColumn("Address",typeof(string)),
                  new DataColumn("City",typeof(string)),
                  new DataColumn("District",typeof(string)),
                  new DataColumn("State",typeof(string)),
                  new DataColumn("Expert  in Pooja",typeof(string)),
                  new DataColumn("Ready For Travel",typeof(string)),
                  new DataColumn("Prohit Exp.",typeof(string)),
                  new DataColumn("Marital Status",typeof(string)),
                  new DataColumn("Date of Birth",typeof(string)),
                  new DataColumn("Health Status",typeof(string)),
                  new DataColumn("Rating",typeof(string)),
                  new DataColumn("Landmark",typeof(string)),
                  new DataColumn("Estrologer Exp.",typeof(string)),
                  new DataColumn("Time Of Birth.",typeof(string)),
                  new DataColumn("Temple Name",typeof(string)) });

                foreach (var item in Result)
                {
                    dt.Rows.Add(item.SrNo.ToString(),
                        item.username,
                        item.mob_number,
                        item.email,
                        item.user_type,
                        item.description,
                        item.referal_code,
                        item.is_active,
                        item.first_name,
                        item.last_name,
                        item.whatsapp_no,
                        item.address,
                        item.city,
                        item.district,
                        item.state,
                        item.export_in_pooja,
                        item.ready_for_travel,
                        item.year_of_exp.ToString(),
                        item.marital_status,
                        item.DOB,
                        item.health_status.ToString(),
                        item.rating.ToString(),
                        item.landmark,
                        item.astrologer_experience.ToString(),
                        item.TOB,
                        item.temple_name
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
                    ws.Range("A1:K1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("A1:K1").Row(1).Merge();


                    ws.Range("L1").Value = "Total User : " + totalRecords;
                    ws.Range("L1").Style.Font.FontSize = 16;
                    ws.Range("L1").Style.Font.Bold = true;
                    ws.Range("L1").Style.Font.FontColor = XLColor.White;
                    ws.Range("L1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("L1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("L1:Z1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("L1:Z1").Row(1).Merge();

                    #endregion

                    #region 'Date Range'
                    ws.Row(2).InsertRowsAbove(1);

                    if (!string.IsNullOrEmpty(role_name))
                    {
                        ws.Range("A2").Value = "User Role : " + role_name;
                    }
                    else
                    {
                        ws.Range("A2").Value = "User Role :" + "All";

                    }
                    ws.Range("A2").Style.Font.FontSize = 14;
                    ws.Range("A2").Style.Font.Bold = true;
                    ws.Range("A2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("A2:K2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("A2:K2").Row(1).Merge();

                    #endregion

                    #region 'search'

                    if (!string.IsNullOrEmpty(search))
                    {
                        ws.Range("L2").Value = "Name/Mobile No.: " + search;
                    }
                    else
                    {
                        ws.Range("L2").Value = "Name/Mobile No.:" + "All";

                    }
                    ws.Range("L2").Style.Font.FontSize = 14;
                    ws.Range("L2").Style.Font.Bold = true;
                    ws.Range("L2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("L2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("L2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("L2:Z2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("L2:Z2").Row(1).Merge();
                    #endregion

                    ws.Column(1).Width = 8;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 20;
                    ws.Column(10).Width = 20;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 20;
                    ws.Column(13).Width = 20;
                    ws.Column(14).Width = 20;
                    ws.Column(15).Width = 20;
                    ws.Column(16).Width = 20;
                    ws.Column(17).Width = 20;
                    ws.Column(18).Width = 20;
                    ws.Column(19).Width = 20;
                    ws.Column(20).Width = 20;
                    ws.Column(21).Width = 20;
                    ws.Column(22).Width = 20;
                    ws.Column(23).Width = 20;
                    ws.Column(24).Width = 20;
                    ws.Column(25).Width = 20;
                    ws.Column(26).Width = 20;
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
            //foreach (ModelState modelState in ViewData.ModelState.Values)
            //{
            //    foreach (ModelError error in modelState.Errors)
            //    {
            //            ModelState.AddModelError("name", error.ErrorMessage);
            //    }
            //}

            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }

}