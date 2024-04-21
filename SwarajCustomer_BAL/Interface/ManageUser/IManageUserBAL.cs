using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.ManageUser
{
    public interface IManageUserBAL
    {
        IList<M_ManageUser> GetManageUserList(int page, int pageSize, string search, int role_id, out int recordsCount);
        IList<M_ManageUserDetails> GetManageUserExcel(string search, int role_id, out int recordsCount);
        int ActivateDeactivate(string Id, string status, int AdminUserId);
        M_ManageUserDetails GetManageUserDetails(int user_id);
        IList<CommonEntity> GetRoleMaster();
        IList<CommonEntity> GetAllUser();
        string Update(M_ManageUserDetails model, int adminUserId);
        IList<M_ManageUser> GetPurohitCustomerLinkingList(int page, int pageSize, string search, int user_id, out int recordsCount, out int p, out int a, out int c);
        IList<M_HourlyTime> GetHourlyTimeList();
     
    }
}
