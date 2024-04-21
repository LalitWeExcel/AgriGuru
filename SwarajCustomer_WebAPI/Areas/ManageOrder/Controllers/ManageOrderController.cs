using ClosedXML.Excel;
using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_BAL.Interface.ManageOrder;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.ManageOrder.Controllers
{
    [RoutePrefix("ManageOrder")]
    [SessionExpireFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ManageOrderController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ApplicationIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdCust"));
        public static string SenderIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdCust"));

        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));


        private IManageOrderBAL _OrderService;
        private INotificationsBAL _notifications;
        private IBookingBAL _objBooking;
        private IUserBAL _objUser;
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            return View();
        }


        [HttpGet]
        public ActionResult GetManageOrderGridView(int page = 1, int noofrecords = 10, string startdate = "", string enddate = "", string status = "", string search = "", int state_id = 0, int district_id = 0)
        {
            _OrderService = new ManageOrderBAL();
            int totalRecords;
            var model = new ManageOrderViewModel();
            startdate = CommonMethods.FormatDate(startdate, "dd-MM-yyyy", "yyyy-MM-dd");
            enddate = CommonMethods.FormatDate(enddate, "dd-MM-yyyy", "yyyy-MM-dd");

            model.ManageOrdertList = _OrderService.GetManageOrderList(page, noofrecords, startdate, enddate, status, search,
                state_id, district_id, out totalRecords);
            model.NoOfRecords = noofrecords;
            model.TotalRecords = totalRecords;
            return PartialView("_Index", model);
        }
        [HttpGet]
        public ActionResult ExcelDownLoad(string startdate = "", string enddate = "", string status = "", string search = "", int state_id = 0, int district_id = 0, string StateName = "", string DistrictName = "")
        {
            try
            {
                _OrderService = new ManageOrderBAL();
                FileContentResult robj;
                string fileName = "ManageOrder.xlsx";
                System.Data.DataTable dt = new DataTable();
                string _hdrReportTittle = "Manage Order";
                int totalRecords;

                startdate = CommonMethods.FormatDate(startdate, "dd-MM-yyyy", "yyyy-MM-dd");
                enddate = CommonMethods.FormatDate(enddate, "dd-MM-yyyy", "yyyy-MM-dd");

                var Result = _OrderService.GetManageOrderList(1, 99999999, startdate, enddate, status, search, state_id, district_id, out totalRecords);

                startdate = CommonMethods.FormatDate(startdate, "yyyy-MM-dd", "dd-MM-yyyy");
                enddate = CommonMethods.FormatDate(enddate, "yyyy-MM-dd", "dd-MM-yyyy");

                dt = ConverToDataTable.ToDataTable(Result);
                dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[37] {
                  new DataColumn("SrNo",typeof(string)),
                  new DataColumn("Order Number",typeof(string)),
                  new DataColumn("Booking Date",typeof(string)),
                  new DataColumn("Booking Type",typeof(string)),
                  new DataColumn("Puja Name",typeof(string)),
                  new DataColumn("Time",typeof(string)),
                  new DataColumn("Is Samagri",typeof(string)),
                  new DataColumn("Puja MRP",typeof(string)),
                  new DataColumn("Puja Discount",typeof(string)),
                  new DataColumn("Puja Discounted Price",typeof(string)),
                  new DataColumn("With Samagri",typeof(string)),
                  new DataColumn("Samagri Discount",typeof(string)),
                  new DataColumn("Purohit Name",typeof(string)),
                  new DataColumn("Purohit Rating",typeof(string)),
                  new DataColumn("Cust. Name",typeof(string)),
                  new DataColumn("Cust. Phone",typeof(string)),
                  new DataColumn("Cust. Address",typeof(string)),
                  new DataColumn("Cust. City",typeof(string)),
                  new DataColumn("Cust. State",typeof(string)),
                  new DataColumn("Cust District",typeof(string)),
                  new DataColumn("Cust. Pincode",typeof(string)),
                  new DataColumn("OTP",typeof(string)),
                  new DataColumn("WhatsApp",typeof(string)),
                  new DataColumn("Skype",typeof(string)),
                  new DataColumn("EPuja Email",typeof(string)),
                  new DataColumn("EPuja PhoneNo",typeof(string)),
                  new DataColumn("Persion Contact Name",typeof(string)),
                  new DataColumn("Persion Contact Number",typeof(string)),
                  new DataColumn("Paid",typeof(string)),
                  new DataColumn("Order Status",typeof(string)),
                  new DataColumn("IsActive",typeof(string)),
                  new DataColumn("OTPConfirm",typeof(string)),
                  new DataColumn("AddToCart",typeof(string)),
                  new DataColumn("#IsTemp",typeof(string)),
                  new DataColumn("Ression",typeof(string)),
                  new DataColumn("Payment Status",typeof(string)),
                  new DataColumn("Payment Mode",typeof(string)),
                });


                foreach (var item in Result)
                {
                    dt.Rows.Add(item.SrNo.ToString(),
                        item.OrderNumber,
                        item.BookingDate,
                        item.BookingType,
                        item.PujaName,
                        item.TimeSlot,
                        item.IsSamagri,
                        item.PujaMRP,
                        item.PujaDiscount,
                        item.PujaDiscountedPrice,
                        item.WithSamagri,
                        item.SamagriDiscount,
                        item.PurohitName,
                        item.PurohitRating,
                        item.CustName,
                        item.CustPhone,
                        item.CustAddress,
                        item.CustCity,
                        item.CustState,
                        item.CustDistrict,
                        item.CustPincode,
                        item.OTP,
                        item.WhatsApp,
                        item.Skype,
                        item.EPujaEmail,
                        item.EPujaPhoneNo,
                        item.PersionofContactName,
                        item.PersionofContactNumber,
                        item.Paid,
                        item.OrderStatus,
                        item.IsActive,
                        item.OTPConfirm,
                        item.AddToCart,
                        item.IsTemp,
                        item.Ression,
                        item.PaymentStatus,
                        item.PaymentMode
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


                    ws.Range("L1").Value = "Total Booking : " + totalRecords;
                    ws.Range("L1").Style.Font.FontSize = 16;
                    ws.Range("L1").Style.Font.Bold = true;
                    ws.Range("L1").Style.Font.FontColor = XLColor.White;
                    ws.Range("L1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("L1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("L1:AK1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
                    ws.Range("L1:AK1").Row(1).Merge();
                    ws.Row(2).InsertRowsAbove(1);
                    #endregion

                    #region 'Date Range'

                    ws.Range("A2").Value = String.Format(" Booking Date  From {0} to {1}  ", startdate, enddate);
                    ws.Range("A2").Style.Font.FontSize = 14;
                    ws.Range("A2").Style.Font.Bold = true;
                    ws.Range("A2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("A2:D2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("A2:D2").Row(1).Merge();

                    #endregion


                    #region 'Date Range'


                    if (!string.IsNullOrEmpty(status))
                    {
                        ws.Range("E2").Value = "Status : " + status;
                    }
                    else
                    {
                        ws.Range("E2").Value = "Status :" + "All";

                    }
                    ws.Range("E2").Style.Font.FontSize = 14;
                    ws.Range("E2").Style.Font.Bold = true;
                    ws.Range("E2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("E2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("E2:I2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("E2:I2").Row(1).Merge();

                    #endregion

                    #region 'search'

                    if (!string.IsNullOrEmpty(search))
                    {
                        ws.Range("J2").Value = "Puja Name.: " + search;
                    }
                    else
                    {
                        ws.Range("J2").Value = "Puja Name.:" + "All";

                    }
                    ws.Range("J2").Style.Font.FontSize = 14;
                    ws.Range("J2").Style.Font.Bold = true;
                    ws.Range("J2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("J2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("J2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("J2:P2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("J2:P2").Row(1).Merge();
                    #endregion

                    #region 'state name'

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
                    ws.Range("Q2:U2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("Q2:U2").Row(1).Merge();
                    #endregion
                    #region 'distt. name'

                    if (!string.IsNullOrEmpty(DistrictName))
                    {
                        ws.Range("V2").Value = "District Name.: " + DistrictName;
                    }
                    else
                    {
                        ws.Range("V2").Value = "District Name.:" + "All";

                    }
                    ws.Range("V2").Style.Font.FontSize = 14;
                    ws.Range("V2").Style.Font.Bold = true;
                    ws.Range("V2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("V2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("V2").Style.Font.FontColor = XLColor.FromArgb(110, 98, 40);
                    ws.Range("V2:AK2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 151);
                    ws.Range("V2:AK2").Row(1).Merge();
                    #endregion

                    ws.Column(1).Width = 8;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 30;
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
                    ws.Column(27).Width = 20;
                    ws.Column(28).Width = 20;
                    ws.Column(29).Width = 20;
                    ws.Column(30).Width = 20;
                    ws.Column(31).Width = 20;
                    ws.Column(32).Width = 20;
                    ws.Column(33).Width = 20;
                    ws.Column(34).Width = 20;
                    ws.Column(35).Width = 20;
                    ws.Column(36).Width = 20;
                    ws.Column(37).Width = 20;
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

        [HttpGet]
        public ActionResult Details(int i, string o)
        {
            ViewBag.LCount = 2;
            ViewBag.Breadcrumb = "Index";
            _OrderService = new ManageOrderBAL();
            var model = _OrderService.Details(i, o);
            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmProhit(int BookingID, string OrderNumber)
        {
            var request = new Responce();
            _OrderService = new ManageOrderBAL();
            _objUser = new UserBAL();
            _objBooking = new BookingBAL();
           _notifications = new NotificationsBAL();

            int admin_id = Convert.ToInt32(Session[SystemVariables.UserId]);
            var responce = _OrderService.ConfirmProhit(BookingID, OrderNumber, admin_id);

            // send PuchNotification
            // send  Email to prohit astrollger
            if (responce.Contains("successfully"))
            {
                var GeBookings = _objBooking.GetBookings(OrderNumber);
                var notification = _notifications.GetPuchNotification(GeBookings.PurohitID, ContentType.PACBAFB.ToString());

                //push notification for  Prohit Astrologer booking Confrim By Admin 
                request.Message = Extensions.BookingConfirmByAdminContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);
                request.booking_id = BookingID;
                request.order_number = OrderNumber;
                request.result = responce;
                PuchNotification_Prohit.Prohit_Astrologer_booking_Confrim_By_Admin_Notification(notification, request);
                _notifications.SaveNotifications(GeBookings.PujaName, responce, notification.user_id, notification.contentsId);


                var CustomerData = _objUser.GetUserProfile(GeBookings.PurohitID);
                if(!string.IsNullOrEmpty(CustomerData.Email))
                   CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, responce, request.Message, 0);

            }

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProhit(string BookingType)
        {
            _OrderService = new ManageOrderBAL();
            var models = _OrderService.GetProhit(BookingType).ConvertAll(x => new kendoUiMultiple { value = x.Id, text = x.Name });
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(M_UpdateProhits model)
        {
             var request = new Responce();
            _OrderService = new ManageOrderBAL();
            _objUser = new UserBAL();
            _objBooking = new BookingBAL();
            _notifications = new NotificationsBAL();

            var result = new M_Responce();
            string errorMessage = string.Empty;

            result = _OrderService.Update(model, Convert.ToInt32(Session[SystemVariables.UserId]));

            if (result.Id > 0 || result.Id == 0)
            {
                errorMessage = result.Result;
                if (result.Result.Contains("successfully"))
                {
                    var GeBookings = _objBooking.GetBookings(model.OrderNumber);

                    var notification_user = _notifications.GetPuchNotification(GeBookings.UserID, ContentType.ABFNPABA.ToString());

                    var notification_NewPurohitID = _notifications.GetPuchNotification(GeBookings.PurohitID, ContentType.ABFNPABA.ToString());

                    var notification_OldProhitID = _notifications.GetPuchNotification(model.OldProhitID, ContentType.ABFNPABA.ToString());


                    //push notification for  Assign booking for new Prohit/Astrologer  Because  old prophet/Astrologer rejects this booking or unable for this booking

                   request.Message = Extensions.BookingConfirmByAdminContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);

                    request.booking_id = GeBookings.BookingID;
                    request.order_number = GeBookings.OrderNumber;
                    request.result = result.Result;


                    // send push notification to customer


                    if (!string.IsNullOrEmpty(notification_user.device_key))
                    {
                        CommonMethods.OrderNotification(notification_user, request, ApplicationIdCust, SenderIdCust);
                        _notifications.SaveNotifications(GeBookings.PujaName, request.Message, notification_user.user_id, notification_user.contentsId);

                        var CustomerData = _objUser.GetUserProfile(GeBookings.UserID);

                        if (!string.IsNullOrEmpty(CustomerData.Email))
                            CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, request.result, request.Message, 0);
                    }

                    // send push notification to new prohit
                    if (!string.IsNullOrEmpty(notification_NewPurohitID.device_key))
                    {
                        PuchNotification_Prohit.Prohit_Astrologer_booking_Confrim_By_Admin_Notification(notification_NewPurohitID, request);
                        _notifications.SaveNotifications(GeBookings.PujaName, request.Message, notification_NewPurohitID.user_id, notification_NewPurohitID.contentsId);

                        var CustomerData = _objUser.GetUserProfile(GeBookings.PurohitID);

                        if (!string.IsNullOrEmpty(CustomerData.Email))
                            CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, request.result, request.Message, 0);
                    }


                    // send push notification to old prohit
                    if (!string.IsNullOrEmpty(notification_OldProhitID.device_key))
                    {
                        PuchNotification_Prohit.Prohit_Astrologer_booking_Confrim_By_Admin_Notification(notification_OldProhitID, request);
                        _notifications.SaveNotifications(GeBookings.PujaName, request.Message, notification_OldProhitID.user_id, notification_OldProhitID.contentsId);

                        var CustomerData = _objUser.GetUserProfile(model.OldProhitID);

                        if (!string.IsNullOrEmpty(CustomerData.Email))
                            CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, request.result, request.Message, 0);
                    }
                }
            }

            else
                errorMessage = "Error occured while saving Manage Order Record.";

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePackage(M_UpdatePackage model)
        {
            var request = new Responce();
            _OrderService = new ManageOrderBAL();
            _objUser = new UserBAL();
            _objBooking = new BookingBAL();
            _notifications = new NotificationsBAL();
            string errorMessage = string.Empty;
            List<string> MuiltipleNewPurohitIDs = model.NewPurohitID.Split(',').ToList();

            var result = new M_Responce();

            result = _OrderService.UpdatePackage(model, Convert.ToInt32(Session[SystemVariables.UserId]));
            errorMessage = result.Result;


            if (result.Result.Contains("successfully"))
            {
                var GeBookings = _objBooking.GetBookings(model.OrderNumber);
                request.Message = Extensions.BookingConfirmByAdminContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);

                request.booking_id = GeBookings.BookingID;
                request.order_number = GeBookings.OrderNumber;
                request.result = result.Result;

                //push notification for Assign multiple Prohit for service package booking Because the old prohit rejects this booking or unable for this booking or no any one select prohit select by admin 
                foreach (var ids in MuiltipleNewPurohitIDs)
                {
                    var notification = _notifications.GetPuchNotification(Convert.ToInt32(ids), ContentType.AMPFSPB.ToString());

                    if (!string.IsNullOrEmpty(notification.device_key))
                    {
                        PuchNotification_Prohit.Prohit_Astrologer_booking_Confrim_By_Admin_Notification(notification, request);
                        _notifications.SaveNotifications(GeBookings.PujaName, request.Message, notification.user_id, notification.contentsId);

                        var CustomerData = _objUser.GetUserProfile(Convert.ToInt32(ids));// GeBookings.PurohitID

                        if (!string.IsNullOrEmpty(CustomerData.Email))
                            CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, request.result, request.Message, 0);
                    }
                }
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
    }
}

