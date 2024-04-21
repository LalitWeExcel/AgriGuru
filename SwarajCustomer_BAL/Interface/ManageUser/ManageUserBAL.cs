using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.ManageUser
{
    public class ManageUserBAL : IManageUserBAL
    {
        private UOW unitOfWork = new UOW();

        public int ActivateDeactivate(string Id, string status, int AdminUserId)
        {
            return unitOfWork.ManageUserRepository.ActivateDeactivate(Id, status, AdminUserId);
        }

        public M_ManageUserDetails GetManageUserDetails(int user_id)
        {
            return unitOfWork.ManageUserRepository.GetManageUserDetails(user_id);
        }

        public IList<M_ManageUser> GetManageUserList(int page, int pageSize, string search, int role_id, out int recordsCount)
        {
            return unitOfWork.ManageUserRepository.GetManageUserList(page, pageSize, search, role_id, out recordsCount);
        }

        public IList<M_ManageUserDetails> GetManageUserExcel(string search, int role_id, out int recordsCount)
        {
            return unitOfWork.ManageUserRepository.GetManageUserExcel(search, role_id, out recordsCount);
        }

        public IList<CommonEntity> GetRoleMaster()
        {
            return unitOfWork.ManageUserRepository.GetRoleMaster();
        }
        public IList<CommonEntity> GetAllUser()
        {
            return unitOfWork.ManageUserRepository.GetAllUser();
        }
        public IList<M_HourlyTime> GetHourlyTimeList()
        {
            return unitOfWork.ManageUserRepository.GetHourlyTimeList();
        }

        public string Update(M_ManageUserDetails model, int adminUserId)
        {
            return unitOfWork.ManageUserRepository.Update(model, adminUserId);
        }

        public IList<M_ManageUser> GetPurohitCustomerLinkingList(int page, int pageSize, string search, int userid, out int recordsCount,
             out int totelProhits, out int totelAstrollgers, out int totelCustomers)
        {
            return unitOfWork.ManageUserRepository.GetPurohitCustomerLinkingList(page, pageSize, search, userid, out recordsCount,
                 out  totelProhits, out  totelAstrollgers, out  totelCustomers);
        }
    }
}
