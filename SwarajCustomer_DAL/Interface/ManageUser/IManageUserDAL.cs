using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface.ManageUser
{
    public  interface IManageUserDAL
    {
        int ActivateDeactivate(string Id, string status, int AdminUserId);
        IList<M_ManageUser> GetManageUserList(int page, int pageSize, string search, int role_id, out int recordsCount);
        IList<M_ManageUserDetails> GetManageUserExcel(string search, int role_id, out int recordsCount);
        M_ManageUserDetails GetManageUserDetails(int user_id);
        IList<CommonEntity> GetRoleMaster();
        IList<CommonEntity> GetAllUser();

        string Update(M_ManageUserDetails model, int adminUserId);
        IList<M_HourlyTime> GetHourlyTimeList();

        IList<M_ManageUser> GetPurohitCustomerLinkingList(int page, int pageSize, string search, int userid, out int recordsCount, out int p, out int a, out int c);
    }
}
