using SwarajCustomer_Common.ViewModel;
using System;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Entities
{
    public class DashboardContent
    {

        public List<Advertisement> DashBoardAdvertisements { get; set; }
        public List<UserDetails> purohits_astrologers { get; set; }
    }

    public class Advertisement
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
        public int BookingTime { get; set; }
		public string Url { get; set; }
        public string DurationText { get; set; }
        public string IsGlobal { get; set; } = "N";
        public string AdvertisingType { get; set; } = "L";
        public string IsActive { get; set; } = "N";

        public string ImageName { get; set; }
        public string VideoName { get; set; }
        public string StateName { get; set; }
        public List<MuiltipleDistrict> MuiltipleDistrictList { get; set; }
    }

    public class Masters
    {
        public List<PujaPath> PujaList { get; set; }
        public List<PujaPath> PathList { get; set; }
        public List<PujaPath> CorporatePujaList { get; set; }
        public List<PujaPath> ServicePackageList { get; set; }

        public List<UserDetails> PurohitList { get; set; }
        public List<UserDetails> AstrologerList { get; set; }
        public List<ZodiacMaster> HoroscopeList { get; set; }
        public List<SamagriCategoryMaster> SamagriList { get; set; }
        public List<TimeSlotMaster> TimeSlotsList { get; set; }
        public List<ConsultationMedium> ConsultationMediumList { get; set; }
        public List<ServiceMaster> ServicesList { get; set; }
        public List<DurationSlotMaster> DurationSlotsList { get; set; }
        public List<StateMaster> StateMasterList { get; set; }
        public List<DistrictMaster> DistrictMasterList { get; set; }
        public string ReferalCode { get; set; }
        public List<Packages> PackageList { get; set; }
        public List<ProhitDuration> ProhitDuration { get; set; }
        public List<PricePercentage> PricePercentage { get; set; }
        public List<PremiumMembership> PremiumMembership { get; set; }
        public List<LanguageMaster> LanguageMaster { get; set; }
        
    }


    public class ConsultationMedium
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ConsultationType> ConsultationTypeList { get; set; }
    }

    public class ConsultationType
    {
        public int ID { get; set; }
        public int MediumID { get; set; }
        public string Name { get; set; }
    }

    public class ProhitDuration
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
    }

    public class PricePercentage
    {
        public int ID { get; set; }
        public string LocationCategory { get; set; }
        public int CategoryID { get; set; }
        public decimal Percentage { get; set; }

    }

    public class PremiumMembership
    {
        public int PremiumMembershipID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DurationInMonths { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
    public class PujaPath
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImagePath { get; set; }
        public int Duration { get; set; }
        public int? SamagriID { get; set; }
        public string DurationText { get; set; }
    }


    public class Packages
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public int NoOfPandit { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Duration { get; set; }
        public int SamagriId { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int CategoryID { get; set; }


        public List<Procedureinvolve> Procedureinvolve { get; set; } = new List<Procedureinvolve>();

    }
    public class Procedureinvolve
    {
        public string Name { get; set; }
    }


    public class SamagriCategoryMaster
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
        public List<SamagriMaster> SamagriList { get; set; }
    }

    public class SamagriMaster
    {
        public int ID { get; set; }
        public int SamagriCategoryID { get; set; }
        public string Name { get; set; }
        public string HindiName { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
    }

    public class TimeSlotMaster
    {
        public int ID { get; set; }
        public string TimeSlot { get; set; }
        public DateTime? StartTime { get; set; }
        public string DurationSlot { get; set; }
        public int Duration { get; set; }
        public string DurationText { get; set; }
    }

    public class DurationSlotMaster
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string DurationText { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int CategoryID { get; set; }
    }

    public class StateMaster
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class DistrictMaster
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public string LocationCategory { get; set; }

    }

    public class FeedBackEntity
    {
        public int mst_feedback_Id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string answer_type { get; set; }
        public string sort_order { get; set; }
    }

    public class SaveFeedbackRequest
    {
        public List<Feedback> Feedback { get; set; }
    }

    public class Feedback
    {
        public int mst_feedback_Id { get; set; }
        public string response { get; set; }
    }

    public class NotificationsEntity
    {
        public int adm_user_id { get; set; }
        public string username { get; set; }
        public string mob_number { get; set; }
        public string email { get; set; }
        public int trn_notifications_Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string notifications_type { get; set; }
        public DateTime date { get; set; }

    }
    public class RatingEntity
    {
        public int mst_rating_Id { get; set; } = 0;
        public string order_no { get; set; } = "";
        public int purohit_Id { get; set; } = 0;
        public int user_Id { get; set; } = 0;
        public int rating { get; set; } = 0;
        public string remarks { get; set; } = "";
    }
    public class LanguageMaster
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
