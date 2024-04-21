using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwarajCustomer_Common.Entities
{
    public class UserLoginEntity
    {
        public string Username { get; set; } = "";
        public string DeviceToken { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string IsValidUser { get; set; } = "";
        public int UserID { get; set; } = 0;
        public string UserRole { get; set; } = "";
        public bool IsOTPSend { get; set; } = false;
        public string Email { get; set; } = "";
        public double Latitude { get; set; } = 0.00;
        public double Longitude { get; set; } = 0.00;
        public int ServiceID { get; set; } = 0;
        public string ReferalCode { get; set; } = "";
        public string Type { get; set; } = "";
    }

    public class LogOffEntity
    {
        public int UserId { get; set; } = 0;
        public string DeviceToken { get; set; } = "";
    }


    public class DeviceToken
    {
        public int UserId { get; set; }
        public int DeviceType { get; set; }
        public string DeviceKey { get; set; }

    }

    public class UserDetails
    {
        public int? UserID { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? UserTypeID { get; set; }
        public string UserType { get; set; }
        public string Description { get; set; }
        public string ReferalCode { get; set; }
        public string ImageName { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WhatsappNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public bool IsOTPSend { get; set; }
        public string OTP { get; set; }
        public string IsValidUser { get; set; }
        public string Badge { get; set; }
        public int MembershipId { get; set; }
        public string MembershipType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? Rating { get; set; }
        public int? Experience { get; set; }
        public int AstrologerExperience { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string IsSuccess {get;set;}
        public List<Specialization> Specializations { get; set; }
        public List<Service> Services{ get; set; }
    }

    public class Userprofile
    {
        public int UserID { get; set; } = 0;
        public string Username { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string Description { get; set; } = "";
        public string UserType { get; set; } = "";
        public string ReferalCode { get; set; } = "";
        public string ImageName { get; set; } = "";


        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string WhatsappNo { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string DOB { get; set; } = "";
        public string TOB { get; set; } = "";
        
        public string AadharCardNumber { get; set; } = "";
        public int Experience { get; set; } = 0;
        public int AstrologerExperience { get; set; } = 0;
        
        public string Latitude { get; set; } = "";
        public string Longitude { get; set; } = "";
        public string Landmark { get; set; } = "";
        public string PinCode { get; set; } = "";
        public int StateId { get; set; } = 0;
        public int DistrictId { get; set; } = 0;
        public string StateName { get; set; } = "";
        public string DistrictName { get; set; } = "";
        public string Badge { get; set; } = "";
        public string RoleName { get; set; } = "";
        public int RoleId { get; set; } =0;
        public string Gender { get; set; } = "";
        public string Profession { get; set; } = "";

    }

    public class UserDataModel
    {
        public int UserID { get; set; } = 0;
        public string Username { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string Email { get; set; } = "";

       [DataType(DataType.Password)]
        //[Required(ErrorMessage = "The Old Password cannot be empty.")]
       [DisplayName("OldPassword")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The Password cannot be empty.")]
        [DisplayName("Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; } 

        [NotMapped]
        [DataType(DataType.Password)]
        [DisplayName("Password confirmation")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "The Confirm Password passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string ReferalCode { get; set; } 
        public string IsVaild { get; set; } = "N";
        public string Message { get; set; } = "";
        public string expired { get; set; } = "";
        public string keys { get; set; } = "";
    
    }




    public class Service
    {
        public int? AstID { get; set; }
        public int? ServiceID { get; set; }
        public string Name { get; set; } = "";
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }

    }

    public class Specialization
    {
        public int? UserID { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public partial class OtpList
    {
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
    }
}
