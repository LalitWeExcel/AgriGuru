using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface
{
    public interface ILoginDAL
    {
        string Authenticate(string _userName, string password);

        bool UserExists(string _userName);

        UserDetails Login(UserLoginEntity objUser, out string ExceptionLog);
        UserDataModel GetUserByEamilReferalCode(string email_id, string referal_code);
        string UpdatePassword(int user_id, string password, string referal_code);
        string ResendOTP(string _userName, bool IsOTPSend);

        string SaveOTP(UserDetails objUser);
        int InsertUpdateDeviceToken(DeviceToken obj);
        string LogOut(LogOffEntity obj);
        UserDetails PurohitLogin(UserLoginEntity prohit, out string ExceptionLog);

        #region login web
        LoginViewModel AdminLogin(string emilId, string password);
        CustomerLoginModel CustomerLogin(string mobile);
        #endregion
    }
}
