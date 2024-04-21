using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL
{
    public class LoginBAL : ILoginBAL
    {
        private UOW unitOfWork = new UOW();

        public string Authenticate(string _userName, string password)
        {
            return unitOfWork.LoginDALRepository.Authenticate(_userName, password);
        }

        public bool UserExists(string _userName)
        {
            return unitOfWork.LoginDALRepository.UserExists(_userName);
        }

        public UserDetails Login(UserLoginEntity objUser, out  string ExceptionLog)
        {
            return unitOfWork.LoginDALRepository.Login(objUser,out ExceptionLog);
        }
        public UserDataModel GetUserByEamilReferalCode(string email_id, string referal_code)
        {
            return unitOfWork.LoginDALRepository.GetUserByEamilReferalCode(email_id, referal_code);
        }
        public string UpdatePassword(int user_id, string password, string referal_code)
        {
            return unitOfWork.LoginDALRepository.UpdatePassword(user_id, password, referal_code);
        }
    
        public UserDetails PurohitLogin(UserLoginEntity prohit, out string ExceptionLog)
        {
            return unitOfWork.LoginDALRepository.PurohitLogin(prohit,out ExceptionLog);
        }

        public string ResendOTP(string _userName, bool IsOTPSend)
        {
            return unitOfWork.LoginDALRepository.ResendOTP(_userName, IsOTPSend);
        }

        public string SaveOTP(UserDetails objUser)
        {
            return unitOfWork.LoginDALRepository.SaveOTP(objUser);
        }

        public int InsertUpdateDeviceToken(DeviceToken obj)
        {
            return unitOfWork.LoginDALRepository.InsertUpdateDeviceToken(obj);
        }
        public string LogOut(LogOffEntity obj)
        {
            return unitOfWork.LoginDALRepository.LogOut(obj);
        }
       
        #region Login WEB

        public LoginViewModel AdminLogin(string emilId, string password)
        {
            return unitOfWork.LoginDALRepository.AdminLogin(emilId, password);
        }
        public CustomerLoginModel CustomerLogin(string mobile)
        {
            return unitOfWork.LoginDALRepository.CustomerLogin(mobile);
        }
        #endregion
    }
}
