using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarajCustomer_Common.ViewModel
{
  public  class ManageFavVideosViewModel
    {
        public IList<M_ManageFavVideos> ManageFavVideosList { get; set; }
        public int TotalRecords { get; set; }
        public int NoOfRecords { get; set; }
    }

    public class M_ManageFavVideos
    {
        public int SrNo { get; set; }
        public int ManageFavVideosId { get; set; }
        public int LanguageId { get; set; } = 0;       
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdvertisingType { get; set; }     
        public string IsActive { get; set; } = "N";
        public string VideoName { get; set; }
    }
    public class M_ManageFavVideosRequest
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoName { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public int LanguageId { get; set; } = 0;
        public string AdvertisingType { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
    }
}
