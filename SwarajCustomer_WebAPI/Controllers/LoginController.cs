using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.Common;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : ApiController
    {
        private ILoginBAL objLogin;
        UserModal.DataModal result = null;

        [HttpPost]
        [Route("LoginUser")]
        public UserModal.DataModal Login(UserLoginEntity _object)
        {
            objLogin = new LoginBAL();
            bool IsOTPSend = _object.IsOTPSend;
            var _token = new DeviceToken();

            try
            {
                string ExceptionLog;
                UserDetails LoginDetails = new UserDetails();
                LoginDetails = objLogin.Login(_object, out ExceptionLog);


                if (LoginDetails.IsSuccess == "true")
                {

                    if (IsOTPSend == true)
                    {
                        SMSUtility.SendSMS(LoginDetails.Mobile, Extensions.OTP_SMS_CUST(LoginDetails.OTP).ToString());
                    }
                    objLogin.SaveOTP(LoginDetails);

                    // save device token
                    _token.UserId = Convert.ToInt32(LoginDetails.UserID);
                    _token.DeviceType = Convert.ToInt32(_object.DeviceType);
                    _token.DeviceKey = _object.DeviceToken;
                    if (!string.IsNullOrEmpty(_token.DeviceKey))
                    {
                        objLogin.InsertUpdateDeviceToken(_token);
                    }

                    result = new UserModal.DataModal()
                    {
                        Data = LoginDetails,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (LoginDetails.IsSuccess == "false")
                {
                    if (ExceptionLog.Contains("UNIQUE KEY"))
                    {
                        result = new UserModal.DataModal()
                        {
                            Data = null,
                            Response = CommonActions.Unauthorized.ToString(),
                            Message = "'" + _object.Username + "' " + "already registered in prohit app !!",
                            Status = false,
                            IsUserActive = false,
                        };
                    }
                    else
                    {
                        result = new UserModal.DataModal()
                        {
                            Data = null,
                            Response = CommonActions.Failed.ToString(),
                            Message = "In-vaild user try again",
                            Status = false,
                            IsUserActive = false,
                        };
                    }
                }
                return result;
            }

            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Login/Login");
            }

        }

        [HttpPost]
        [Route("ResendOTP")]
        public UserModal.DataModal ResendOTP(UserLoginEntity objUser)
        {
            try
            {
                objLogin = new LoginBAL();
                string objOutput = objLogin.ResendOTP(objUser.Username, objUser.IsOTPSend);
                result = new UserModal.DataModal()
                {
                    Data = objOutput,
                    Response = objOutput == null ? CommonActions.Failed.ToString() : CommonActions.Successful.ToString(),
                    Message = objOutput == null ? CommonActions.Failed.ToString() : CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Login/ResendOTP");
            }
        }



        private bool IsUserExists(string UserName)
        {
            bool userExists = false;
            try
            {
                Exception ex = new Exception();
                APIExceptionHandler.ExceptionLogging(ex, "Login UserName - " + UserName + " - " + DateTime.Now.ToString());

                objLogin = new LoginBAL();
                userExists = objLogin.UserExists(UserName);

                return userExists;
            }
            catch (Exception ex)
            {
                // return ex;
                APIExceptionHandler.ExceptionLogging(ex, "Login UserName - " + UserName + " - " + DateTime.Now.ToString());
                return false;
            }
        }

        [HttpPost]
        [Route("PurohitLogin")]
        public UserModal.DataModal PurohitLogin(UserLoginEntity _object)
        {
            objLogin = new LoginBAL();
            bool IsOTPSend = _object.IsOTPSend;
            var _token = new DeviceToken();

            try
            {
                string ExceptionLog;
                var LoginDetails = objLogin.PurohitLogin(_object, out ExceptionLog);

                if (LoginDetails.IsSuccess == "true")
                {
                    if (IsOTPSend == true)
                    {
                        SMSUtility.SendSMS(LoginDetails.Mobile, Extensions.OTP_SMS_CUST(LoginDetails.OTP).ToString());
                    }
                    objLogin.SaveOTP(LoginDetails);

                    // save device token
                    _token.UserId = Convert.ToInt32(LoginDetails.UserID);
                    _token.DeviceType = Convert.ToInt32(_object.DeviceType);
                    _token.DeviceKey = _object.DeviceToken;
                    if (!string.IsNullOrEmpty(_token.DeviceKey))
                    {
                        objLogin.InsertUpdateDeviceToken(_token);
                    }

                    result = new UserModal.DataModal()
                    {
                        Data = LoginDetails,
                        Response = CommonActions.Successful.ToString(),
                        Message = CommonActions.Successful.ToString(),
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else if (LoginDetails.IsSuccess == "false")
                {
                    if (ExceptionLog.Contains("UNIQUE KEY"))
                    {
                        result = new UserModal.DataModal()
                        {
                            Data = null,
                            Response = CommonActions.Unauthorized.ToString(),
                            Message = "'" + _object.Username + "' " + "already registered in customer app !!",
                            Status = false,
                            IsUserActive = false,
                        };
                    }
                    else
                    {
                        result = new UserModal.DataModal()
                        {
                            Data = null,
                            Response = CommonActions.Failed.ToString(),
                            Message = "In-vaild user try again",
                            Status = false,
                            IsUserActive = false,
                        };
                    }
                }
                return result;
            }

            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Login/PurohitLogin");
            }

        }


        [HttpPost]
        [Route("LogOut")]
        public UserModal.DataModal LogOut(LogOffEntity _object)
        {
            try
            {
                var data = new LoginBAL().LogOut(_object);
                result = new UserModal.DataModal()
                {
                    Data = "",
                    Response = CommonActions.Successful.ToString(),
                    Message = data,
                    Status = true,
                    IsUserActive = true,
                };
                return result;
            }

            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Login/LogOut");
            }
        }

    }
}
