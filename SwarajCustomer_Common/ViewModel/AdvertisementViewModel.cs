using System.Collections.Generic;

namespace SwarajCustomer_Common.ViewModel
{
    public class AdvertisementViewModel
    {
        public IList<M_Advertisement> AdvertisementList { get; set; }
        public int TotalRecords { get; set; }
        public int NoOfRecords { get; set; }
    }

    public class M_Advertisement
    {
        public int SrNo { get; set; }
        public int AdvertisementId { get; set; }
        public int ProductID { get; set; }
        public int CatagoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal Discount { get; set; }
        public int DiscountInRupees { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string Service { get; set; }
        public string ServiceCode { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int? SamagriID { get; set; }
        public string BookingDate { get; set; }
        public string Url { get; set; }
        public string DurationText { get; set; }
        public string IsGlobal { get; set; } = "N";
        public string AdvertisingType { get; set; } = "L";
        public string IsActive { get; set; } = "N";

        public string ImageName { get; set; }
        public string VideoName { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }

    }


    public class M_SaveAdvertisement
    {
        public int AdvertisementId { get; set; }
        public int CategoryId { get; set; }
        public int MainProductId { get; set; }
        public decimal Discount { get; set; }
        public int DiscountInRupees { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string IsGlobal { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string DistrictId { get; set; }
        public string BookingDate { get; set; }
        public int TimeSlotid { get; set; }
        public string Url { get; set; }
        public string IsActive { get; set; }
        public decimal MRP { get; set; }
        public int Duration { get; set; }
        public string ImageName { get; set; }
        public string VideoName { get; set; }
        public string Title { get; set; }
        public List<MuiltipleDistrict> MuiltipleDistrictList { get; set; }
    }

    public class MuiltipleDistrict
    {
        public int DistrictId { get; set; } 
        public string DistrictName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class M_SaveIndependentads
    {
        public int AdvertisementId { get; set; }
        public string Title { get; set; }
        public decimal Discount { get; set; }
        public decimal MRP { get; set; }
        public int DiscountInRupees { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string VideoName { get; set; }

    }
}
