using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Common;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace SwarajCustomer_DAL
{
    public class LoginDAL : ILoginDAL
    {
		readonly Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public LoginDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public string Authenticate(string _userName, string password)
        {
            string IsSuccess = "false";

            adm_user UserDetails = null;
            try
            {
                if (_userName.Contains("@"))
                {
                    UserDetails = _databaseContext.adm_user.Where(id => id.email == _userName && id.is_active == "Y").OrderBy(x => x.c_time).FirstOrDefault();

                    if (UserDetails != null)
                    {
                        IsSuccess = "true";
                    }
                    else
                    {
                        IsSuccess = "false";
                    }
                }
                else
                {
                    UserDetails = _databaseContext.adm_user.Where(id => id.mob_number == _userName && id.is_active == "Y").OrderBy(x => x.c_time).FirstOrDefault();

                    if (UserDetails != null)
                    {
                        IsSuccess = "true";
                    }
                    else
                    {
                        IsSuccess = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = ex.Message.ToString();
            }
            return IsSuccess;
        }

        public bool UserExists(string _userName)
        {
            bool IsSuccess = false;

            adm_user UserDetails = null;
            try
            {
                if (_userName.Contains("@"))
                {
                    UserDetails = _databaseContext.adm_user.Where(id => id.email == _userName && id.is_active == "Y").OrderBy(x => x.c_time).FirstOrDefault();

                    if (UserDetails != null)
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
                else
                {
                    UserDetails = _databaseContext.adm_user.Where(id => id.mob_number == _userName && id.is_active == "Y").OrderBy(x => x.c_time).FirstOrDefault();

                    if (UserDetails != null)
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return IsSuccess;
        }

        public UserDetails Login(UserLoginEntity objUser, out string ExceptionLog)
        {
            ExceptionLog = "";
            string IsSuccess = "false";
            UserDetails objUserLoginDetails = new UserDetails();
            try
            {
                objUserLoginDetails.IsOTPSend = objUser.IsOTPSend;
                IsSuccess = GetuserLoginDetails(objUser, objUserLoginDetails, out ExceptionLog);
            }
            catch (Exception ex)
            {
                IsSuccess = ex.Message.ToString();
            }

            return objUserLoginDetails;
        }

        private string GetuserLoginDetails(UserLoginEntity objUser, UserDetails objUserLoginDetails, out string ExceptionLog)
        {
            ExceptionLog = "";
            bool IsOTPSend = objUserLoginDetails.IsOTPSend;
            string LoginRole = string.Empty;
            string IsSuccess = "false";

            try
            {
                adm_user UserDetails = _databaseContext.adm_user.Where(x => x.mob_number == objUser.Username
                                        && x.is_active == "Y" && x.user_type.ToUpper() == "CUST").FirstOrDefault();

                if (UserDetails == null)
                {
                    var username = objUser.Username.Substring(objUser.Username.Length - 3);
                    objUser.ReferalCode = username + RandomCodeGenrator.RandomCode();
                    objUser.Type = "CUST";
                    UserDetails = Register(objUser, out ExceptionLog);
                }

                var UserRoles = _databaseContext.adm_user_role.Where(id => id.adm_user_id == UserDetails.adm_user_id 
                                 && id.is_active == "Y").FirstOrDefault();

                if (UserRoles.adm_role_id == (int)Roles.CUST)
                {
                    objUserLoginDetails.UserTypeID = UserRoles.adm_role_id;
                    objUserLoginDetails.UserID = UserRoles.adm_user_id;
                    GetUserDetails(objUserLoginDetails, UserDetails);

                    IsSuccess = "true";
                    objUserLoginDetails.IsValidUser = IsSuccess;
                    objUserLoginDetails.IsSuccess = IsSuccess;

                    string OTP = GenerateRandom.GenerateRandomOTP();
                    objUserLoginDetails.OTP = OTP;
                    var retval = new ObjectParameter("retval", typeof(Int32));
                    int OtuPut = _databaseContext.usp_adm_user_ins_otp(UserDetails.adm_user_id, EncryptDecrypt.Encrypt(OTP), UserDetails.mob_number, retval);
                }
                else
                {
                    objUserLoginDetails.IsValidUser = IsSuccess;
                    objUserLoginDetails.IsSuccess = IsSuccess;
                }
                return IsSuccess;
            }
            catch (Exception ex)
            {
                objUserLoginDetails.IsValidUser = IsSuccess;
                objUserLoginDetails.IsSuccess = IsSuccess;
                return IsSuccess;
            }
        }

        private adm_user Register(UserLoginEntity objects, out string ExceptionLog)
        {
            ExceptionLog = "";
            var UserDetails = new adm_user();
            try
            {
                DbParam[] param = new DbParam[5];
                param[0] = new DbParam("@mob_number", objects.Username, SqlDbType.NVarChar);
                param[1] = new DbParam("@latitude", objects.Latitude, SqlDbType.NVarChar);
                param[2] = new DbParam("@longitude", objects.Longitude, SqlDbType.NVarChar);
                param[3] = new DbParam("@referal_code", objects.ReferalCode, SqlDbType.NVarChar);
                param[4] = new DbParam("@type", objects.Type, SqlDbType.NVarChar);
                Db.Insert("usp_register_user", param, false);

                UserDetails = _databaseContext.adm_user.Distinct().Where(x => x.mob_number == objects.Username
                || x.email == objects.Username && x.is_active == "Y").FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionLog = ex.Message;
                LogAPIException.ExceptionLog(ex, "User/Login");
            }
            finally
            {
                _databaseContext.Database.Connection.Close();
            }

            return UserDetails;
        }

        private void GetUserDetails(UserDetails objUserLoginDetails, adm_user UserDetails)
        {
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@adm_user_id", UserDetails.adm_user_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_adm_user_get", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        objUserLoginDetails.UserID = Db.ToInteger(row["adm_user_id"]);
                        objUserLoginDetails.Username = Db.ToString(row["username"]);
                        objUserLoginDetails.Mobile = Db.ToString(row["mob_number"]);
                        objUserLoginDetails.Email = Db.ToString(row["email"]);
                        objUserLoginDetails.UserType = Db.ToString(row["user_type"]);
                        objUserLoginDetails.Description = Db.ToString(row["description"]);
                        objUserLoginDetails.ReferalCode = Db.ToString(row["referal_code"]);

                        objUserLoginDetails.FirstName = Db.ToString(row["first_name"]);
                        objUserLoginDetails.LastName = Db.ToString(row["last_name"]);
                        objUserLoginDetails.WhatsappNo = Db.ToString(row["whatsapp_no"]);
                        objUserLoginDetails.Address = Db.ToString(row["address"]);
                        objUserLoginDetails.City = Db.ToString(row["city"]);
                        objUserLoginDetails.District = Db.ToString(row["district"]);
                        objUserLoginDetails.State = Db.ToString(row["state"]);
                        objUserLoginDetails.Rating = Db.ToInteger(row["Rating"]);
                        objUserLoginDetails.Experience = Db.ToInteger(row["Experience"]);
                        objUserLoginDetails.AstrologerExperience = Db.ToInteger(row["AstrologerExperience"]);
                        objUserLoginDetails.RoleId = Db.ToInteger(row["RoleId"]);
                        objUserLoginDetails.RoleName = Db.ToString(row["RoleName"]);

                        objUserLoginDetails.Latitude = Db.ToString(row["latitude"]);
                        objUserLoginDetails.Longitude = Db.ToString(row["longitude"]);
                    }
                }
            }
        }

        public string ResendOTP(string _userName, bool IsOTPSend)
        {
            UserDetails objUserLoginDetails = new UserDetails();
            string IsSuccess;
            bool OTPSend = IsOTPSend;

            adm_user UserDetails = null;
            UserDetails = _databaseContext.adm_user.Where(id => (id.mob_number == _userName || id.email == _userName) && id.is_active == "Y").OrderBy(x => x.c_time).FirstOrDefault();

            if (UserDetails != null)
            {
                var UserRoles = _databaseContext.adm_user_role.Where(id => id.adm_user_id == UserDetails.adm_user_id && id.is_active == "Y").FirstOrDefault();

                if (UserRoles.adm_role_id == (int)Roles.CUST)
                {
                    objUserLoginDetails.UserTypeID = UserRoles.adm_role_id;
                    objUserLoginDetails.UserID = UserRoles.adm_user_id;
                    GetUserDetails(objUserLoginDetails, UserDetails);
                }
                else
                {
                    return IsSuccess = "false";
                }
                IsSuccess = "true";

                string OTP = GenerateRandom.GenerateRandomOTP();
                if (OTPSend == true)
                {
                    if (!string.IsNullOrEmpty(UserDetails.mob_number))
                    {
                        //Send OTP
                        //Thread.Sleep(4000);
                        SMSUtility.SendSMS(UserDetails.mob_number, Extensions.OTP_SMS_CUST(OTP).ToString());

                    }
                    if (!string.IsNullOrEmpty(UserDetails.email))
                    {
                        //Send Email
                    }
                }
                else
                {
                    objUserLoginDetails.OTP = OTP;
                }

                objUserLoginDetails.IsValidUser = IsSuccess;
                objUserLoginDetails.OTP = OTP;
                ObjectParameter retval = new ObjectParameter("retval", typeof(Int32));
                int OtuPut = _databaseContext.usp_adm_user_ins_otp(UserDetails.adm_user_id, EncryptDecrypt.Encrypt(OTP), UserDetails.mob_number, retval);
            }
            else
            {
                IsSuccess = "false";
            }

            return (objUserLoginDetails.OTP);
        }

        public string SaveOTP(UserDetails objUser)
        {
            string result = "";
            var command = _databaseContext.Database.Connection.CreateCommand();
            command.CommandText = "usp_adm_user_ins_otp";
            command.CommandType = CommandType.StoredProcedure;

            // ab by lalit bcs missing @userId tag in sp
            var UserID_parameter = command.CreateParameter();
            UserID_parameter.ParameterName = "@userId";
            UserID_parameter.Value = objUser.UserID;
            command.Parameters.Add(UserID_parameter);

            var otp_parameter = command.CreateParameter();
            otp_parameter.ParameterName = "@OTP";
            otp_parameter.Value = objUser.OTP;
            command.Parameters.Add(otp_parameter);

            var mobile_parameter = command.CreateParameter();
            mobile_parameter.ParameterName = "@MobileNumber";
            mobile_parameter.Value = objUser.Mobile;
            command.Parameters.Add(mobile_parameter);

            try
            {
                _databaseContext.Database.Connection.Open();
                command.CommandTimeout = CommonMethods.CommandTimeoutExec;
                var reader = command.ExecuteReader();

                result = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)_databaseContext).ObjectContext.Translate<int>(reader).FirstOrDefault().ToString();
                reader.Close();
            }
            catch (Exception ex)
            {
                LogAPIException.ExceptionLog(ex, "Login/SaveOTP");
            }
            finally
            {
                _databaseContext.Database.Connection.Close();
            }

            return result;
        }

        public int InsertUpdateDeviceToken(DeviceToken obj)
        {
            var command = _databaseContext.Database.Connection.CreateCommand();
            command.CommandText = "[usp_insert_update_device_token]";
            command.CommandType = CommandType.StoredProcedure;

            var useridpeparameter = command.CreateParameter();
            useridpeparameter.ParameterName = "@user_id";
            useridpeparameter.Value = obj.UserId;
            command.Parameters.Add(useridpeparameter);

            var devicetypeparameter = command.CreateParameter();
            devicetypeparameter.ParameterName = "@device_type";
            devicetypeparameter.Value = obj.DeviceType;
            command.Parameters.Add(devicetypeparameter);

            var devicekeypeparameter = command.CreateParameter();
            devicekeypeparameter.ParameterName = "@device_key";
            devicekeypeparameter.Value = obj.DeviceKey;
            command.Parameters.Add(devicekeypeparameter);

            try
            {
                _databaseContext.Database.Connection.Open();
                command.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeoutExec"]);
                var reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _databaseContext.Database.Connection.Close();
            }
            return 1;
        }

        public UserDetails PurohitLogin(UserLoginEntity prohit, out string ExceptionLog)
        {
            ExceptionLog = "";
            var objProhitLoginDetails = new UserDetails();
            try
            {
                objProhitLoginDetails.IsOTPSend = prohit.IsOTPSend;
                objProhitLoginDetails.IsSuccess = GetProhitLoginDetails(prohit, objProhitLoginDetails, out ExceptionLog);
            }
            catch (Exception ex)
            {
                objProhitLoginDetails.IsSuccess = ex.Message.ToString();
            }

            return objProhitLoginDetails;
        }

        private string GetProhitLoginDetails(UserLoginEntity objUser, UserDetails objProhitLoginDetails, out string ExceptionLog)
        {
            ExceptionLog = "";
            string IsSuccess = "false";
            bool IsOTPSend = objProhitLoginDetails.IsOTPSend;

            string LoginRole = string.Empty;
            try
            {

                var command = _databaseContext.Database.Connection.CreateCommand();
                command.CommandText = "usp_adm_prohit_login";
                command.CommandType = CommandType.StoredProcedure;

                var mobile_parameter = command.CreateParameter();
                mobile_parameter.ParameterName = "@MobileNumber";
                mobile_parameter.Value = objUser.Username;
                command.Parameters.Add(mobile_parameter);

                _databaseContext.Database.Connection.Open();
                command.CommandTimeout = CommonMethods.CommandTimeoutExec;
                var reader = command.ExecuteReader();

                adm_user UserDetails = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)_databaseContext).ObjectContext.Translate<adm_user>(reader).FirstOrDefault();
                reader.Close();


                if (UserDetails == null)
                {
                    var username = objUser.Username.Substring(objUser.Username.Length - 3);
                    objUser.ReferalCode = username + RandomCodeGenrator.RandomCode();
                    objUser.Type = "PRHT";
                    UserDetails = Register(objUser, out ExceptionLog);
                }

                var UserRoles = _databaseContext.adm_user_role.Where(id => id.adm_user_id == UserDetails.adm_user_id && id.is_active == "Y").ToList();

                foreach (var item in UserRoles)
                {
                    objProhitLoginDetails.UserTypeID = item.adm_role_id;
                    objProhitLoginDetails.UserID = item.adm_user_id;
                    GetUserDetails(objProhitLoginDetails, UserDetails);

                    IsSuccess = "true";
                    objProhitLoginDetails.IsValidUser = IsSuccess;
                    objProhitLoginDetails.IsSuccess =   IsSuccess;

                    string OTP = GenerateRandom.GenerateRandomOTP();
                    objProhitLoginDetails.OTP = OTP;
                    var retval = new ObjectParameter("retval", typeof(Int32));
                    int OtuPut = _databaseContext.usp_adm_user_ins_otp(UserDetails.adm_user_id, EncryptDecrypt.Encrypt(OTP), UserDetails.mob_number, retval);
                    break;
                }

                objProhitLoginDetails.IsValidUser = IsSuccess;
                objProhitLoginDetails.IsSuccess = IsSuccess;
                return IsSuccess;

            }
            catch (Exception ex)
            {
                objProhitLoginDetails.IsValidUser = IsSuccess;
                objProhitLoginDetails.IsSuccess = IsSuccess;
                return IsSuccess;
            }
        }

        public LoginViewModel AdminLogin(string _emailId, string password)
        {
            DbParam[] param = new DbParam[2];
            LoginViewModel loggedInUser = new LoginViewModel();
            param[0] = new DbParam("@email", _emailId, SqlDbType.VarChar);
            param[1] = new DbParam("@password", password, SqlDbType.VarChar);
            DataSet dataSet = Db.GetDataSet("usp_admin_login_By_role", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {

                DataRow row = dataSet.Tables[0].Rows[0];
                loggedInUser.UserId = Db.ToInteger(row["UserId"]);
                loggedInUser.UserName = Db.ToString(row["UserName"]);
                loggedInUser.Email = Db.ToString(row["Email"]);
                loggedInUser.RoleId = Db.ToInteger(row["RoleId"]);
                loggedInUser.RoleName = Db.ToString(row["RoleName"]);
                loggedInUser.Password = Db.ToString(row["Password"]);
            }
            return loggedInUser;
        }

        public UserDataModel GetUserByEamilReferalCode(string user_email, string referal_code)
        {
            var profile = new UserDataModel();
            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@user_email", user_email, SqlDbType.NVarChar);
            param[1] = new DbParam("@referal_code", referal_code, SqlDbType.NVarChar);
            DataSet ds = Db.GetDataSet("usp_get_user_profile_email", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                profile.UserID = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                profile.Username = Db.ToString(ds.Tables[0].Rows[0]["Username"]);
                profile.MobileNumber = Db.ToString(ds.Tables[0].Rows[0]["MobileNumber"]);
                profile.Email = Db.ToString(ds.Tables[0].Rows[0]["Email"]);
                profile.ReferalCode = Db.ToString(ds.Tables[0].Rows[0]["ReferalCode"]);
            }

            return profile;
        }

        public string UpdatePassword(int user_id, string password, string referal_code)
        {
            var message = "Unable to update Password";
            var userData = _databaseContext.adm_user.Where(id => id.adm_user_id == user_id && id.is_active == "Y").FirstOrDefault();
            if (userData != null)
            {
                userData.password = password;
                _databaseContext.Entry(userData).State = EntityState.Modified;
                _databaseContext.SaveChanges();
                message = "Password updated successfully";
            }

            return message;
        }

        public string LogOut(LogOffEntity obj)
        {
            string result = string.Empty;

            DbParam[] param = new DbParam[2];
            LoginViewModel loggedInUser = new LoginViewModel();
            param[0] = new DbParam("@user_id", obj.UserId, SqlDbType.Int);
            param[1] = new DbParam("@device_key", obj.DeviceToken, SqlDbType.NVarChar);
            DataSet dataSet = Db.GetDataSet("usp_delete_device_token", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }

        public CustomerLoginModel CustomerLogin(string mobileNumber)
        {
            var result = new CustomerLoginModel();
            try
            {
                var _objects = _databaseContext.adm_user.Where(x => x.mob_number == mobileNumber
                                && x.is_active == "Y"
                                && x.user_type.ToUpper() == "CUST").FirstOrDefault();


                if(_objects != null)
                {
                    var UserRoles = (from p in _databaseContext.adm_user_role
                                     join role in _databaseContext.adm_role
                                     on p.adm_role_id equals role.adm_role_id
                                     where p.is_active == "Y" & role.is_active == "Y"  & p.adm_user_id == _objects.adm_user_id
                                     select new CustomerLoginModel
                                     {
                                         RoleId = role.adm_role_id,
                                         RoleName = role.code,
                                     }).FirstOrDefault();

                    string OTP = GenerateRandom.GenerateRandomOTP();
                    result.UserId = _objects.adm_user_id;
                    result.MobileNumber = _objects.mob_number;
                    if (!string.IsNullOrEmpty(_objects.logo))
                        result.ImageName = CommonMethods.GetCustomerImageUrl(_objects.logo);
                    else
                        result.ImageName = CommonMethods.CustomerIcon;

                    result.OTP = OTP;
                    result.Email = _objects.username;
                    result.RoleId = Convert.ToInt32(UserRoles.RoleId);
                    result.RoleName = UserRoles.RoleName;
                    result.IsSuccess = true;

                    var retval = new ObjectParameter("retval", typeof(Int32));

                    int OtuPut = _databaseContext.usp_adm_user_ins_otp(_objects.adm_user_id, EncryptDecrypt.Encrypt(OTP), _objects.mob_number, retval);
                }
                else
                {
                    result.LogsException = "Please enter valid Mobile Number";
                    result.IsSuccess = false;
                }
            }
            catch(Exception ex)
            {
                result.LogsException = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }
    }
}
