using System.Collections.Generic;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_BAL.Interface.Master
{
    public class MastersBAL : IMastersBAL
    {
        private UOW unitOfWork = new UOW();

        public int Update(M_MasterModel model, int AdminUserId)
        {
            return unitOfWork.MasterRepository.Update(model, AdminUserId);
        }

        public List<DropDownObject> GetCategory()
        {
            return unitOfWork.MasterRepository.GetCategory();
        }

        public IList<M_MasterModel> GetMastersExcel(int Ids, out int recordsCount)
        {
            return unitOfWork.MasterRepository.GetMastersExcel(Ids, out recordsCount);
        }

        public IList<M_MasterModel> GetMastersList(int page, int pageSize, int categoryId, out int recordsCount)
        {
            return unitOfWork.MasterRepository.GetMastersList(page, pageSize, categoryId,out recordsCount);
        }

        public M_ResponceResult Save(M_MasterModel model, int adminUserId)
        {
            return unitOfWork.MasterRepository.Save(model, adminUserId);
        }

        public List<BulkUpload_Validation> SaveServicesBulkUpload(string FileContent, int ID, int UserId)
        {
            return unitOfWork.MasterRepository.SaveServicesBulkUpload(FileContent, ID, UserId);
        }

        public M_ResponceResult SaveUpdateCombosPackages(M_CombosPackagesModel model, int adminUserId)
        {
            return unitOfWork.MasterRepository.SaveUpdateCombosPackages(model, adminUserId);
        }
    }
}
