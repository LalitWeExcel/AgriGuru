using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.Common;
using System;

namespace SwarajCustomer_BAL
{
    public class CommonDBMethods
    {
        static ILoginBAL _objLogin;

        public static UserLoginEntity GetUserAuthObject(string username, string role)
        {
            UserLoginEntity objUserLoginEntity = null;
            UserLoginEntity objUserLogin = ExceptionLogging.getUserAuthInfo(username.ToLower(), role);
            if (objUserLogin != null)
            {
                objUserLoginEntity = new UserLoginEntity
                {
                    UserID = objUserLogin.UserID,
                    UserRole = role,
                    Email = objUserLogin.Email,
                    Username = objUserLogin.Username
                };
            }

            return objUserLoginEntity;
        }

        public static void LoggingException(Exception ex, string Module)
        {
            SwarajCustomer_DAL.Common.LogAPIException.ExceptionLog(ex, Module);
        }
    }
}
