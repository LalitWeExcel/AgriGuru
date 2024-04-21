using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface
{
    public interface IUserBAL
    {
        //List<Advertisement> GetAds(double latitude, double longitude);
        //List<Advertisement> GetEPujaAds(double latitude, double longitude);
        List<Advertisement> GetDashBoardAdvertisements(double latitude, double longitude,string type);

        List<UserDetails> GetTopAstrologersPurohits(double latitude, double longitude);
        Masters GetMasters(int userId, MasterRequest request);
        List<TimeSlotMaster> GetTimeSlots();
        List<UserDetails> GetPurohitAstroProfileDetail(UserLoginEntity userObj);
        Userprofile GetUserProfile(int user_id);
      
        string InsertCustomer(Userprofile model,int user_Id);

        string UpdateUserProfile(Userprofile _object);
        string UpdateProhitProfile(PurohitRegisterEntity _object);
        PurohitEntity GetProhitProfile(int prohit_id);

        List<OtpList> GetOTPList();

        string PurohitRegister(PurohitRegisterEntity purohit);



    }
}
