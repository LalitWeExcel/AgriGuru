using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface.DashBoard
{
    public interface IDashBoardDAL
    {
        DashBoardViewModel GetDashBoardUserCount(string start_date, string end_date);
        List<UpcomingBirthdaysList> GetComeingBirthday();
        List<DropDownObject> GetDistrict(int ids);
        List<DropDownObject> GetState();
    }
}
