using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL
{
    public class UserBAL : IUserBAL
    {
        private UOW unitOfWork = new UOW();

        public List<Advertisement> GetDashBoardAdvertisements(double latitude, double longitude,string type)
        {
            return unitOfWork.UserDALRepository.GetDashBoardAdvertisements(latitude, longitude,type);
        }

        public List<UserDetails> GetTopAstrologersPurohits(double latitude, double longitude)
        {
            return unitOfWork.UserDALRepository.GetTopAstrologersPurohits(latitude, longitude);
        }

        public Masters GetMasters(int userId, MasterRequest request)
        {
            return unitOfWork.UserDALRepository.GetMasters(userId, request);
        }

        public List<TimeSlotMaster> GetTimeSlots()
        {
            return unitOfWork.UserDALRepository.GetTimeSlots();
        }
   
        public List<UserDetails> GetPurohitAstroProfileDetail(UserLoginEntity userObj)
        {
            return unitOfWork.UserDALRepository.GetPurohitAstroProfileDetail(userObj);
        }
        public Userprofile GetUserProfile(int user_id)
        {
            return unitOfWork.UserDALRepository.GetUserProfile(user_id);
        }
 
        public string InsertCustomer(Userprofile model,int user_Id)
        {
            return unitOfWork.UserDALRepository.InsertCustomer(model, user_Id);
        }


        public string UpdateUserProfile(Userprofile _object)
        {
            return unitOfWork.UserDALRepository.UpdateUserProfile(_object);
        }

        public string UpdateProhitProfile(PurohitRegisterEntity _object)
        {
            return unitOfWork.UserDALRepository.UpdateProhitProfile(_object);
        }
        public PurohitEntity GetProhitProfile(int  user_id)
        {
            return unitOfWork.UserDALRepository.GetProhitProfile(user_id);
        }

        public List<OtpList> GetOTPList()
        {
            return unitOfWork.UserDALRepository.GetOTPList();
        }

        public string PurohitRegister(PurohitRegisterEntity purohit)
        {
            return unitOfWork.UserDALRepository.PurohitRegister(purohit);
        }
    }
}
