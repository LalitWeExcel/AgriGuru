namespace SwarajCustomer_Common.Entities
{
    public class NotificationEnitity
    {
        public string device_key { get; set; }
        public int device_type { get; set; }
        public string contents { get; set; }
        public int contentsId { get; set; }
        public string contentsType { get; set; }
        public int user_id { get; set; }
        public string CustomerMobile { get; set; }

        public string Title { get; set; }
        public int BookingId { get; set; }
        public string OrderNumber { get; set; }
        public int Type { get; set; }
        public int RecordType { get; set; }
        public object Details { get; set; }


    }
    public class Notification_Data_Message
    {
        public int NotificationId { get; set; }
        public string Info { get; set; }
    }
}
