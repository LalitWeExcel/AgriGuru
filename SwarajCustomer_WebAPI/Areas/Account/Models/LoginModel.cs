using System.ComponentModel.DataAnnotations;

namespace SwarajCustomer_WebAPI.Areas.Account.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}