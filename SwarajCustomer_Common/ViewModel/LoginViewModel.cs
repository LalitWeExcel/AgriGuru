namespace SwarajCustomer_Common.ViewModel
{
    public partial class LoginViewModel
    {
        public int UserId { get; set; } = 0;
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }

    }
    public partial class CustomerLoginModel
    {
        public int UserId { get; set; } = 0;
        public string ImageName { get; set; } = "";
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public string LogsException { get; set; }
        public bool IsSuccess { get; set; }
    }
}

