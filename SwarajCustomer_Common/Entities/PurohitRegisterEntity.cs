using System.Collections.Generic;

namespace SwarajCustomer_Common.Entities
{
    public class PurohitRegisterEntity
    {
        public int prohit_id { get; set; } = 0;
        public string purohit_name { get; set; } = "";
        public string mobile_number { get; set; } = "";
        public string alternate_number { get; set; } = "";
        public string address { get; set; } = "";
        public string referal_code { get; set; } = "";
        public int stateId { get; set; } = 0;
        public int districtId { get; set; } = 0;
        public string landmark { get; set; } = "";
        public int pin_code { get; set; } = 0;
        public int purohit_experience { get; set; } = 0;
        public int astro_experience { get; set; } = 0;
        public string is_prohit { get; set; } = "N";
        public string is_astro { get; set; } = "N";
        public string temple_name { get; set; } = "N";
        public string latitude { get; set; } = "";
        public string longitude { get; set; } = "";
        public string ImageName { get; set; } = "";
        
        public List<PurohitPujaPath> PurohitPujaPath { get; set; } = new List<PurohitPujaPath>();
        public List<AstrologerServices> AstrologerServices { get; set; } = new List<AstrologerServices>();
    }

    public class PurohitPujaPath
    {
        public int Id { get; set; } = 0;
        public int puja_path_Id { get; set; } = 0;
    }
    public class AstrologerServices
    {
        public int Id { get; set; } = 0;
        public int service_Id { get; set; } = 0;
    }
    public class PurohitEntity
    {
        public int ProhitId { get; set; } = 0;
        public string Username { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string Description { get; set; } = "";
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
        public string MembershipType { get; set; } = "";
        public int MembershipId { get; set; } = 0;
        public string RoleName { get; set; } = "";
        public string RoleId { get; set; } = "";
        public string temple_name { get; set; } = "N";
        public string UserType { get; set; } 
        public string is_prohit { get; set; } = "N";
        public string is_astro { get; set; } = "N";

        public string Gender { get; set; } = "";
        public string Profession { get; set; } = "";
        public List<PurohitPujaPath> PurohitPujaPath { get; set; } = new List<PurohitPujaPath>();
        public List<AstrologerServices> AstrologerServices { get; set; } = new List<AstrologerServices>();
    }
}
