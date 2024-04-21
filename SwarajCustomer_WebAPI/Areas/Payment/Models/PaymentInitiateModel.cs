namespace SwarajCustomer_WebAPI.Areas.Payment.Models
{
    public class PaymentInitiateModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        public decimal amount { get; set; }

        public int userid { get; set; }
        public int bookingid { get; set; }
        public string ordernumber { get; set; }
        public string pujaname { get; set; }
        public string fullpayment { get; set; }
        public decimal remainingamount { get; set; }
        

    }

    public class PremiumMembershipModel
    {
        public string orderId { get; set; }
        public string razorpayKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public decimal Amount { get; set; }
        public int UserID { get; set; }
        public int PremiumMembershipID { get; set; }
        public string RoleName { get; set; }
        public string currency { get; set; }
    }

    public class CheckoutMyCartPaymentModel
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string amount { get; set; }
        public string fullpayment { get; set; }
        public string bookingid { get; set; }
        public string ordernumber { get; set; }
        public string pujaname { get; set; }

    }
}
