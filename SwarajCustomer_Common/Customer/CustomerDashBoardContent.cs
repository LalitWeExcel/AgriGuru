using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Customer
{
    public  class CustomerDashBoardContent
    {
        public List<Advertisement> GetDashBoardAdvertisements { get; set; }
        public List<UserDetails> GetTopAstrologersPurohits { get; set; }
    }
}
