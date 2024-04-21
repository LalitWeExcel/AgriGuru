using System.Collections.Generic;

namespace SwarajCustomer_Common.ViewModel
{
    public class ManagePaymentViewModel
    {
        public IList<M_ManagePayment> ManagePaymentList { get; set; }
        public int TotalRecords { get; set; }
        public int NoOfRecords { get; set; }

        public int TotelSuccessPayment { get; set; } = 0;
        public int TotelFailedPayment { get; set; } = 0;
        public decimal TotelRevenue { get; set; } = 0;

    }

    public class M_ManagePayment
    {
        public int SrNo { get; set; }
        public int BookingID { get; set; }
        public string OrderNumber { get; set; }
        public string BookingDate { get; set; }
        public int trn_razorpay_order_id { get; set; }
        public string rzp_paymentid { get; set; }
        public string rzp_orderid { get; set; }
        public decimal rzp_amount { get; set; }


        public string payment_status { get; set; }
        public string error_code { get; set; }
        public string error_desc { get; set; }
        public string txn_id { get; set; }
        public string payment_mode { get; set; }
        public string status { get; set; } = "F";
        public string payment_details { get; set; }
        public string RazorPayDate { get; set; }
        public int UserID { get; set; }
        public int PurohitID { get; set; }
        public int PujaID { get; set; }
        public string PujaName { get; set; }
        public string TimeSlot { get; set; }
        public string PurohitName { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }

        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustDistrict { get; set; }
        public string BookingType { get; set; }
        public string Paid { get; set; }
        public string OrderStatus { get; set; }
        public string IsActive { get; set; }
        public string OTPConfirm { get; set; }
        public string AddToCart { get; set; }
        public string full_payment { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    

    public class M_SaveManagePayment
    {
        public int SrNo { get; set; }
        public string rzp_paymentid { get; set; }
        public string rzp_orderid { get; set; }
        public decimal rzp_amount { get; set; }
        public int user_id { get; set; }
        public int booking_id { get; set; }
        public string order_number { get; set; }

        public string payment_status { get; set; }
        public string error_code { get; set; }
        public string error_desc { get; set; }
        public string txn_id { get; set; }
        public string payment_mode { get; set; }
        public string status { get; set; } = "F";
        public string payment_details { get; set; }
        public string full_payment { get; set; }
        public decimal remaining_amount { get; set; }

    }

    public class M_SavePremiummemberShip
    {
        public int SrNo { get; set; }
        public string rzp_paymentid { get; set; }
        public string rzp_orderid { get; set; }
        public decimal rzp_amount { get; set; }
        public int user_id { get; set; }
        public int premium_membership_id { get; set; }
        public string role_name { get; set; }
        public string payment_status { get; set; }
        public string error_code { get; set; }
        public string error_desc { get; set; }
        public string txn_id { get; set; }
        public string payment_mode { get; set; }
        public string payment_details { get; set; }
        
        public string status { get; set; } = "F";
    }

}
