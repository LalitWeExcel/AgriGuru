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
    
    public partial class mst_zodiacs
    {
        public int mst_zodiacs_id { get; set; }
        public string name { get; set; }
        public string date_range { get; set; }
        public string image_path { get; set; }
        public string is_active { get; set; }
        public Nullable<int> c_by { get; set; }
        public Nullable<System.DateTime> c_time { get; set; }
        public Nullable<int> u_by { get; set; }
        public Nullable<System.DateTime> u_time { get; set; }
        public Nullable<int> sort_order { get; set; }
        public string description { get; set; }
    }
}
