using SwarajCustomer_Common.Entities;

namespace SwarajCustomer_Common.Customer
{
	public class IndependentsAdsContent
	{
		public Masters Masters { get; set; }
		public Userprofile Userprofile { get; set; }
		public int TrnAdsId { get; set; }
		public string Name { get; set; }
		public string PujaMRP { get; set; }
		public string PujaDiscount { get; set; }
		public int DiscountInRupees { get; set; }
		public string PujaDiscountedPrice { get; set; }
	
		
	}
}
