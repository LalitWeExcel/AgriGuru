using System;

namespace SwarajCustomer_Common.Entities
{
    public class SMSEntity
    {
        public int SMS_Gateway_ID { get; set; }
        public string Module { get; set; }
        public string SMS_Gateway_Provider_Name { get; set; }
        public string SMS_Gateway_API { get; set; }
        public string SMS_Contact_No { get; set; }
        public string SMS_Text { get; set; }
        public Nullable<bool> Is_Active { get; set; }
    }
}
