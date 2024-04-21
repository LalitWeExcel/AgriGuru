using ClosedXML.Excel;
using SwarajCustomer_BAL.Interface.ManagePayment;
using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManagePayment.Controllers
{
    [RoutePrefix("ManagePayment")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ManagePaymentController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IManagePaymentBAL _paymentService;
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            return View();
        }

       



        [HttpGet]
        public ActionResult GetManagePaymentGridView(int page = 1, int noofrecords = 10, string startdate = "", string enddate = "", string paymentstatus = "", string paymentmode = "", string search = "",int state_id=0,int district_id=0)
        {
            _paymentService = new ManagePaymentBAL();
            int totalRecords;
            int totelSuccessPayment;
            int totelFailedPayment;
            decimal totelRevenue;

            var model = new ManagePaymentViewModel();
            startdate = CommonMethods.FormatDate(startdate, "dd-MM-yyyy", "yyyy-MM-dd");
            enddate = CommonMethods.FormatDate(enddate, "dd-MM-yyyy", "yyyy-MM-dd");

            model.ManagePaymentList = _paymentService.GetManagePaymentList(page, noofrecords, startdate, enddate, paymentstatus, paymentmode, search, out totalRecords,out totelSuccessPayment,out totelFailedPayment,out totelRevenue, state_id, district_id);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;

            model.TotelSuccessPayment = totelSuccessPayment;
            model.TotelFailedPayment = totelFailedPayment;
            model.TotelRevenue = totelRevenue;


            return PartialView("_Index", model);
        }

        [HttpGet]
        public ActionResult ExcelDownLoad(string startdate = "", string enddate = "", string paymentstatus = "", string paymentmode = "", string search = "",int state_id=0,int district_id=0,string StateName="",string DistrictName="")
        {
            try
            {
                _paymentService = new ManagePaymentBAL();
                FileContentResult robj;
                string fileName = "ManagePayment.xlsx";
                System.Data.DataTable dt = new DataTable();
                string _hdrReportTittle = "Manage Payment";
                int totalRecords;


                int totelSuccessPayment;
                int totelFailedPayment;
                decimal totelRevenue;

                startdate = CommonMethods.FormatDate(startdate, "dd-MM-yyyy", "yyyy-MM-dd");
                enddate = CommonMethods.FormatDate(enddate, "dd-MM-yyyy", "yyyy-MM-dd");

                var Result = _paymentService.GetManagePaymentList(1, 99999999, startdate, enddate, paymentstatus, paymentmode, search, out totalRecords,out totelSuccessPayment, out totelFailedPayment, out totelRevenue, state_id, district_id);

                startdate = CommonMethods.FormatDate(startdate, "yyyy-MM-dd", "dd-MM-yyyy");
                enddate = CommonMethods.FormatDate(enddate, "yyyy-MM-dd", "dd-MM-yyyy");

                dt = ConverToDataTable.ToDataTable(Result);
                dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[30] {
                new DataColumn("SrNo",typeof(string)),
                new DataColumn("Rzp. Paymentid",typeof(string)),
                new DataColumn("Rzp. Orderid",typeof(string)),
                new DataColumn("Booking Amount",typeof(string)),
                new DataColumn("Remaining Amount",typeof(string)),
                new DataColumn("Total Amount",typeof(string)),
                new DataColumn("Transaction Number",typeof(string)),
                new DataColumn("Payment Status",typeof(string)),
                new DataColumn("Payment Mode",typeof(string)),
                new DataColumn("Bank Name",typeof(string)),
                new DataColumn("Status",typeof(string)),
                new DataColumn("Error Code",typeof(string)),
                new DataColumn("Error Desc.",typeof(string)),
                new DataColumn("Order Number",typeof(string)),
                new DataColumn("Booking Date",typeof(string)),
                new DataColumn("RazorPay Date",typeof(string)),
                new DataColumn("Booking Type",typeof(string)),
                new DataColumn("Puja Name",typeof(string)),
                new DataColumn("Time",typeof(string)),
                new DataColumn("Purohit Name",typeof(string)),
                new DataColumn("Cust. Name",typeof(string)),
                new DataColumn("Cust. Phone",typeof(string)),
                new DataColumn("Cust. City",typeof(string)),
                new DataColumn("Cust. State",typeof(string)),
                new DataColumn("Cust. District",typeof(string)),
                new DataColumn("Paid",typeof(string)),
                new DataColumn("Order Status",typeof(string)),
                new DataColumn("IsActive",typeof(string)),
                new DataColumn("OTPConfirm",typeof(string)),
                new DataColumn("AddToCart",typeof(string)),
                });


                foreach (var item in Result)
                {
                    dt.Rows.Add(item.SrNo.ToString(),
                        item.rzp_paymentid,
                        item.rzp_orderid,
                        item.rzp_amount,
                        item.RemainingAmount,
                        item.TotalAmount,
                        item.txn_id,
                        item.payment_status,
                        item.payment_mode,
                        item.payment_details,
                        item.status,
                        item.error_code,
                        item.error_desc,
                        item.OrderNumber,
                        item.BookingDate,
                        item.RazorPayDate,
                        item.BookingType,
                        item.PujaName,
                        item.TimeSlot,
                        item.PurohitName,
                        item.CustName,
                        item.CustPhone,
                        item.CustCity,
                        item.CustState,
                        item.CustDistrict,
                        item.Paid,
                        item.OrderStatus,
                        item.IsActive,
                        item.OTPConfirm,
                        item.AddToCart
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
                    ws.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("A1:D1").Row(1).Merge();

                    ws.Range("E1").Value = "Total Success Payment : " + totelSuccessPayment;
                    ws.Range("E1").Style.Font.FontSize = 16;
                    ws.Range("E1").Style.Font.Bold = true;
                    ws.Range("E1").Style.Font.FontColor = XLColor.White;
                    ws.Range("E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("E1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("E1:H1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("E1:H1").Row(1).Merge();


                    ws.Range("I1").Value = "Total Failed Payment : " + totelFailedPayment;
                    ws.Range("I1").Style.Font.FontSize = 16;
                    ws.Range("I1").Style.Font.Bold = true;
                    ws.Range("I1").Style.Font.FontColor = XLColor.White;
                    ws.Range("I1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("I1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("I1:K1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("I1:K1").Row(1).Merge();


                    ws.Range("L1").Value = "Total Revenue : " + totelRevenue;
                    ws.Range("L1").Style.Font.FontSize = 16;
                    ws.Range("L1").Style.Font.Bold = true;
                    ws.Range("L1").Style.Font.FontColor = XLColor.White;
                    ws.Range("L1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("L1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("L1:O1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("L1:O1").Row(1).Merge();


                    ws.Range("P1").Value = "Total Records : " + totalRecords;
                    ws.Range("P1").Style.Font.FontSize = 16;
                    ws.Range("P1").Style.Font.Bold = true;
                    ws.Range("P1").Style.Font.FontColor = XLColor.White;
                    ws.Range("P1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("P1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("P1:AD1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("P1:AD1").Row(1).Merge();
                    ws.Row(2).InsertRowsAbove(1);
                    #endregion

                    #region 'Date Range'

                    ws.Range("A2").Value = String.Format(" Booking Date  From {0} to {1}  ", startdate, enddate);
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
                        ws.Range("L2").Value = "Search.: " + search;
                    }
                    else
                    {
                        ws.Range("L2").Value = "Search.:" + "All";

                    }
                    ws.Range("L2").Style.Font.FontSize = 14;
                    ws.Range("L2").Style.Font.Bold = true;
                    ws.Range("L2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("L2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("L2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("L2:P2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("L2:P2").Row(1).Merge();
                    #endregion

                    #region 'State Name'

                    if (!string.IsNullOrEmpty(StateName))
                    {
                        ws.Range("Q2").Value = "State Name.: " + StateName;
                    }
                    else
                    {
                        ws.Range("Q2").Value = "State Name.:" + "All";

                    }
                    ws.Range("Q2").Style.Font.FontSize = 14;
                    ws.Range("Q2").Style.Font.Bold = true;
                    ws.Range("Q2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("Q2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("Q2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("Q2:Y2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("Q2:Y2").Row(1).Merge();
                    #endregion

                    #region 'District  Name'

                    if (!string.IsNullOrEmpty(DistrictName))
                    {
                        ws.Range("Z2").Value = "District Name.: " + DistrictName;
                    }
                    else
                    {
                        ws.Range("Z2").Value = "District Name.:" + "All";

                    }
                    ws.Range("Z2").Style.Font.FontSize = 14;
                    ws.Range("Z2").Style.Font.Bold = true;
                    ws.Range("Z2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("Z2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("Z2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("Z2:AD2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("Z2:AD2").Row(1).Merge();
                    #endregion

                    ws.Column(1).Width = 8;
                  ws.Column(2).Width = 30;
                  ws.Column(3).Width = 30;
                  ws.Column(4).Width = 20;
                  ws.Column(5).Width = 20;
                  ws.Column(6).Width = 30;
                  ws.Column(7).Width = 30;
                  ws.Column(8).Width = 30;
                  ws.Column(9).Width = 20;
                  ws.Column(10).Width = 20;
                  ws.Column(11).Width = 8;
                  ws.Column(12).Width = 30;
                  ws.Column(13).Width = 30;
                  ws.Column(14).Width = 30;
                  ws.Column(15).Width = 15;
                  ws.Column(16).Width = 30;
                  ws.Column(17).Width = 15;
                  ws.Column(18).Width = 30;
                  ws.Column(19).Width = 30;
                  ws.Column(20).Width = 15;
                  ws.Column(21).Width = 30;
                  ws.Column(22).Width = 30;
                  ws.Column(23).Width = 30;
                  ws.Column(24).Width = 30;
                  ws.Column(25).Width = 30;
                  ws.Column(26).Width = 15;
                  ws.Column(27).Width = 15;
                  ws.Column(28).Width = 15;
                  ws.Column(29).Width = 15;
                  ws.Column(30).Width = 15;

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
    }
}