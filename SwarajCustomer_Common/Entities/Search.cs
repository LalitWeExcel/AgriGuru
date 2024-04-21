using System.Collections.Generic;

namespace SwarajCustomer_Common.Entities
{
    public class SearchReq
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Text { get; set; }
    }

    public class MasterRequest
    {
        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;
        public string Language { get; set; } = "";
    }

    public class SearchRes
    {
        public int ID { get; set; }
        public string Name{ get; set; }
        public string ImageName { get; set; }
        
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
