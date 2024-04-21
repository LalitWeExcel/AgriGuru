using SwarajCustomer_Common.Utility;
using System;

namespace SwarajCustomer_Common.Entities
{
    public class CustomerLogin
    {
        public string MobileNo { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
    }

    public class CustomerLoginEntity : MessageModel
    {
        public string MobileNo { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string IsValidUser { get; set; }
        public int CustomerID { get; set; }
        public string Role { get; set; }
    }

    public class CustomerDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int? DistrictID { get; set; }
        public int? StateID { get; set; }
        public string DOB { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }

        public string DealerCode { get; set; }
        public int? DealerID { get; set; }
        public string IsValidUser { get; set; }
        public string Mobile { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string OTP { get; set; }
        public string Role { get; set; }
        public int Cust_Id { get; set; }
        public string TringoId { get; set; }
        public bool IsGuestUser { get; set; }
        public int StateId { get; set; }
        public string TringoVillage { get; set; }
        public string TringoAddress { get; set; }
    }
}
