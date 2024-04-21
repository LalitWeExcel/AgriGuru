using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.DashBoard
{
    public interface IDashBoardBAL
    {
        DashBoardViewModel GetDashBoardUserCount(string start_date, string end_date);
        List<UpcomingBirthdaysList> GetComeingBirthday();
        List<DropDownObject> GetState();
        List<DropDownObject> GetDistrict(int ids);
    }
}
