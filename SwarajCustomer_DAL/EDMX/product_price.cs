//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SwarajCustomer_DAL.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class product_price
    {
        public int product_price_id { get; set; }
        public Nullable<int> product_size_id { get; set; }
        public Nullable<decimal> mrp { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<decimal> final_amount { get; set; }
        public Nullable<int> sequence { get; set; }
        public string is_active { get; set; }
        public Nullable<int> c_by { get; set; }
        public Nullable<System.DateTime> c_time { get; set; }
        public Nullable<int> u_by { get; set; }
        public Nullable<System.DateTime> u_time { get; set; }
    }
}
