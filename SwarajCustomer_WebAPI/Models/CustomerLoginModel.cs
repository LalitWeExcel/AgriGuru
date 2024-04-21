using System.ComponentModel.DataAnnotations;

namespace SwarajCustomer_WebAPI.Models
{
    public class CustomerLoginModel
    {
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }

    }

    public class CustomerOTPModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "Invalid OTP.")]
        public string OTP { get; set; }
        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


    }
}