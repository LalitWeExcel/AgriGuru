using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Customer
{
    public  class BookPurohitContent
    {
        public Masters Masters { get; set; }
        public List<UserDetails> GetTopAstrologersPurohits { get; set; }
        public Userprofile Userprofile { get; set; }
        public int PurohitID { get; set; }
        
    }
}
