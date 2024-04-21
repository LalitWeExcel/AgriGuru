using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_Common.Customer
{
	public class BookAstrologerContent
	{
		public Masters Masters { get; set; }
		public Userprofile Userprofile { get; set; }
		public List<UserDetails> GetTopAstrologersPurohits { get; set; }
		public int AstrologerId { get; set; }
		public int ServicesId { get; set; }
	}
}
