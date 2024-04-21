using SwarajCustomer_Common.Entities;

namespace SwarajCustomer_Common.Customer
{
	public class BookPujaPackagesContent
	{
		public Masters Masters { get; set; }
		public Userprofile Userprofile { get; set; }

		public int MainProductId { get; set; }
		public int PackageId { get; set; }
		public string PackageMRP { get; set; }
		public string PackageDiscount { get; set; }
		public string PackageDiscountedPrice { get; set; }
	}
}
