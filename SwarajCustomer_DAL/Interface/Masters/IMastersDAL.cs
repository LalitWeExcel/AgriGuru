using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface.Master
{
    public  interface IMastersDAL
    {
        IList<M_MasterModel> GetMastersList(int page, int pageSize, int categoryId, out int recordsCount);
        IList<M_MasterModel> GetMastersExcel(int categoryId, out int recordsCount);
        int Update(M_MasterModel model, int AdminUserId);
        List<DropDownObject> GetCategory();
        M_ResponceResult Save(M_MasterModel model, int adminUserId);
        List<BulkUpload_Validation> SaveServicesBulkUpload(string FileContent, int ID, int UserId);
        M_ResponceResult SaveUpdateCombosPackages(M_CombosPackagesModel model, int adminUserId);
    }
}
