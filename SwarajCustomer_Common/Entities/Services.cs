using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarajCustomer_Common.Entities
{
    public class ServicesList
    {
        public List<ServiceMaster> services { get; set; }
        public List<ZodiacMaster> zodiacs { get; set; }
    }

    public class ServiceMaster
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormValue { get; set; }
    }

    public class ZodiacMaster
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DateRange { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}
