namespace SwarajCustomer_WebAPI.Areas.Payment.Models
{
    public class OrderModel
    {     public string orderId { get; set; }
        public string razorpayKey { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        public string description { get; set; }


        public int userid { get; set; }
        public int bookingid { get; set; }
        public string ordernumber { get; set; }
        public string fullpayment { get; set; }
        public decimal remainingamount { get; set; }
    }

    public class CheckoutMyCartOrderModel
    {
        public string orderId { get; set; }
        public string razorpayKey { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public int userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string description { get; set; }
        public string ordernumber { get; set; }
        public string bookingid { get; set; }
        
        public string fullpayment { get; set; }
    }
}