using System.Collections.Generic;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_BAL.Interface.DashBoard
{
    public  class DashBoardBAL : IDashBoardBAL
    {
        private UOW unitOfWork = new UOW();

        public List<UpcomingBirthdaysList> GetComeingBirthday()
        {
            return unitOfWork.DashBoardRepository.GetComeingBirthday();
        }

        public DashBoardViewModel GetDashBoardUserCount(string start_date, string end_date)
        {
            return unitOfWork.DashBoardRepository.GetDashBoardUserCount(start_date, end_date);
        }

        public List<DropDownObject> GetDistrict(int ids)
        {
            return unitOfWork.DashBoardRepository.GetDistrict(ids);
        }

        public List<DropDownObject> GetState()
        {
            return unitOfWork.DashBoardRepository.GetState();
        }
    }
}
