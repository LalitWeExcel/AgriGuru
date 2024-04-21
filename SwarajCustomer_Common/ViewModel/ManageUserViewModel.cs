using SwarajCustomer_Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwarajCustomer_Common.ViewModel
{
    public class ManageUserViewModel
    {
        public IList<M_ManageUser> ManageUserList { get; set; }
        public int TotalRecords { get; set; } = 0;
        public int NoOfRecords { get; set; } = 0;
        public int TotalProhits { get; set; } = 0;
        public int TotalAstrollgers { get; set; } = 0;
        public int TotalCustomers { get; set; } = 0;

    }

    public class M_ManageUser
    {
        public int SrNo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ReferalCode { get; set; }
        public string UserType { get; set; }
        public string IsActive { get; set; }
        public string ImageName { get; set; }
        public string Createdby { get; set; }
        


    }
    public class M_ManageUserDetails
    {
        
        public int SrNo { get; set; }
        public int adm_user_id { get; set; }

        [Required(ErrorMessage = "User Name Required")]
        [StringLength(50, ErrorMessage = "User Name Must be under 50 characters")]
        public string username { get; set; }
        public string mob_number { get; set; }
        public string email { get; set; }
        public string user_type { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [StringLength(50, ErrorMessage = "Description Must be under 50 characters")]
        public string description { get; set; }
        public int birthday_email_sent { get; set; } = 0;
        public string referal_code { get; set; }
        public string ImageName { get; set; }
        public string is_active { get; set; }
        public int user_details_id { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [StringLength(50, ErrorMessage = "First Name Must be under 50 characters")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [StringLength(50, ErrorMessage = "Last Name Must be under 50 characters")]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Whats-App Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = " Whats-App Number Must be under 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string whatsapp_no { get; set; }

        [Required(ErrorMessage = "Address Required")]
        [StringLength(50, ErrorMessage = "Address Must be under 50 characters")]
        public string address { get; set; }

        [Required(ErrorMessage = "City Required")]
        [StringLength(50, ErrorMessage = "City Name Must be under 50 characters")]
        public string city { get; set; }

        [Required(ErrorMessage = "State Name Required")]
        [RegularExpression(@"^(?!0)[0-9]{1,1000}$", ErrorMessage = "Please Select State Name")]
        public int mst_state_id { get; set; }

        [Required(ErrorMessage = "District Name Required")]
        [RegularExpression(@"^(?!0)[0-9]{1,1000}$", ErrorMessage = "Please Select District Name ")]
        public int mst_district_id { get; set; }

        public string state { get; set; }
        public string district { get; set; }

        [Required(ErrorMessage = "Pin Code Required")]
        [RegularExpression(@"^(?!000000)[0-9]{6,8}$", ErrorMessage = "Pin Code Must be under 6-8 characters")]
        public string pincode { get; set; }
        public string export_in_pooja { get; set; }
        public string ready_for_travel { get; set; }
        public int year_of_exp { get; set; }
        public string marital_status { get; set; }

        [Required(ErrorMessage = "Date of Birth Required")]
        public string DOB { get; set; }
        [Required(ErrorMessage = "Time of Birth Required")]
        public string TOB { get; set; }
        [Required(ErrorMessage = "Place of Birth Required")]
        public string PlaceOfBirth { get; set; }

        [Required(ErrorMessage = "Adhar Card Number is required.")]
        [RegularExpression(@"^([0-9]{16})$", ErrorMessage = "Adhar Card Number Must be under 16 characters")]
        [DataType(DataType.PhoneNumber)]
        public string aadhar_number_name { get; set; }
        public int qualification { get; set; }
        public int health_status { get; set; }
        public int rating { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string landmark { get; set; }
        public int astrologer_experience { get; set; }
        public string gender { get; set; }
        public string profession { get; set; }
        
        public string temple_name { get; set; }
        public List<M_ManageUserRoles> ManageUserRoles { get; set; }=
        new List<M_ManageUserRoles>();
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
        public List<Service> Services { get; set; } = new List<Service>();
        public List<M_Createdby> Createdby { get; set; } = new List<M_Createdby>();

    }

    public class M_HourlyTime
    {
        public int ID { get; set; }
        public string Time { get; set; }
    }
    public class M_ManageUserRoles
    {
        public int adm_user_role_id { get; set; }
        public int adm_user_id { get; set; }
        public int adm_role_id { get; set; }
        public string role_name { get; set; }
    }
    public class M_Createdby
    {
        public int Ids { get; set; }
        public string Name { get; set; }
    }

    public class M_ActiveDisActive
    {
        public int Id { get; set; }
    }

 

}

