namespace SwarajCustomer_WebAPI.Models
{
    public class UserModal
    {
        public class DataModal
        {
            public object Data { get; set; }
            public string Response { get; set; }
            public bool Status { get; set; }
            public string Message { get; set; }
            public bool IsUserActive { get; set; }
        }
    }
}