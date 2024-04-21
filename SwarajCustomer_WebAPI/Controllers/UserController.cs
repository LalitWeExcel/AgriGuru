using Newtonsoft.Json;
using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserBAL objUser;
        UserModal.DataModal result = null;
        [HttpPost]
        [Route("Dashboard")]
        public UserModal.DataModal Dashboard(UserLoginEntity objUserLogin)
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();

                var dashboard = new DashboardContent();
                dashboard.DashBoardAdvertisements = objUser.GetDashBoardAdvertisements(objUserLogin.Latitude, objUserLogin.Longitude, objUserLogin.Type);

                dashboard.purohits_astrologers = objUser.GetTopAstrologersPurohits(objUserLogin.Latitude, objUserLogin.Longitude);

                result = new UserModal.DataModal()
                {
                    Data = dashboard,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }

            catch (Exception ex)
            {  
                //dash.localAds = objUser.GetAds(objUserLogin.Latitude, objUserLogin.Longitude);
                //dash.epujaAds = objUser.GetEPujaAds(objUserLogin.Latitude, objUserLogin.Longitude);
                log.Info("Dashoard/Dashboard  step 01  error  ", ex);
                return APIExceptionHandler.ExceptionLogging(ex, "User/Dashboard");
            }
        }

        [HttpPost]
        [Route("Masters")]
        public UserModal.DataModal GetMasters(MasterRequest request)
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;

                Masters masters = new Masters();
                masters = objUser.GetMasters(user_Id, request);

                result = new UserModal.DataModal()
                {
                    Data = masters,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "User/GetMasters");
            }
        }

        [HttpPost]
        [Route("GetPurohitAstroProfileDetail")]
        public UserModal.DataModal GetPurohitAstroProfileDetail(UserLoginEntity objUserLogin)
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();

                List<UserDetails> user = new List<UserDetails>();
                user = objUser.GetPurohitAstroProfileDetail(objUserLogin);

                result = new UserModal.DataModal()
                {
                    Data = user,
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "User/GetUserProfile");
            }
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public UserModal.DataModal GetUserProfile()
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = objUser.GetUserProfile(user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "User/GetUserProfile");
            }
        }


        [HttpPost]
        [Route("InsertCustomer")]
        public UserModal.DataModal InsertCustomer(Userprofile model)
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;
                model.ReferalCode = model.MobileNumber.Substring(model.MobileNumber.Length - 3) + RandomCodeGenrator.RandomCode();

                model.AadharCardNumber = GenerateRandom.GenerateRandomNumber(16);

                result = new UserModal.DataModal()
                {
                    Data = objUser.InsertCustomer(model, user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "User/InsertCustomer");
            }
        }


        [HttpPost]
        [Route("PurohitRegister")]
        public UserModal.DataModal PurohitRegister(PurohitRegisterEntity _object)
        {
            try
            {
                objUser = new UserBAL();
                _object.referal_code = _object.mobile_number.Substring(_object.mobile_number.Length - 3) + RandomCodeGenrator.RandomCode();

                var prin = Identity();
                _object.prohit_id  = prin.UserDetails.UserID;

                var PurohitRegister = objUser.PurohitRegister(_object);

                if (PurohitRegister.Contains("successfully"))
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Successful.ToString(),
                        Message = PurohitRegister,
                        Status = true,
                        IsUserActive = true,
                    };
                }
                else
                {
                    result = new UserModal.DataModal()
                    {
                        Data = null,
                        Response = CommonActions.Successful.ToString(),
                        Message = PurohitRegister,
                        Status = false,
                        IsUserActive = true,
                    };
                }

                return result;
            }

            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "Login/PurohitRegister");
            }
        }



        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<UserModal.DataModal> UpdateUserProfile()
        {
            try
            {
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;

                objUser = new UserBAL();
                var userprofile = new Userprofile();
                string attachments = string.Empty;

                string Url = Request.RequestUri.AbsoluteUri;
                var savepath = ConfigurationManager.AppSettings["CustomerSavePath"].ToString();
                var uploadPath = HostingEnvironment.MapPath("~/" + savepath);

                Directory.CreateDirectory(uploadPath);
                var provider = new MultipartFormDataStreamProvider(uploadPath);
                await Request.Content.ReadAsMultipartAsync(provider);



                int i = 1;
                foreach (MultipartFileData file in provider.FileData)
                {
                    string fileName = file.Headers.ContentDisposition.FileName.Replace('"', ' ');
                    fileName = fileName.Replace('/', ' ').Trim();
                    string Extension = Path.GetExtension(fileName);
                    string newfilename = DateTime.Now.Ticks.ToString() + (i++).ToString() + Extension;
                    string path = Path.Combine(uploadPath, newfilename);
                    string fileObj = Path.GetFullPath(file.LocalFileName);
                    File.Move(file.LocalFileName, path);
                    attachments = attachments + newfilename;

                    //save image name here
                    userprofile.ImageName = attachments;
                }

                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        int a = 0;
                        if (key.ToUpper() == "UserID".ToUpper())
                        {
                            userprofile.UserID = Convert.ToInt32(val);// int.TryParse(val, out a) ? Convert.ToInt32(val) : 0;
                        }
                        if (key.ToUpper() == "Username".ToUpper())
                        {
                            userprofile.Username = val;
                        }
                        if (key.ToUpper() == "MobileNumber".ToUpper())
                        {
                            userprofile.MobileNumber = val;
                        }
                        if (key.ToUpper() == "Email".ToUpper())
                        {
                            userprofile.Email = val;
                        }
                        if (key.ToUpper() == "FirstName".ToUpper())
                        {
                            userprofile.FirstName = val;
                        }
                        if (key.ToUpper() == "LastName".ToUpper())
                        {
                            userprofile.LastName = val;
                        }

                        if (key.ToUpper() == "StateId".ToUpper())
                        {
                            userprofile.StateId = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "DistrictId".ToUpper())
                        {
                            userprofile.DistrictId = Convert.ToInt32(val);
                        }

                        if (key.ToUpper() == "Address".ToUpper())
                        {
                            userprofile.Address = val;
                        }

                        if (key.ToUpper() == "City".ToUpper())
                        {
                            userprofile.City = val;
                        }
                        if (key.ToUpper() == "DOB".ToUpper())
                        {
                            userprofile.DOB = val;
                        }
                        if (key.ToUpper() == "TOB".ToUpper())
                        {
                            userprofile.TOB = val;
                        }

                        if (key.ToUpper() == "Landmark".ToUpper())
                        {
                            userprofile.Landmark = val;
                        }
                        if (key.ToUpper() == "PinCode".ToUpper())
                        {
                            userprofile.PinCode = val;
                        }
                        if (key.ToUpper() == "Gender".ToUpper())
                        {
                            userprofile.Gender = val;
                        }
                        if (key.ToUpper() == "Profession".ToUpper())
                        {
                            userprofile.Profession = val;
                        }

                    }
                }

                result = new UserModal.DataModal()
                {
                    Data = objUser.UpdateUserProfile(userprofile),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;

            }
            catch (Exception ex)
            {
                return new UserModal.DataModal()
                {
                    Data = ex.Message,
                    Response = CommonActions.Failed.ToString(),
                    Message = CommonActions.Failed.ToString(),
                    Status = false,
                };
            }
        }

        [HttpPost]
        [Route("UpdateProhitProfile")]
        public async Task<UserModal.DataModal> UpdateProhitProfile()
        {
            try
            {
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;

                objUser = new UserBAL();
                var prohitprofile = new PurohitRegisterEntity();
                string attachments = string.Empty;

                string Url = Request.RequestUri.AbsoluteUri;
                var savepath = ConfigurationManager.AppSettings["CustomerSavePath"].ToString();
                var uploadPath = HostingEnvironment.MapPath("~/" + savepath);

                Directory.CreateDirectory(uploadPath);
                var provider = new MultipartFormDataStreamProvider(uploadPath);
                await Request.Content.ReadAsMultipartAsync(provider);



                int i = 1;
                foreach (MultipartFileData file in provider.FileData)
                {
                    string fileName = file.Headers.ContentDisposition.FileName.Replace('"', ' ');
                    fileName = fileName.Replace('/', ' ').Trim();
                    string Extension = Path.GetExtension(fileName);
                    string newfilename = DateTime.Now.Ticks.ToString() + (i++).ToString() + Extension;
                    string path = Path.Combine(uploadPath, newfilename);
                    string fileObj = Path.GetFullPath(file.LocalFileName);
                    System.IO.File.Move(file.LocalFileName, path);
                    //save image name here
                    prohitprofile.ImageName = attachments + newfilename;

                }

                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key.ToUpper() == "prohit_id".ToUpper())
                        {
                            prohitprofile.prohit_id = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "purohit_name".ToUpper())
                        {
                            prohitprofile.purohit_name = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "mobile_number".ToUpper())
                        {
                            prohitprofile.mobile_number = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "alternate_number".ToUpper())
                        {
                            prohitprofile.alternate_number = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "address".ToUpper())
                        {
                            prohitprofile.address = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "stateId".ToUpper())
                        {
                            prohitprofile.stateId = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "districtId".ToUpper())
                        {
                            prohitprofile.districtId = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "landmark".ToUpper())
                        {
                            prohitprofile.landmark = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "pin_code".ToUpper())
                        {
                            prohitprofile.pin_code = Convert.ToInt32(val);
                        }

                        if (key.ToUpper() == "purohit_experience".ToUpper())
                        {
                            prohitprofile.purohit_experience = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "astro_experience".ToUpper())
                        {
                            prohitprofile.astro_experience = Convert.ToInt32(val);
                        }
                        if (key.ToUpper() == "is_prohit".ToUpper())
                        {
                            prohitprofile.is_prohit = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "is_astro".ToUpper())
                        {
                            prohitprofile.is_astro = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "temple_name".ToUpper())
                        {
                            prohitprofile.temple_name = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "latitude".ToUpper())
                        {
                            prohitprofile.latitude = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "longitude".ToUpper())
                        {
                            prohitprofile.longitude = Convert.ToString(val);
                        }
                        if (key.ToUpper() == "PurohitPujaPath".ToUpper())
                        {
                            prohitprofile.PurohitPujaPath = JsonConvert.DeserializeObject<List<PurohitPujaPath>>(val);
                        }
                        if (key.ToUpper() == "AstrologerServices".ToUpper())
                        {
                            prohitprofile.AstrologerServices = JsonConvert.DeserializeObject<List<AstrologerServices>>(val);
                        }
                    }
                }

                result = new UserModal.DataModal()
                {
                    Data = objUser.UpdateProhitProfile(prohitprofile),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };
                return result;

            }
            catch (Exception ex)
            {
                return new UserModal.DataModal()
                {
                    Data = ex.Message,
                    Response = CommonActions.Failed.ToString(),
                    Message = CommonActions.Failed.ToString(),
                    Status = false,
                };
            }
        }



        [HttpGet]
        [Route("GetProhitProfile")]
        public UserModal.DataModal GetProhitProfile()
        {
            try
            {
                objUser = new UserBAL();
                var prin = Identity();
                int user_Id = prin.UserDetails.UserID;

                result = new UserModal.DataModal()
                {
                    Data = objUser.GetProhitProfile(user_Id),
                    Response = CommonActions.Successful.ToString(),
                    Message = CommonActions.Successful.ToString(),
                    Status = true,
                    IsUserActive = true,
                };

                return result;
            }
            catch (Exception ex)
            {
                return APIExceptionHandler.ExceptionLogging(ex, "User/GetProhitProfile");
            }
        }
        private static BasicAuthenticationAttribute.MyPrincipal Identity()
        {
            return System.Web.HttpContext.Current.User as BasicAuthenticationAttribute.MyPrincipal;
        }
    }
}