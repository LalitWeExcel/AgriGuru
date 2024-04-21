using System.Collections.Generic;

namespace SwarajCustomer_Common.ViewModel
{
    public class MastersViewModel
    {
        public IList<M_MasterModel> MastersList { get; set; }
        public int TotalRecords { get; set; }
        public int NoOfRecords { get; set; }
    }

    public class M_MasterModel
    {
        public int SrNo { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int combos_packages_category_id { get; set; }
        public int combos_packages_main_product_id { get; set; }

        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        
        public string Code { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public int Duration { get; set; }
        public int SamagriId { get; set; }

        public decimal A_LocationWisePrice { get; set; }
        public decimal B_LocationWisePrice { get; set; }
        public decimal C_LocationWisePrice { get; set; }
        
    }

    public class M_CombosPackagesModel
    {
        public int Id { get; set; }
        public int combos_packages_category_id { get; set; }
        public int combos_packages_main_product_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public int Duration { get; set; }

    }

    public class DropDownObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class kendoUiMultiple
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public class M_ResponceResult
    {
        public int Result { get; set; }
        public int Id { get; set; }
    }

    public class MasterDropDown
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MRP { get; set; }
        public int Duration { get; set; }
    }

    public class M_Responce
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public string Template { get; set; }
    }

    public class BulkUpload
    {
        public int CategoryID { get; set; }

        public int DealerMappingListCount { get; set; }
        public int DealerMappingServiceListCount { get; set; }
    }

    public class BulkUpload_Validation
    {
        public string RowNo { get; set; }
        public string ErrorMessage { get; set; }
    }
}
