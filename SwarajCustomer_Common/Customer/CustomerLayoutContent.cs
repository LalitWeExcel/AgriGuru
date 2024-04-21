using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Customer
{
    public class CustomerLayoutContent
    { 
        public List<NotificationsEntity> M_Notifications { get; set; }
        public MyCartResponce MyCart { get; set; }
    }
}
