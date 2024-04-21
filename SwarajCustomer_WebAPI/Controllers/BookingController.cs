using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("Booking")]
    public class BookingController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ApplicationIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdCust"));
        public static string SenderIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdCust"));

        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));

        private IUserBAL _objUser;
        private INotificationsBAL _notifications;
        private IBookingBAL objBooking;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("GetPujaBookedSlots")]
        public UserModal.DataModal GetPujaBookedSlots(PujaEntity objPuja)
        {
            try
            {
                objBooking = new BookingBAL();

                DateTime PujaDateTime = DateTime.Now;

                List<TimeSlotMaster> bookedSlots = new List<TimeSlotMaster>();
                bookedSlots = objBooking.GetPujaBookedSlots(objPuja.PujaID, objPuja.PujaDate);

                result = new UserModal.DataModal()
                {
                    Data = bookedSlots,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetPujaBookedSlots");
            }
        }

        [HttpPost]
        [Route("GetAstroBookedSlots")]
        public UserModal.DataModal GetAstroBookedSlots(AstroEntity objAstro)
        {
            try
            {
                objBooking = new BookingBAL();

                List<TimeSlotMaster> bookedSlots = new List<TimeSlotMaster>();
                bookedSlots = objBooking.GetAstroBookedSlots(objAstro.AstroID, objAstro.AstroDate);

                result = new UserModal.DataModal()
                {
                    Data = bookedSlots,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetAstroBookedSlots");
            }
        }

        [HttpPost]
        [Route("SavePujaOrder")]
        public UserModal.DataModal SavePujaOrder(PujaOrderDetails objPuja)
        {
            try
            {
                var paymentUrl = string.Empty;

                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var CustomerData = _objUser.GetUserProfile(user_Id);
                var response = objBooking.SavePujaOrder(objPuja);

                // push  notification Add by lalit sharma 27-07-2020
                // save notification for listing
                // send email

                StringBuilder sb = new StringBuilder();
                string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.SaveOrderHtmlPath));

                if (response.result.Contains("successfully"))
                {
                    foreach (var item in objPuja.PujasList)
                    {
                        var PujaName = objBooking.GetProductName(item.PujaID);
                        var TimeSlots = _objUser.GetTimeSlots().Where(x => x.ID == item.PujaTime).FirstOrDefault().TimeSlot;

                        if (!string.IsNullOrEmpty(item.EPujaEmail))
                            CommonMethods.SendHtmlMail(new string[] { CustomerData.Email, item.EPujaEmail }, new string[] { CommonMethods.EmailCC }, response.result, Extensions.EPujaOrderContent(PujaName, item.PujaDate, TimeSlots, null), 0);


                        // redirect tp payment gateway
                        if (objPuja.IsAddToCart == "N")
                        {
                            decimal totalAmount = decimal.Round(Convert.ToDecimal(item.PujaDiscountedPrice),2);
                            if (item.IsSamagri == true)
                                totalAmount = decimal.Add(totalAmount, Convert.ToDecimal(item.SamagriDiscountedPrice));

                            //Is Full Payment
                            decimal RemainingAmount = 0;
                            if (objPuja.FullPayment == "N")
                            {
                                var calucatedAmount = (totalAmount * 20 / 100);
                                RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                                totalAmount = calucatedAmount;
                            }

                            paymentUrl = RazorpayOrder(CustomerData, response, PujaName, totalAmount, objPuja.FullPayment, RemainingAmount);

                            var data = new CheckOutResponce();
                            data.BookingID = response.booking_id;
                            data.OrderNumber = response.order_number;

                            result = new UserModal.DataModal()
                            {
                            
                                Data = data,
                                Response = paymentUrl,
                                Message = CommonActions.Successful.ToString(),
                                Status = true,
                                IsUserActive = true,
                            };
                        }
                    }
                }
                else if (response.result.Contains("cart"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (response.result.Contains("already"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Failed.ToString(),
                        Message =  response.result,
                        Status = false,
                        IsUserActive = true
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/SavePujaOrder");
            }
        }


        [HttpPost]
        [Route("SaveAstroOrder")]
        public UserModal.DataModal SaveAstroOrder(AstroOrderDetails objAstro)
        {
            try
            {
                var paymentUrl = string.Empty;
                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var CustomerData = _objUser.GetUserProfile(user_Id);
                var response = objBooking.SaveAstroOrder(objAstro);


                // push  notification Add by lalit sharma 22-08-2020
                // save notification for listing
                // send email 

                StringBuilder sb = new StringBuilder();
                string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.SaveOrderHtmlPath));

                if (response.result.Contains("successfully"))
                {
                    foreach (var item in objAstro.OrderList)
                    {
                        var serviceName = objBooking.GetservicesName(item.ServiceID);

                        // redirect tp payment gateway
                        if (objAstro.IsAddToCart == "N")
                        {
                            decimal totalAmount = decimal.Round(Convert.ToDecimal(item.AstroDiscountedPrice), 2);
                            //Is Full Payment

                            decimal RemainingAmount = 0;
                            if (objAstro.FullPayment == "N")
                            {
                               var calucatedAmount = (totalAmount * 20 / 100);
                                RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                                totalAmount = calucatedAmount;
                            }

                            paymentUrl = RazorpayOrder(CustomerData, response, serviceName, totalAmount, objAstro.FullPayment, RemainingAmount);
                            var data = new CheckOutResponce();
                            data.BookingID = response.booking_id;
                            data.OrderNumber = response.order_number;

                            result = new UserModal.DataModal()
                            {
                                Data = data,
                                Response = paymentUrl,
                                Message = CommonActions.Successful.ToString(),
                                Status = true,
                                IsUserActive = true,
                            };
                        }
                    }

                }
                else if (response.result.Contains("cart"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (response.result.Contains("already"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Failed.ToString(),
                        Message = response.result,
                        Status = false,
                        IsUserActive = true
                };
                }
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetAstroBookedSlots");
            }
        }

        [HttpPost]
        [Route("SavePackageOrder")]
        public UserModal.DataModal SavePackageOrder(PackageOrderDetails _object)
        {

            try
            {
                var paymentUrl = string.Empty;
                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var CustomerData = _objUser.GetUserProfile(user_Id);
                var response = objBooking.SavePackageOrder(_object);

                // push  notification Add by lalit sharma 27-07-2020
                // save notification for listing
                // send email

                StringBuilder sb = new StringBuilder();
                string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.SaveOrderHtmlPath));

                if (response.result.Contains("successfully"))
                {
                    foreach (var item in _object.PackageList)
                    {
                        var PujaName = objBooking.GetProductName(item.MainProductId);

                        // redirect tp payment gateway
                        if (_object.IsAddToCart == "N")
                        {
                            decimal totalAmount = decimal.Round(Convert.ToDecimal(item.PackageDiscountedPrice), 2);
                            //Is Full Payment
                            decimal RemainingAmount = 0;
                            if (_object.FullPayment == "N")
                            {
                                var calucatedAmount = (totalAmount * 20 / 100);
                                RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                                totalAmount = calucatedAmount;
                            }
                            paymentUrl = RazorpayOrder(CustomerData, response, PujaName, totalAmount, _object.FullPayment, RemainingAmount);

                            var data = new CheckOutResponce();
                            data.BookingID = response.booking_id;
                            data.OrderNumber = response.order_number;

                            result = new UserModal.DataModal()
                            {
                                Data = data, 
                                Response = paymentUrl,
                                Message = CommonActions.Successful.ToString(),
                                Status = true,
                                IsUserActive = true,
                            };
                        }
                    }
                }

                else if (response.result.Contains("cart"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (response.result.Contains("already"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Failed.ToString(),
                        Message = response.result,
                        Status = false,
                        IsUserActive = true
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/SavePackageOrder");
            }
        }

        [HttpPost]
        [Route("SaveIndependentAds")]
        public UserModal.DataModal SaveIndependentAds(IndependentAdsDetails _object)
        {
            try
            {
                var paymentUrl = string.Empty;
                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var CustomerData = _objUser.GetUserProfile(user_Id);
                var response = objBooking.SaveIndependentAds(_object);

                // push  notification Add by lalit sharma 27-07-2020
                // save notification for listing
                // send email

                StringBuilder sb = new StringBuilder();
                string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.SaveOrderHtmlPath));

                if (response.result.Contains("successfully"))
                {
                    foreach (var item in _object.IndependentAdsList)
                    {
                        var PujaName = objBooking.GetIndependentAdsName(item.TrnAdsId);
                        // redirect tp payment gateway
                        if (_object.IsAddToCart == "N")
                        {
                            decimal totalAmount = decimal.Round(Convert.ToDecimal(item.PujaDiscountedPrice), 2);
                            //Is Full Payment
                            decimal RemainingAmount = 0;
                            if (_object.FullPayment == "N")
                            {
                                var calucatedAmount = (totalAmount * 20 / 100);
                                RemainingAmount = decimal.Subtract(totalAmount, calucatedAmount);
                                totalAmount = calucatedAmount;
                            }
                            paymentUrl = RazorpayOrder(CustomerData, response, PujaName, totalAmount, _object.FullPayment, RemainingAmount);

                            var data = new CheckOutResponce();
                            data.BookingID = response.booking_id;
                            data.OrderNumber = response.order_number;

                            result = new UserModal.DataModal()
                            {
                                Data = data,
                                Response = paymentUrl,
                                Message = CommonActions.Successful.ToString(),
                                Status = true,
                                IsUserActive = true,
                            };
                        }
                    }
                }

                else if (response.result.Contains("cart"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (response.result.Contains("already"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = response.result,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Failed.ToString(),
                        Message = response.result,
                        Status = false,
                        IsUserActive = true
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/SaveIndependentAds");
            }
        }

        [HttpPost]
        [Route("MyBookings")]
        public UserModal.DataModal MyBookings(MyBookingsReq objBookings)
        {
            try
            {
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;


                objBooking = new BookingBAL();
                result = new UserModal.DataModal()
                {
                    Data = objBooking.GetMyBookings(objBookings),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/MyBookings");
            }
        }

        [HttpPost]
        [Route("GetBookings")]
        public UserModal.DataModal GetBookings(Responce request)
        {
            try
            {
                objBooking = new BookingBAL();
                result = new UserModal.DataModal()
                {
                    Data = objBooking.GetBookings(request.order_number),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetBookings");
            }
        }
        [HttpPost]
        [Route("BookingCancel")]
        public UserModal.DataModal BookingCancel(Responce request)
        {
            try
            {
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;
                request.RejectByProhit = "N";

                var responce = objBooking.BookingCancel(request, user_Id);
                var GeBookings = objBooking.GetBookings(request.order_number);

                request.Message = Extensions.BookingCancelContent(GeBookings.PujaName, GeBookings.BookingDate, GeBookings.PujaTime, GeBookings.OrderNumber);

                var notification = _notifications.GetPuchNotification(GeBookings.PurohitID, ContentType.MBC.ToString());



                 CommonMethods.OrderNotification(notification, request, ApplicationIdCust, SenderIdCust);
                _notifications.SaveNotifications(GeBookings.PujaName, responce, notification.user_id, notification.contentsId);

                result = new UserModal.DataModal()
                {
                    Data = responce,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/BookingCancel");
            }
        }

        [HttpGet]
        [Route("MyCart")]
        public UserModal.DataModal MyCart()
        {
            try
            {
                objBooking = new BookingBAL();
                var prin = identity();
                var data = objBooking.MyCart(prin.UserDetails.UserID);

                result = new UserModal.DataModal()
                {
                    Data = data,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/GetDraftBookings");
            }
        }

        [HttpPost]
        [Route("DeleteCartItem")]
        public UserModal.DataModal DeleteCartItem(Responce request)
        {
            try
            {
                objBooking = new BookingBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = objBooking.DeleteCartItem(request, user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/DeleteCart");
            }
        }

        [HttpPost]
        [Route("CheckoutMyCartPayment")]
        public UserModal.DataModal CheckoutMyCartPayment(CheckOutRequest request)
        {
            try
            {
                var PaymentPeremeters = new List<PaymentPeremeters>();
                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                _notifications = new NotificationsBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var CustomerData = _objUser.GetUserProfile(user_Id);

                foreach (var order in request.CheckOut)
                {
                    var model = new PaymentPeremeters();
                    var item = objBooking.GetBookings(order.OrderNumber);
                    model.TotalAmount = Convert.ToDecimal(item.PujaDiscountedPrice);

                    if (item.IsSamagri == "1")
                        model.TotalAmount = decimal.Add(model.TotalAmount, Convert.ToDecimal(item.SamagriDiscountedPrice));

                    if (item.BookingType == "Astrologer")
                        model.PujaName = objBooking.GetservicesName(item.PujaID);
                    else
                        model.PujaName = objBooking.GetProductName(item.PujaID);

                    if (request.FullPayment == "N")
                    {
                        var calucatedAmount = (model.TotalAmount * 20 / 100);
                        model.RemainingAmount = decimal.Subtract(model.TotalAmount, calucatedAmount);
                        model.TotalAmount = calucatedAmount;
                    }
                    
                    model.UserId = CustomerData.UserID;
                    model.Name = CustomerData.FirstName + ' ' + CustomerData.LastName;
                    model.Email = CustomerData.Email;

                    model.MobileNumber = CustomerData.MobileNumber;
                    model.FullPayment = request.FullPayment;
                    model.BookingID = item.BookingID;
                    model.OrderNumber = item.OrderNumber;
                    PaymentPeremeters.Add(model);
                }

                var UserId =      string.Join(",", PaymentPeremeters.Select(x => x.UserId).Take(1)).Trim();
                var Name =        string.Join(",", PaymentPeremeters.Select(x => x.Name).Take(1)).Trim();
                var Email =       string.Join(",", PaymentPeremeters.Select(x => x.Email).Take(1)).Trim();
                var MobileNumber= string.Join(",", PaymentPeremeters.Select(x => x.MobileNumber).Take(1)).Trim();
                var FullPayment = string.Join(",", PaymentPeremeters.Select(x => x.FullPayment).Take(1)).Trim();

                var TotalAmount = string.Join(",", PaymentPeremeters.Sum(x => x.TotalAmount));
                var BookingID =   string.Join(",", PaymentPeremeters.Select(x => x.BookingID));
                var OrderNumber = string.Join(",", PaymentPeremeters.Select(x => x.OrderNumber));
                var PujaName =    string.Join(",", PaymentPeremeters.Select(x => x.PujaName));

                result = new UserModal.DataModal()
                {
                    Data = RazorpayCheckoutMyCartPayment(Convert.ToInt32(UserId), Name, Email, MobileNumber, TotalAmount, FullPayment, BookingID, OrderNumber, PujaName),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/CheckoutMyCartPayment");
            }
        }

        [HttpPost]
        [Route("CheckoutMyCart")]
        public UserModal.DataModal CheckoutMyCart(CheckOutRequest request)
        {
            try
            {
                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                var prin = identity();

                int user_Id = prin.UserDetails.UserID;
                var data = objBooking.CheckoutMyCart(request.CheckOut, user_Id);
                var CustomerData = _objUser.GetUserProfile(user_Id);


                // send email to user
                string ordernumber = string.Join(",", request.CheckOut.Select(x => x.OrderNumber));
                StringBuilder sb = new StringBuilder();
                string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.CheckoutMyCartHtmlPath));

                sb.Append(misc_template.Replace("[CustomerName]", CustomerData.Username)
                                       .Replace("[OrderNumber]", ordernumber));


                CommonMethods.SendHtmlMail(new string[] { CustomerData.Email }, new string[] { CommonMethods.EmailCC }, data, sb.ToString(), 0);


                result = new UserModal.DataModal()
                {
                    Data = data,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/CheckoutMyCart");
            }
        }

        [HttpPost]
        [Route("BookingOrderStatus")]
        public UserModal.DataModal BookingOrderStatus(CheckOutResponce _request)
        {
            try
            {

                _objUser = new UserBAL();
                objBooking = new BookingBAL();
                var prin = identity();
                int user_Id = prin.UserDetails.UserID;

                var response = objBooking.BookingOrderStatus(_request);
                result = new UserModal.DataModal()
                {
                    Data = response,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Booking/BookingOrderStatus");
            }
        }

        private static string RazorpayOrder(Userprofile CustomerData, Responce response, string PujaName, decimal totalAmount, string fullpayment, decimal RemainingAmount)
        {
            string paymentUrl;
            paymentUrl = string.Format(CommonMethods.PaymentGateWayUrl +
                      "name="             + CustomerData.FirstName + ' ' + CustomerData.LastName +
                      "&email="           + CustomerData.Email +
                      "&contactNumber="   + CustomerData.MobileNumber +
                      "&amount="          + totalAmount +
                      "&fullpayment="     + fullpayment +
                      "&remainingamount=" + RemainingAmount +
                      "&userid="          + CustomerData.UserID +
                      "&bookingid="       + response.booking_id +
                      "&ordernumber="     + response.order_number +
                      "&pujaname="        + PujaName
                     );
            return paymentUrl;
        }

        private static string RazorpayCheckoutMyCartPayment(int userid, string Name, string Email, string MobileNumber, string totalAmount, string Fullpayment, string BookingID, string OrderNumber, string PujaName)
        {
            string paymentUrl;
            paymentUrl = string.Format(CommonMethods.CheckoutMyCartPaymentGateWayUrl +
                      "userid="         +  userid +
                      "&name="           + Name +
                      "&email="         + Email +
                      "&contactNumber=" + MobileNumber +
                      "&amount="        + totalAmount +
                      "&fullpayment="   + Fullpayment +
                      "&bookingid="     + BookingID +
                      "&ordernumber="   + OrderNumber +
                      "&pujaname="      + PujaName
                     );
            return paymentUrl;
        }

        private static BasicAuthenticationAttribute.MyPrincipal identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}