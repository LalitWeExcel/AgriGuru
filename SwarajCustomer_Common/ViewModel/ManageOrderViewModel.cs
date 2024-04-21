using System.Collections.Generic;

namespace SwarajCustomer_Common.ViewModel
{
    public  class ManageOrderViewModel
    {
        public IList<M_ManageOrder> ManageOrdertList { get; set; }
        public int TotalRecords { get; set; }
        public int NoOfRecords { get; set; }
    }

    public class M_ManageOrder
    {
        
       public int SrNo { get; set; }
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public string BookingDate { get; set; }
        public int PurohitID { get; set; }
        public int UserID { get; set; }
        public int PujaID { get; set; }
        public int NoOfPandit { get; set; }
        public string PujaName { get; set; }
        public string PujaDate { get; set; }
        public string TimeSlot { get; set; }
        public string IsSamagri { get; set; }
        public string PujaMRP { get; set; }
        public string PujaDiscount { get; set; }
        public string PujaDiscountedPrice { get; set; }
        public string WithSamagri { get; set; }
        public string SamagriMRP { get; set; }
        public string SamagriDiscount { get; set; }
        public string PurohitName { get; set; }
        public string Paid { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string CustAddress { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustDistrict { get; set; }
        public string CustPincode { get; set; }
        public string OrderStatus { get; set; }
        public int PurohitRating { get; set; }
        public string OTP { get; set; }
        public string OTPConfirm { get; set; }
        public string BookingType { get; set; }
        public string AddToCart { get; set; }
        public string WhatsApp { get; set; }
        public string Skype { get; set; }
        public string EPujaEmail { get; set; }
        public string EPujaPhoneNo { get; set; }
        public string PersionofContactName { get; set; }
        public string PersionofContactNumber { get; set; }
        public string IsActive { get; set; }
        public string IsTemp { get; set; }
        public string Ression { get; set; }
        public string ConfirmStatus { get; set; }
        public string RejectByProhit { get; set; } = "N";
        public string Buttion { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentDetails { get; set; }

    }
    public class M_UpdatePackage
    {
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public string NewPurohitID { get; set; }

        public int OldProhitID { get; set; }
    }

    public class M_PackagerProhits
    {
        public int Ids { get; set; }
        public string Name { get; set; }
    }
    public class M_UpdateProhits
    {
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public string BookingType { get; set; }
        public int NewPurohitID { get; set; }
        public string Ression { get; set; }
        public int OldProhitID { get; set; }
    }
}
