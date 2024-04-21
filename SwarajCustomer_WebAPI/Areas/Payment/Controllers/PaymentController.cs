using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_BAL.Interface.ManagePayment;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_WebAPI.Areas.Payment.Models;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Payment.Controllers
{

    [RoutePrefix("Payment")]
    [BasicAuthentication]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]

    public class PaymentController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ApplicationIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdCust"));
        public static string SenderIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdCust"));

        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));

        private IManagePaymentBAL _managepayment;
        private IBookingBAL _bookingservice;
        private INotificationsBAL _notifications;

        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("CreateOrder")]
        public ActionResult CreateOrder(PaymentInitiateModel _requestData)
        {
            string transactionId = GenerateRandom.GenerateRandomNumber(20);


            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", decimal.Round(_requestData.amount, 2) * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            // Create order model for return on view
            var orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = CommonMethods.KEY_ID,
                amount = _requestData.amount,
                currency = "INR",
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                address = "N/A",
                description = _requestData.pujaname,
                userid = _requestData.userid,
                bookingid = _requestData.bookingid,
                ordernumber = _requestData.ordernumber,
                fullpayment = _requestData.fullpayment,
                remainingamount = _requestData.remainingamount,
            };

            // Return on PaymentPage with Order data
            return View("PaymentPage", orderModel);
        }


        [HttpPost]
        [Route("Complete")]
        public ActionResult Complete()
        {
            _notifications = new NotificationsBAL();
            _bookingservice = new BookingBAL();
            _managepayment = new ManagePaymentBAL();


            var model = new M_SaveManagePayment();
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];


            // get  payment user id, bookingid,order number for save data base
            int user_id = Convert.ToInt32(Request.Params["userid"]);
            int booking_id = Convert.ToInt32(Request.Params["bookingid"]);
            string order_number = Request.Params["ordernumber"];
            string full_payment = Request.Params["fullpayment"];
            decimal remaining_amount = Convert.ToDecimal(Request.Params["remainingamount"]);


            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            //string amount = paymentCaptured.Attributes["amount"];
            string payment_status = paymentCaptured.Attributes["status"];
            string payment_mode = paymentCaptured.Attributes["method"];

            string txnid = "";
            string payment_details = "";

            if (payment_mode == "upi")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["upi_transaction_id"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "netbanking")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["bank_transaction_id"];
                payment_details = paymentCaptured.Attributes["bank"];
            }
            else if (payment_mode == "card")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["auth_code"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "wallet")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["transaction_id"];
                payment_details = paymentCaptured.Attributes["wallet"];
            }

            string error_code = paymentCaptured.Attributes["error_code"];
            string error_description = paymentCaptured.Attributes["error_description"];

            //// Check payment made successfully
            //// save  payment made in trn_razorpay_order
            ///
            if (payment_status == "captured")
            {
                model.rzp_paymentid = paymentId;
                model.rzp_orderid = orderId;
                model.rzp_amount = Convert.ToDecimal(Request.Params["amount"]);
                model.user_id = user_id;
                model.booking_id = booking_id;
                model.order_number = order_number;
                model.payment_status = payment_status;
                model.error_code = error_code;
                model.error_desc = error_description;
                model.txn_id = txnid;
                model.payment_mode = payment_mode;
                model.payment_details = payment_details;
                model.status = "S";
                model.full_payment = full_payment;
                model.remaining_amount = remaining_amount;

                var result = _managepayment.Save(model);
                var data = _bookingservice.GetBookingsStatus(order_number);

                // push notification  and email to customer
                if (data.BookingType == "Independent Ads")
                    SendEmailAndPushNotification(result, data, ContentType.SIA);
                else if (data.BookingType == "Astrologer")
                    SendEmailAndPushNotification(result, data, ContentType.SAO);
                else if (data.BookingType == "Service")
                    SendEmailAndPushNotification(result, data, ContentType.SPSO);
                else
                    SendEmailAndPushNotification(result, data, ContentType.SPO);

                return RedirectToAction("Success", "Payment", new { error = error_description, booking_id = model.booking_id, order_number = model.order_number });
            }
            else
            {

                model.rzp_paymentid = paymentId;
                model.rzp_orderid = orderId;
                model.rzp_amount = Convert.ToDecimal(Request.Params["amount"]);
                model.user_id = user_id;
                model.booking_id = booking_id;
                model.order_number = order_number;
                model.payment_status = payment_status;
                model.error_code = error_code;
                model.error_desc = error_description;
                model.txn_id = txnid;
                model.payment_mode = payment_mode;
                model.payment_details = payment_details;
                model.status = "F";
                model.full_payment = full_payment;
                model.remaining_amount = remaining_amount;
                _managepayment.Save(model);

                return RedirectToAction("Failed", "Payment", new { error = error_description, booking_id = model.booking_id, order_number = model.order_number });
            }
        }

        private void SendEmailAndPushNotification(M_Responce result, BookingOrderModel data, ContentType contentType)
        {
            var sb = new StringBuilder();
            Responce response = new Responce();

            NotificationEnitity notification = _notifications.GetPuchNotification(data.UserID, contentType.ToString());
            NotificationEnitity Prohit_notification = _notifications.GetPuchNotification(data.PurohitID, contentType.ToString());

            response.booking_id = data.BookingID;
            response.order_number = data.OrderNumber;

            // push notification for prohit -astro
            if (!string.IsNullOrEmpty(Prohit_notification.device_key))
            {
                response.Message = Extensions.PujaOrderContentProhitAstro(data.PujaName, data.BookingDate, data.PujaTime, data.OrderNumber);
                CommonMethods.OrderNotification(Prohit_notification, response, ApplicationIdProhit, SenderIdProhit);
            }

            // push notification for customer
            if (!string.IsNullOrEmpty(notification.device_key))
            {
                response.Message = Extensions.PujaOrderContent(data.PujaName, data.BookingDate, data.PujaTime, data.OrderNumber);
                CommonMethods.OrderNotification(notification, response, ApplicationIdCust, SenderIdCust);
            }

            _notifications.SaveNotifications(data.PujaName, response.Message, notification.user_id, notification.contentsId);


            sb.Append(result.Template.Replace("[CustomerName]", data.CustName)
                                    .Replace("[PujaName]", data.PujaName)
                                    .Replace("[OrderNumber]", data.OrderNumber)
                                    .Replace("[BookingDate]", data.BookingDate + " " + data.PujaTime)
                                    .Replace("[Description]", response.Message)
                                    .Replace("[OrderAmount]", data.rzp_amount.ToString())
                                    .Replace("[PaymentType]", data.PaymentMode)
                                    .Replace("[NetBanking]", data.PaymentDetails)
                                    .Replace("[RemaningAmount]", data.RemainingAmount.ToString())
                                    .Replace("[TotalAmount]", data.TotalAmount.ToString())
                                    .Replace("[CustomerAddress]", data.CustAddress)
                                    .Replace("[CustomerPinCode]", data.CustPincode)
                                    .Replace("[CustomerCity]", data.CustCity)
                                    .Replace("[CustomerState]", data.CustState));

            if (!string.IsNullOrEmpty(data.CustEmail))
                CommonMethods.SendHtmlMail(new string[] { data.CustEmail }, new string[] { CommonMethods.EmailCC }, result.Result, sb.ToString(), 0);
        }

        [HttpGet]
        public ActionResult Success(int booking_id,string order_number)
        {
            ViewBag.booking_id = booking_id;
            ViewBag.order_number = order_number;
            ViewBag.status = "Success";     
            return View();
        }
   
        [HttpGet]
        public ActionResult Failed(string error, int booking_id, string order_number)
        {
            ViewBag.error_desc = error;
            ViewBag.booking_id = booking_id;
            ViewBag.order_number = order_number;
            ViewBag.status = "Failed";
            return View();
        }

        [HttpGet]
        [Route("PremiumMembership")]
        public ActionResult PremiumMembership(PremiumMembershipModel _requestData)
        {

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", decimal.Round(_requestData.Amount, 2) * 100);  // Amount will in paise
            options.Add("receipt", GenerateRandom.GenerateRandomNumber(20));
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            Razorpay.Api.Order orderResponse = client.Order.Create(options);

            _requestData.orderId = orderResponse["id"].ToString();
            _requestData.razorpayKey = CommonMethods.KEY_ID;
            _requestData.currency = "INR";
            return View(_requestData);
        }

        [HttpPost]
        [Route("CompletePremiumMembership")]
        public ActionResult CompletePremiumMembership()
        {
            _managepayment = new ManagePaymentBAL();
            var model = new M_SavePremiummemberShip();
            string paymentId = Request.Params["rzp_paymentid"];

            string orderId = Request.Params["rzp_orderid"];

            // get  payment user id, role_name,premium_membership_id for save data base
            int user_id = Convert.ToInt32(Request.Params["userid"]);
            string role_name = Convert.ToString(Request.Params["rolename"]);
            int premium_membership_id = Convert.ToInt32(Request.Params["premiummembershipid"]);


            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string payment_status = paymentCaptured.Attributes["status"];
            string payment_mode = paymentCaptured.Attributes["method"];


            string txnid = "";
            string payment_details = "";

            if (payment_mode == "upi")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["upi_transaction_id"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "netbanking")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["bank_transaction_id"];
                payment_details = paymentCaptured.Attributes["bank"];
            }
            else if (payment_mode == "card")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["auth_code"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "wallet")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["transaction_id"];
                payment_details = paymentCaptured.Attributes["wallet"];
            }

            string error_code = paymentCaptured.Attributes["error_code"];
            string error_description = paymentCaptured.Attributes["error_description"];

            //// Check payment made successfully
            //// save  payment made in trn_razorpay_premium_prohits
            ///
            if (payment_status == "captured")
            {
                model.rzp_paymentid = paymentId;
                model.rzp_orderid = orderId;
                model.rzp_amount = Convert.ToDecimal(Request.Params["amount"]);
                model.user_id = user_id;
                model.premium_membership_id = premium_membership_id;
                model.role_name = role_name;
                model.payment_status = payment_status;
                model.error_code = error_code;
                model.error_desc = error_description;
                model.txn_id = txnid;
                model.payment_mode = payment_mode;
                model.payment_details = payment_details;
                model.status = "S";
             
                _managepayment.SavePremiumMembership(model);

                return RedirectToAction("Success", "Payment", new { booking_id = 0, order_number = "" });
            }
            else
            {

                model.rzp_paymentid = paymentId;
                model.rzp_orderid = orderId;
                model.rzp_amount = Convert.ToDecimal(Request.Params["amount"]);
                model.user_id = user_id;
                model.premium_membership_id = premium_membership_id;
                model.role_name = role_name;
                model.payment_status = payment_status;
                model.error_code = error_code;
                model.error_desc = error_description;
                model.txn_id = txnid;
                model.payment_mode = payment_mode;
                model.payment_details = payment_details;
                model.status = "F";
                _managepayment.SavePremiumMembership(model);

     
                return RedirectToAction("Failed", "Payment", new { error = error_description, booking_id = 0, order_number = "" });
            }
        }

        [HttpGet]
        [Route("CheckoutMyCartPayment")]
        public ActionResult CheckoutMyCartPayment(CheckoutMyCartPaymentModel _requestData)
        {
            decimal amount = _requestData.amount.Split(',').Sum(x => Convert.ToDecimal(x));
            string transactionId = GenerateRandom.GenerateRandomNumber(20);

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", decimal.Round(amount, 2) * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            var orderModel = new CheckoutMyCartOrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = CommonMethods.KEY_ID,
                currency = "INR",
                userid = _requestData.userid,
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                description = _requestData.pujaname,
                bookingid = _requestData.bookingid,
                ordernumber = _requestData.ordernumber,
                fullpayment = _requestData.fullpayment,
                amount = amount
            };

            return View("CheckoutMyCartPayment", orderModel);
        }

        [HttpPost]
        [Route("CheckoutMyCartPayment")]
        public ActionResult CheckoutMyCartPayment()
        {
            _notifications = new NotificationsBAL();
            _bookingservice = new BookingBAL();
            _managepayment = new ManagePaymentBAL();

            var model = new M_SaveManagePayment();

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];
            string orderId = Request.Params["rzp_orderid"];
            int user_id = Convert.ToInt32(Request.Params["userid"]);
            string userName = Convert.ToString(Request.Params["name"]);
            string userEmail = Convert.ToString(Request.Params["email"]);
            string bookingid = Convert.ToString(Request.Params["bookingid"]);


            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(CommonMethods.KEY_ID, CommonMethods.KEY_SECRET);
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string payment_status = paymentCaptured.Attributes["status"];
            string payment_mode = paymentCaptured.Attributes["method"];

            string txnid = "";
            string payment_details = "";

            if (payment_mode == "upi")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["upi_transaction_id"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "netbanking")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["bank_transaction_id"];
                payment_details = paymentCaptured.Attributes["bank"];
            }
            else if (payment_mode == "card")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["auth_code"];
                payment_details = payment_mode;
            }
            else if (payment_mode == "wallet")
            {
                txnid = paymentCaptured.Attributes["acquirer_data"]["transaction_id"];
                payment_details = paymentCaptured.Attributes["wallet"];
            }

            string error_code = paymentCaptured.Attributes["error_code"];
            string error_description = paymentCaptured.Attributes["error_description"];

            // Check payment made successfully
            // save  payment made in trn_razorpay_order

            string full_payment = Request.Params["fullpayment"];
            List<string> order_number = Request.Params["ordernumber"].Split(',').ToList();

            decimal totalAmount = 0;
            decimal RemainingAmount = 0;

            if (payment_status == "captured")
            {
                foreach (var order in order_number)
                {
                    var item = _bookingservice.GetBookings(order);
                    totalAmount = Convert.ToDecimal(item.PujaDiscountedPrice);

                    if (item.IsSamagri == "1")
                        totalAmount = decimal.Add(totalAmount, Convert.ToDecimal(item.SamagriDiscountedPrice));

                    //Is Full Payment
                    if (full_payment == "N")
                    {
                        var calucatedAmount = (totalAmount * 20 / 100);
                        RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                        totalAmount = calucatedAmount;
                    }
                    model.rzp_paymentid = paymentId;
                    model.rzp_orderid = orderId;
                    model.rzp_amount = totalAmount;
                    model.user_id = user_id;
                    model.booking_id = item.BookingID;
                    model.order_number = item.OrderNumber;
                    model.payment_status = payment_status;
                    model.error_code = error_code;
                    model.error_desc = error_description;
                    model.txn_id = txnid;
                    model.payment_mode = payment_mode;
                    model.payment_details = payment_details;
                    model.status = "S";
                    model.full_payment = full_payment;
                    model.remaining_amount = RemainingAmount;



                    var result = _managepayment.CheckoutMyCartPaymentSave(model, userName, userEmail);
                    var data = _bookingservice.GetBookingsStatus(item.OrderNumber);

                    // push notification  and email to customer
                    if (data.BookingType == "Independent Ads")
                        SendEmailAndPushNotification(result, data, ContentType.SIA);
                    else if (data.BookingType == "Astrologer")
                        SendEmailAndPushNotification(result, data, ContentType.SAO);
                    else if (data.BookingType == "Service")
                        SendEmailAndPushNotification(result, data, ContentType.SPSO);
                    else
                        SendEmailAndPushNotification(result, data, ContentType.SPO);
                }
                return RedirectToAction("Success", "Payment", new {  booking_id = model.booking_id, order_number = model.order_number });
            }
            else
            {
                foreach (var order in order_number)
                {
                    var item = _bookingservice.GetBookings(order);
                    totalAmount = Convert.ToDecimal(item.PujaDiscountedPrice);

                    if (item.IsSamagri == "1")
                        totalAmount = decimal.Add(totalAmount, Convert.ToDecimal(item.SamagriDiscountedPrice));

                    //Is Full Payment
                    if (full_payment == "N")
                    {
                        var calucatedAmount = (totalAmount * 20 / 100);
                        RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                        totalAmount = calucatedAmount;
                    }
                    model.rzp_paymentid = paymentId;
                    model.rzp_orderid = orderId;
                    model.rzp_amount = totalAmount;
                    model.user_id = user_id;
                    model.booking_id = item.BookingID;
                    model.order_number = item.OrderNumber;
                    model.payment_status = payment_status;
                    model.error_code = error_code;
                    model.error_desc = error_description;
                    model.txn_id = txnid;
                    model.payment_mode = payment_mode;
                    model.payment_details = payment_details;
                    model.status = "F";
                    model.full_payment = full_payment;
                    model.remaining_amount = RemainingAmount;
                    _managepayment.CheckoutMyCartPaymentSave(model, userName, userEmail);
                }
                return RedirectToAction("Failed", "Payment", new { error = error_description, booking_id = model.booking_id, order_number = model.order_number });
             
            }
        }

        private static BasicAuthenticationAttribute.MyPrincipal Identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}