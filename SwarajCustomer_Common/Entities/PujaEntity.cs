using System;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Entities
{
    public class PujaEntity
    {
        public int PujaID { get; set; }
        public DateTime PujaDate { get; set; }
    }

    public class AstroEntity
    {
        public int AstroID { get; set; }
        public DateTime AstroDate { get; set; }
    }

    public class PujaOrderDetails
    {
        public List<PujaDetails> PujasList { get; set; }
        public CustomerDetails CustomerDetail { get; set; }
        public string IsAddToCart { get; set; } = "N";
        public string FullPayment { get; set; } = "N";

    }

    public class PackageOrderDetails
    {
        public List<PackageDetails> PackageList { get; set; }
        public CustomerDetails CustomerDetail { get; set; }
        public string IsAddToCart { get; set; } = "N";
        public string FullPayment { get; set; } = "N";

    }
    public class PackageDetails
    {
        public int MainProductId { get; set; }
        public int PackageId { get; set; }
        public int ProhitDurationId { get; set; }
        public string PackageDate { get; set; }
        public int PackageTime { get; set; }
        public string PackageMRP { get; set; }
        public string PackageDiscount { get; set; }
        public string PackageDiscountedPrice { get; set; }
        public string PersionofContactName { get; set; }
        public string PersionofContactNumber { get; set; }
        public int Purohit_Id { get; set; } = 0;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string IsForOther { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OTP { get; set; }
        public string IsPaid { get; set; } = "N";

    }
    public class PujaDetails
    {
        public int PujaID { get; set; }
        public string PujaDate { get; set; }
        public int PujaTime { get; set; }
        public string PujaMRP { get; set; }
        public string PujaDiscount { get; set; }
        public string PujaDiscountedPrice { get; set; }
        public bool IsSamagri { get; set; }
        public string SamagriMRP { get; set; }
        public string SamagriDiscount { get; set; }
        public string SamagriDiscountedPrice { get; set; }
        public int PurohitID { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string IsForOther { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OTP { get; set; }
        public string IsPaid { get; set; } = "N";
        public string EPujaEmail { get; set; }
        public string EPujaPhoneNo { get; set; }

    }

    public class MyBookingsReq
    {
        public int UserID { get; set; }
    }


    public class RemimnderReq
    {
        public string PujaName { get; set; } = "";
        public string OrderNumber { get; set; } = "";
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class Responce
    {
        public string result { get; set; }
        public int booking_id { get; set; }
        public string order_number { get; set; }
        public string Message { get; set; }
        public string RejectByProhit { get; set; } = "N";

    }
    public class PaymentStatus
    {
        public string paymentStatus { get; set; }

    }

    public class MyBookingsRes
    {
        public List<MyBookings> UpcomingBookings { get; set; }
        public List<MyBookings> CompletedBookings { get; set; }
        public List<MyBookings> CancelledBookings { get; set; }
    }

    public class MyBookings
    {
        public PujaBooking OrderDetails { get; set; }
        public CustomerBooking CustomerDetails { get; set; }
    }

    public class PujaBooking
    {
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public int PujaID { get; set; }
        public string PujaName { get; set; }
        public string BookingDate { get; set; }
        public string PujaTime { get; set; }
        public string PujaMRP { get; set; } = "0";
        public string PujaDiscount { get; set; } = "0";
        public string PujaDiscountedPrice { get; set; } = "0";
        public string IsSamagri { get; set; } = "0";
        public string SamagriMRP { get; set; } = "0";
        public string SamagriDiscount { get; set; } = "0";
        public string SamagriDiscountedPrice { get; set; } = "0";
        public int PurohitID { get; set; }
        public int UserID { get; set; }

        public string PurohitName { get; set; } = "N/A";
        public string Paid { get; set; } = "N/A";
        public string OrderStatus { get; set; } = "N/A";
        public string OTP { get; set; } = "N/A";
        public string OTPConfirm { get; set; } = "N";
        public string PurohitRating { get; set; } = "N/A";
        public string WhatsApp { get; set; } = "N/A";
        public string Skype { get; set; } = "N/A";
        public string BookingType { get; set; } = "N/A";
        public string EPujaEmail { get; set; } = "N/A";
        public string EPujaPhoneNo { get; set; } = "N/A";
        public string PersionofContactName { get; set; } = "N/A";
        public string PersionofContactNumber { get; set; } = "N/A";

        public string status { get; set; } = "F";
        public string RazorPayDate { get; set; } = "N/A";
        public string payment_status { get; set; } = "N/A";
        public decimal rzp_amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TotalAmount { get; set; }

    }

    public class CustomerBooking
    {
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string CustAddress { get; set; }
        public string CustCity { get; set; }
        public string CustPincode { get; set; }
        public string CustState { get; set; }
    }

    public class AstroOrderDetails
    {
        public List<AstroDetails> OrderList { get; set; }
        public CustomerDetails CustomerDetail { get; set; }
        public string IsAddToCart { get; set; } = "N";
        public string FullPayment { get; set; } = "N";
    }

    public class AstroDetails
    {
        public int ServiceID { get; set; }
        public string BookingDate { get; set; }
        public int BookingTime { get; set; }
        public int BookingDuration { get; set; }
        public string AstroMRP { get; set; }
        public string AstroDiscount { get; set; }
        public string AstroDiscountedPrice { get; set; }
        public int AstrologerID { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string IsForOther { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OTP { get; set; }
        public int ConsultationMediumID { get; set; }
        public int ConsultationTypeID { get; set; }
        public string Whatsapp { get; set; }
        public string Skype { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string TOB { get; set; }
        public string POB { get; set; }
        public string IsPaid { get; set; } = "N";
    }

    public class MyCartResponce
    {
        public List<MyBookings> MyCart { get; set; }
    }

    public class CheckOutRequest
    {
        public List<CheckOutResponce> CheckOut { get; set; }
        public string FullPayment { get; set; } = "N";
    }

    public class CheckOutResponce
    {
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
     

    }

    public class PaymentPeremeters
    {
        public string PaymentUrl { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string FullPayment { get; set; }
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public string PujaName { get; set; }

    }

    public class BookingOrderModel
    {
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public int PujaID { get; set; }
        public string PujaName { get; set; }
        public string BookingDate { get; set; }
        public string PujaTime { get; set; }
        public int PurohitID { get; set; }
        public int UserID { get; set; }
        public string BookingType { get; set; }
        
        public string PurohitName { get; set; } = "N/A";
        public string status { get; set; } = "F";
        public string RazorPayDate { get; set; } = "N/A";
        public string payment_status { get; set; } = "N/A";
        public string PaymentMode { get; set; } = "N/A";
        public string PaymentDetails { get; set; } = "N/A";
        public decimal rzp_amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string CustEmail { get; set; }
        public string CustAddress { get; set; }
        public string CustCity { get; set; }
        public string CustPincode { get; set; }
        public string CustState { get; set; }

    }

    public class IndependentAdsDetails
    {
        public List<IndependentAds> IndependentAdsList { get; set; }
        public CustomerDetails CustomerDetail { get; set; }
        public string IsAddToCart { get; set; } = "N";
        public string FullPayment { get; set; } = "N";

    }

    public class IndependentAds
    {
        public int TrnAdsId { get; set; }
        public string PujaDate { get; set; }
        public int PujaTime { get; set; }
        public string PujaMRP { get; set; }
        public string PujaDiscount { get; set; }
        public string PujaDiscountedPrice { get; set; }
        public int PurohitID { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string IsForOther { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OTP { get; set; }
        public string IsPaid { get; set; } = "N";

    }
}
