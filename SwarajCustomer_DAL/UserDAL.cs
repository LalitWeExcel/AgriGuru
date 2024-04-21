using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SwarajCustomer_DAL
{
    public class UserDAL : IUserDAL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SwarajTestEntities _databaseContext;

        public UserDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Advertisement> GetDashBoardAdvertisements(double latitude, double longitude, string type)
        {
            var _Advertisements = new List<Advertisement>();
            try
            {
                DbParam[] param = new DbParam[3];
                param[0] = new DbParam("@latitude", latitude.ToString(), SqlDbType.VarChar);
                param[1] = new DbParam("@longitude", longitude.ToString(), SqlDbType.VarChar);
                param[2] = new DbParam("@type", type, SqlDbType.NVarChar);

                DataSet ds = Db.GetDataSet("usp_get_dashboard_advertisements", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ad = new Advertisement();
                        ad.MuiltipleDistrictList = new List<MuiltipleDistrict>();
                        ad.SrNo = Db.ToInteger(row["SrNo"]);
                        ad.AdvertisementId = Db.ToInteger(row["AdvertisementId"]);
                        ad.CatagoryID = Db.ToInteger(row["category_id"]);
                        ad.ProductID = Db.ToInteger(row["main_product_id"]);
                        ad.CategoryName = Db.ToString(row["category_name"]);
                        ad.ActualAmount = Db.ToDecimal(row["mrp"]);
                        ad.Discount = Db.ToDecimal(row["discount"]);
                        ad.DiscountInRupees = Db.ToInteger(row["discount_in_rupees"]);
                        if (ad.DiscountInRupees > 0)
                        {     //  admin give  Discount In Rupees then minus ActualAmount  -  DiscountInRupees
                            var calucation = ad.ActualAmount - ad.DiscountInRupees;
                            ad.DiscountedAmount = calucation - (calucation * ad.Discount / 100);
                            ad.DiscountedAmount = decimal.Round(Convert.ToDecimal(ad.DiscountedAmount), 2);
                        }
                        else
                        {
                            ad.DiscountedAmount = ad.ActualAmount - (ad.ActualAmount * ad.Discount / 100);
                            ad.DiscountedAmount = decimal.Round(Convert.ToDecimal(ad.DiscountedAmount), 2);
                        }

                        ad.Service = Db.ToString(row["pooja_name"]);
                        ad.ServiceCode = Db.ToString(row["pooja_code"]);
                        ad.Details = ad.Discount + "% DISCOUNT ON " + ad.Service.ToUpper() + " BOOKING!";
                        ad.Description = Db.ToString(row["description"]);
                        ad.SamagriID = Db.ToInteger(row["samagri_id"]);
                        ad.Duration = Db.ToInteger(row["duration"]);
                        ad.DurationText = CommonMethods.GetDurationText(ad.Duration);
                        ad.IsGlobal = Db.ToString(row["is_global"]);
                        ad.AdvertisingType = Db.ToString(row["advertising_type"]);
                        ad.BookingDate = Db.ToString(row["BookingDate"]);
                        ad.BookingTime = Db.ToInteger(row["BookingTime"]);
                        ad.Url = Db.ToString(row["url"]);
                        ad.IsActive = Db.ToString(row["is_active"]);
                        ad.StateName = Db.ToString(row["StateName"]);
                        var image = Db.ToString(row["ImageName"]);
                        var video = Db.ToString(row["VideoName"]);


                        if (!string.IsNullOrEmpty(image))
                        {
                            ad.ImageName = CommonMethods.AdvertismentRetPath + image;
                        }
                        else ad.ImageName = CommonMethods.AdvertismentIcon;

                        if (!string.IsNullOrEmpty(video))
                        {
                            ad.VideoName = CommonMethods.AdvertismentVideosRetPath + video;
                        }
                        else ad.VideoName = CommonMethods.AdvertismentVideosIcon;

                        var _district = ds.Tables[1].Select("AdvertisementId = " + ad.AdvertisementId.ToString());
                        if (_district != null && _district.Any())
                        {
                            foreach (DataRow row2 in _district)
                            {
                                var m = new MuiltipleDistrict();
                                m.DistrictId = Db.ToInteger(row2["DistrictId"]);
                                m.DistrictName = Db.ToString(row2["DistrictName"]);
                                m.Latitude = Db.ToString(row2["Latitude"]);
                                m.Longitude = Db.ToString(row2["Longitude"]);
                                ad.MuiltipleDistrictList.Add(m);
                            }
                        }
                        _Advertisements.Add(ad);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("Dashoard/GetDashBoardAdvertisements  step 02 error  ", ex);
                LogsExceptio.ExceptionLog(ex, "step 03  User/GetDashBoardAdvertisements ");
            }

            return _Advertisements;
        }

        public List<UserDetails> GetTopAstrologersPurohits(double latitude, double longitude)
        {
            List<UserDetails> response = new List<UserDetails>();
            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@latitude", latitude.ToString(), SqlDbType.VarChar);
            param[1] = new DbParam("@longitude", longitude.ToString(), SqlDbType.VarChar);
            DataSet ds = Db.GetDataSet("usp_get_top_astrologers_purohits", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UserDetails user = new UserDetails();
                        user.Specializations = new List<Specialization>();
                        user.Services = new List<Service>();

                        user.UserID = Db.ToInteger(row["adm_user_id"]);
                        user.Username = Db.ToString(row["username"]);
                        user.Mobile = Db.ToString(row["mob_number"]);
                        user.Email = Db.ToString(row["email"]);
                        user.UserType = Db.ToString(row["user_type"]);
                        user.ReferalCode = Db.ToString(row["ReferalCode"]);
                        user.Badge = Db.ToString(row["badge"]);
                        user.MembershipType = Db.ToString(row["membership_name"]);
                        user.MembershipId = Db.ToInteger(row["premium_membership_id"]);
                        user.Description = Db.ToString(row["description"]);
                        user.ImageName = Db.ToString(row["ImageName"]);
                        if (!string.IsNullOrEmpty(user.ImageName))
                            user.ImageName = string.Format(CommonMethods.CustomerRetPath + user.ImageName);
                        else
                            user.ImageName = CommonMethods.CustomerIcon;
                        user.FirstName = Db.ToString(row["first_name"]);
                        user.LastName = Db.ToString(row["last_name"]);
                        user.WhatsappNo = Db.ToString(row["whatsapp_no"]);
                        user.Address = Db.ToString(row["address"]);
                        user.City = Db.ToString(row["city"]);
                        user.District = Db.ToString(row["district"]);
                        user.State = Db.ToString(row["state"]);
                        user.Rating = Db.ToInteger(row["rating"]);
                        user.Experience = Db.ToInteger(row["experience"]);
                        user.AstrologerExperience = Db.ToInteger(row["AstrologerExperience"]);
                        user.Latitude = Db.ToString(row["latitude"]);
                        user.Longitude = Db.ToString(row["longitude"]);


                        var specialization = ds.Tables[1].Select("adm_user_id = " + user.UserID.ToString());
                        if (specialization != null && specialization.Any())
                        {
                            foreach (DataRow row1 in specialization)
                            {
                                Specialization s = new Specialization();
                                s.UserID = Db.ToInteger(row1["adm_user_id"]);
                                s.code = Db.ToString(row1["code"]);
                                s.name = Db.ToString(row1["name"]);
                                user.Specializations.Add(s);
                            }
                        }

                        var service = ds.Tables[2].Select("ast_id = " + user.UserID.ToString());
                        if (service != null && service.Any())
                        {
                            foreach (DataRow row2 in service)
                            {
                                Service s = new Service();
                                s.AstID = Db.ToInteger(row2["ast_id"]);
                                s.ServiceID = Db.ToInteger(row2["service_id"]);
                                s.MRP = Convert.ToDecimal(Db.ToString(row2["price"]));
                                s.Discount = Convert.ToDecimal(Db.ToString(row2["discount"]));
                                s.DiscountedPrice = s.MRP - Math.Round(s.MRP * (s.Discount / 100));
                                user.Services.Add(s);
                            }
                        }
                        response.Add(user);
                    }
                }

            }

            return response;
        }

        public Masters GetMasters(int userId, MasterRequest request)
        {
            var _district = string.Empty;

            var response = new Masters();
            var pujaList = new List<PujaPath>();
            var pathList = new List<PujaPath>();
            var corpList = new List<PujaPath>();
            var servicePackageList = new List<PujaPath>();

            var astrologersList = new List<UserDetails>();
            var purohitsList = new List<UserDetails>();
            var zodiacsList = new List<ZodiacMaster>();
            var samagriList = new List<SamagriCategoryMaster>();
            var timeSlotsList = new List<TimeSlotMaster>();
            var consultationMediumList = new List<ConsultationMedium>();
            var servicesList = new List<ServiceMaster>();
            var durationSlotsList = new List<DurationSlotMaster>();
            var stateMasterList = new List<StateMaster>();
            var districtMasterList = new List<DistrictMaster>();

            var poojanSamagriList = new List<SamagriMaster>();
            var anyaSamagriList = new List<SamagriMaster>();
            var gharSamagriList = new List<SamagriMaster>();

            var homeVisitList = new List<ConsultationType>();
            var phoneCallList = new List<ConsultationType>();
            var videoCallList = new List<ConsultationType>();
            var PackageList = new List<Packages>();
            var ProhitDuration = new List<ProhitDuration>();
            var PricePercentage = new List<PricePercentage>();
            var PremiumMembership = new List<PremiumMembership>();
            var LanguageMaster = new List<LanguageMaster>();


            if (request.Latitude != 0 && request.Longitude != 0)
                _district = GoogleLocation.GetAddressByLatLong(request.Latitude, request.Longitude)
                    .address.state_district.Trim();


            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@userId", userId, SqlDbType.Int);
            param[1] = new DbParam("@districtName", _district, SqlDbType.NVarChar);
            DataSet ds = Db.GetDataSet("usp_get_masters", param);


            if (request.Latitude != 0 && request.Longitude != 0)
            {
                _LatitudeLongitudeMasters(response, pujaList, pathList, corpList, servicePackageList, astrologersList, purohitsList, zodiacsList, samagriList, timeSlotsList, consultationMediumList, servicesList, durationSlotsList, stateMasterList, districtMasterList, poojanSamagriList, anyaSamagriList, gharSamagriList, homeVisitList, phoneCallList, videoCallList, PackageList, ProhitDuration, PricePercentage, PremiumMembership, LanguageMaster, ds, request);
            }
            else
            {
                _Masters(response, pujaList, pathList, corpList, servicePackageList, astrologersList, purohitsList, zodiacsList, samagriList, timeSlotsList, consultationMediumList, servicesList, durationSlotsList, stateMasterList, districtMasterList, poojanSamagriList, anyaSamagriList, gharSamagriList, homeVisitList, phoneCallList, videoCallList, PackageList, ProhitDuration, PricePercentage, PremiumMembership, LanguageMaster, ds, request);
            }
            return response;
        }

        private static void _Masters(Masters response, List<PujaPath> pujaList, List<PujaPath> pathList, List<PujaPath> corpList,
            List<PujaPath> servicePackageList, List<UserDetails> astrologersList, List<UserDetails> purohitsList, List<ZodiacMaster> zodiacsList, List<SamagriCategoryMaster> samagriList, List<TimeSlotMaster> timeSlotsList, List<ConsultationMedium> consultationMediumList, List<ServiceMaster> servicesList, List<DurationSlotMaster> durationSlotsList, List<StateMaster> stateMasterList, List<DistrictMaster> districtMasterList, List<SamagriMaster> poojanSamagriList, List<SamagriMaster> anyaSamagriList, List<SamagriMaster> gharSamagriList, List<ConsultationType> homeVisitList, List<ConsultationType> phoneCallList, List<ConsultationType> videoCallList, List<Packages> PackageList, List<ProhitDuration> ProhitDuration, List<PricePercentage> PricePercentage, List<PremiumMembership> PremiumMembership, List<LanguageMaster> LanguageMaster, DataSet ds, MasterRequest request)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PujaPath puja = new PujaPath();
                        puja.ID = Db.ToInteger(row["main_product_id"]);
                        puja.CategoryID = Db.ToInteger(row["category_id"]);
                        puja.Category = Db.ToString(row["category"]);
                        puja.Code = Db.ToString(row["code"]);
                        puja.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            puja.Name = Db.ToString(row["hindi_name"]);

                        puja.Description = Db.ToString(row["description"]);
                        puja.MRP = Db.ToDecimal(row["mrp"]);
                        puja.Discount = Db.ToDecimal(row["discount"]);
                        puja.DiscountedPrice = puja.MRP - Math.Round(puja.MRP * (puja.Discount / 100));

                        puja.DiscountedPrice = decimal.Round(Convert.ToDecimal(puja.DiscountedPrice), 2);
                        puja.ImagePath = CommonMethods.PujaRetPath;//Db.ToString(row["image_path"]);
                        puja.SamagriID = Db.ToInteger(row["samagri_id"]);
                        puja.Duration = Db.ToInteger(row["duration"]);
                        puja.DurationText = CommonMethods.GetDurationText(puja.Duration);

                        if (puja.CategoryID == Convert.ToInt32(PujaCategory.PUJA))
                        {
                            pujaList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.PATH))
                        {
                            pathList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.CORP))
                        {
                            corpList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.COMBOSPACKAGESERVCIE))
                        {
                            servicePackageList.Add(puja);
                        }
                    }
                }
                response.PujaList = pujaList;
                response.PathList = pathList;
                response.CorporatePujaList = corpList;
                response.ServicePackageList = servicePackageList;

            }
            if (ds != null && ds.Tables.Count > 1)
            {
                string img = string.Empty;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        UserDetails user = new UserDetails();
                        user.UserID = Db.ToInteger(row["adm_user_id"]);
                        user.Username = Db.ToString(row["username"]);
                        user.Mobile = Db.ToString(row["mob_number"]);
                        user.Email = Db.ToString(row["email"]);
                        user.UserTypeID = Db.ToInteger(row["user_type_id"]);
                        user.UserType = Db.ToString(row["user_type"]);
                        user.Description = Db.ToString(row["description"]);
                        user.ImageName = Db.ToString(row["ImageName"]);
                        if (!string.IsNullOrEmpty(user.ImageName))
                            user.ImageName = string.Format(CommonMethods.CustomerRetPath + user.ImageName);
                        else
                            user.ImageName = CommonMethods.CustomerIcon;
                        user.FirstName = Db.ToString(row["first_name"]);
                        user.LastName = Db.ToString(row["last_name"]);
                        user.WhatsappNo = Db.ToString(row["whatsapp_no"]);
                        user.Address = Db.ToString(row["address"]);
                        user.City = Db.ToString(row["city"]);
                        user.District = Db.ToString(row["district"]);
                        user.State = Db.ToString(row["state"]);
                        user.Rating = Db.ToInteger(row["rating"]);
                        user.Experience = Db.ToInteger(row["experience"]);
                        user.AstrologerExperience = Db.ToInteger(row["AstrologerExperience"]);
                        user.Latitude = Db.ToString(row["latitude"]);
                        user.Longitude = Db.ToString(row["longitude"]);
                        if (user.UserTypeID == Convert.ToInt32(Roles.AST))
                        {
                            astrologersList.Add(user);
                        }
                        if (user.UserTypeID == Convert.ToInt32(Roles.PRHT))
                        {
                            purohitsList.Add(user);
                        }
                    }
                }
                response.AstrologerList = astrologersList;
                response.PurohitList = purohitsList;
            }
            if (ds != null && ds.Tables.Count > 2)
            {
                string img = string.Empty;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        ZodiacMaster zodiac = new ZodiacMaster();
                        zodiac.ID = Db.ToInteger(row["mst_zodiacs_id"]);
                        zodiac.Name = Db.ToString(row["name"]);
                        zodiac.DateRange = Db.ToString(row["date_range"]);
                        zodiac.ImagePath = Db.ToString(row["image_path"]);
                        zodiac.Description = Db.ToString(row["description"]);
                        zodiacsList.Add(zodiac);
                    }
                }
                response.HoroscopeList = zodiacsList;
            }
            if (ds != null && ds.Tables.Count > 3)
            {
                string img = string.Empty;
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        SamagriCategoryMaster category = new SamagriCategoryMaster();
                        category.ID = Db.ToInteger(row["samagri_category_id"]);
                        category.Name = Db.ToString(row["name"]);
                        category.MRP = Db.ToDecimal(row["mrp"]);
                        category.Discount = Db.ToDecimal(row["discount"]);
                        category.DiscountedPrice = category.MRP - Math.Round(category.MRP * (category.Discount / 100));
                        samagriList.Add(category);
                    }
                }
                response.SamagriList = samagriList;
            }
            if (ds != null && ds.Tables.Count > 4)
            {
                string img = string.Empty;
                if (ds.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        SamagriMaster samagri = new SamagriMaster();
                        samagri.ID = Db.ToInteger(row["samagri_product_id"]);
                        samagri.SamagriCategoryID = Db.ToInteger(row["samagri_category_id"]);
                        samagri.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            samagri.Name = Db.ToString(row["name_hindi"]);

                        samagri.HindiName = Db.ToString(row["name_hindi"]);
                        samagri.Code = Db.ToString(row["code"]);
                        samagri.Quantity = Db.ToString(row["quantity"]);
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.PUJA))
                        {
                            poojanSamagriList.Add(samagri);
                        }
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.ANYA))
                        {
                            anyaSamagriList.Add(samagri);
                        }
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.GHAR))
                        {
                            gharSamagriList.Add(samagri);
                        }
                    }
                }
                foreach (var item in samagriList)
                {
                    if (item.ID == Convert.ToInt32(SamagriCategory.PUJA))
                    {
                        item.SamagriList = poojanSamagriList;
                    }
                    if (item.ID == Convert.ToInt32(SamagriCategory.ANYA))
                    {
                        item.SamagriList = anyaSamagriList;
                    }
                    if (item.ID == Convert.ToInt32(SamagriCategory.GHAR))
                    {
                        item.SamagriList = gharSamagriList;
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 5)
            {
                string img = string.Empty;
                if (ds.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[5].Rows)
                    {
                        TimeSlotMaster time = new TimeSlotMaster();
                        time.ID = Db.ToInteger(row["time_slot_id"]);
                        time.TimeSlot = Db.ToString(row["name"]);
                        time.StartTime = Db.ToDateTime(row["start_time"]);
                        timeSlotsList.Add(time);
                    }
                }
                response.TimeSlotsList = timeSlotsList;
            }
            if (ds != null && ds.Tables.Count > 6)
            {
                string img = string.Empty;
                if (ds.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[6].Rows)
                    {
                        ConsultationMedium medium = new ConsultationMedium();
                        medium.ID = Db.ToInteger(row["consultation_medium_id"]);
                        medium.Code = Db.ToString(row["code"]);
                        medium.Name = Db.ToString(row["name"]);
                        consultationMediumList.Add(medium);
                    }
                }
                response.ConsultationMediumList = consultationMediumList;
            }
            if (ds != null && ds.Tables.Count > 7)
            {
                string img = string.Empty;
                if (ds.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[7].Rows)
                    {
                        ConsultationType type = new ConsultationType();
                        type.ID = Db.ToInteger(row["consultation_type_id"]);
                        type.MediumID = Db.ToInteger(row["consultation_medium_id"]);
                        type.Name = Db.ToString(row["name"]);
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.HVST))
                        {
                            homeVisitList.Add(type);
                        }
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.PCAL))
                        {
                            phoneCallList.Add(type);
                        }
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.VCAL))
                        {
                            videoCallList.Add(type);
                        }
                    }
                }
                foreach (var item in consultationMediumList)
                {
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.HVST))
                    {
                        item.ConsultationTypeList = homeVisitList;
                    }
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.PCAL))
                    {
                        item.ConsultationTypeList = phoneCallList;
                    }
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.VCAL))
                    {
                        item.ConsultationTypeList = videoCallList;
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 8)
            {
                string img = string.Empty;
                if (ds.Tables[8].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[8].Rows)
                    {
                        ServiceMaster srv = new ServiceMaster();
                        srv.ID = Db.ToInteger(row["mst_services_id"]);
                        srv.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            srv.Name = Db.ToString(row["hindi_name"]);

                        srv.Description = Db.ToString(row["description"]);
                        srv.FormValue = Db.ToString(row["form_value"]);
                        servicesList.Add(srv);
                    }
                }
                response.ServicesList = servicesList;
            }
            if (ds != null && ds.Tables.Count > 9)
            {
                string img = string.Empty;
                if (ds.Tables[9].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[9].Rows)
                    {
                        DurationSlotMaster duration = new DurationSlotMaster();
                        duration.ID = Db.ToInteger(row["duration_slot_id"]);
                        duration.Name = Db.ToString(row["name"]);
                        duration.CategoryID = Db.ToInteger(row["mst_category_id"]);
                        duration.Duration = Db.ToInteger(row["duration"]);
                        duration.DurationText = CommonMethods.GetDurationText(duration.Duration);
                        duration.MRP = Db.ToDecimal(row["mrp"]);
                        duration.Discount = Db.ToDecimal(row["discount"]);
                        duration.DiscountedPrice = duration.MRP - Math.Round(duration.MRP * (duration.Discount / 100));
                        durationSlotsList.Add(duration);
                    }
                }
                response.DurationSlotsList = durationSlotsList;
            }
            if (ds != null && ds.Tables.Count > 10)
            {
                string img = string.Empty;
                if (ds.Tables[10].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[10].Rows)
                    {
                        StateMaster state = new StateMaster();
                        state.ID = Db.ToInteger(row["mst_state_id"]);
                        state.Code = Db.ToString(row["code"]);
                        state.Description = Db.ToString(row["description"]);
                        state.Name = Db.ToString(row["name"]);
                        stateMasterList.Add(state);
                    }
                }
                response.StateMasterList = stateMasterList;
            }
            if (ds != null && ds.Tables.Count > 11)
            {
                string img = string.Empty;
                if (ds.Tables[11].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[11].Rows)
                    {
                        DistrictMaster district = new DistrictMaster();
                        district.ID = Db.ToInteger(row["mst_district_id"]);
                        district.Code = Db.ToString(row["code"]);
                        district.Description = Db.ToString(row["description"]);
                        district.Name = Db.ToString(row["name"]);
                        district.StateID = Db.ToInteger(row["mst_state_id"]);
                        district.LocationCategory = Db.ToString(row["location_category"]);

                        districtMasterList.Add(district);
                    }
                }
                response.DistrictMasterList = districtMasterList;
            }
            if (ds != null && ds.Tables.Count > 12 && ds.Tables[12].Rows.Count > 0)
            {
                response.ReferalCode = Convert.ToString(ds.Tables[12].Rows[0]["referal_code"]);
            }
            if (ds != null && ds.Tables.Count > 13)
            {
                if (ds.Tables[13].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[13].Rows)
                    {
                        Packages package = new Packages();
                        package.ID = Db.ToInteger(row["PackageId"]);
                        package.Name = Db.ToString(row["Name"]);
                        package.Code = Db.ToString(row["Code"]);
                        package.MRP = Db.ToDecimal(row["MRP"]);
                        package.Discount = Db.ToDecimal(row["Discount"]);
                        package.Description = Db.ToString(row["Description"]);
                        package.Note = Db.ToString(row["Note"]);
                        package.NoOfPandit = Db.ToInteger(row["NoOfPandit"]);
                        package.Duration = Db.ToInteger(row["Duration"]);
                        package.SamagriId = Db.ToInteger(row["SamagriId"]);
                        package.CategoryID = Db.ToInteger(row["mst_category_id"]);
                        package.DiscountedPrice = package.MRP - Math.Round(package.MRP * (package.Discount / 100));

                        var reasons = ds.Tables[14].Select("PackageId = " + package.ID.ToString());
                        if (reasons != null && reasons.Any())
                        {
                            foreach (DataRow row2 in reasons)
                            {
                                var reason = new Procedureinvolve();
                                reason.Name = Db.ToString(row2["name"]);
                                package.Procedureinvolve.Add(reason);
                            }
                        }

                        PackageList.Add(package);
                    }
                }
                response.PackageList = PackageList;
            }
            if (ds != null && ds.Tables.Count > 15)
            {
                if (ds.Tables[15].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[15].Rows)
                    {
                        ProhitDuration d = new ProhitDuration();
                        d.ID = Db.ToInteger(row["ID"]);
                        d.Name = Db.ToString(row["Name"]);
                        d.Duration = Db.ToInteger(row["Duration"]);
                        ProhitDuration.Add(d);
                    }
                }
                response.ProhitDuration = ProhitDuration;
            }
            if (ds != null && ds.Tables.Count > 16 && ds.Tables[16].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[16].Rows)
                {
                    var p = new PricePercentage();
                    p.ID = Db.ToInteger(row["ID"]);
                    p.LocationCategory = Db.ToString(row["location_category"]);
                    p.CategoryID = Db.ToInteger(row["CategoryID"]);
                    p.Percentage = Db.ToDecimal(row["Percentage"]);
                    PricePercentage.Add(p);
                }
                response.PricePercentage = PricePercentage;
            }
            if (ds != null && ds.Tables.Count > 17 && ds.Tables[17].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[17].Rows)
                {
                    var p = new PremiumMembership();
                    p.PremiumMembershipID = Db.ToInteger(row["PremiumMembershipID"]);
                    p.Name = Db.ToString(row["Name"]);
                    p.Code = Db.ToString(row["Code"]);
                    p.Description = Db.ToString(row["Description"]);
                    p.DurationInMonths = Db.ToInteger(row["DurationInMonths"]);
                    p.MRP = Db.ToDecimal(row["MRP"]);
                    p.Discount = Db.ToDecimal(row["Discount"]);
                    p.DiscountedPrice = p.MRP - Math.Round(p.MRP * (p.Discount / 100));

                    PremiumMembership.Add(p);
                }
                response.PremiumMembership = PremiumMembership;
            }
            if (ds != null && ds.Tables.Count > 18 && ds.Tables[18].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[18].Rows)
                {
                    var l = new LanguageMaster();
                    l.ID = Db.ToInteger(row["ID"]);
                    l.Name = Db.ToString(row["Name"]);
                    LanguageMaster.Add(l);
                }
                response.LanguageMaster = LanguageMaster;
            }
        }

        private static void _LatitudeLongitudeMasters(Masters response, List<PujaPath> pujaList, List<PujaPath> pathList, List<PujaPath> corpList, List<PujaPath> servicePackageList, List<UserDetails> astrologersList, List<UserDetails> purohitsList, List<ZodiacMaster> zodiacsList, List<SamagriCategoryMaster> samagriList, List<TimeSlotMaster> timeSlotsList, List<ConsultationMedium> consultationMediumList, List<ServiceMaster> servicesList, List<DurationSlotMaster> durationSlotsList, List<StateMaster> stateMasterList, List<DistrictMaster> districtMasterList, List<SamagriMaster> poojanSamagriList, List<SamagriMaster> anyaSamagriList, List<SamagriMaster> gharSamagriList, List<ConsultationType> homeVisitList, List<ConsultationType> phoneCallList, List<ConsultationType> videoCallList, List<Packages> PackageList, List<ProhitDuration> ProhitDuration, List<PricePercentage> PricePercentage, List<PremiumMembership> PremiumMembership, List<LanguageMaster> LanguageMaster, DataSet ds, MasterRequest request)
        {
            if (ds != null && ds.Tables.Count > 16 && ds.Tables[16].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[16].Rows)
                {
                    var p = new PricePercentage();
                    p.ID = Db.ToInteger(row["ID"]);
                    p.LocationCategory = Db.ToString(row["location_category"]);
                    p.CategoryID = Db.ToInteger(row["CategoryID"]);
                    p.Percentage = Db.ToDecimal(row["Percentage"]);
                    PricePercentage.Add(p);
                }
                response.PricePercentage = PricePercentage;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PujaPath puja = new PujaPath();
                        puja.ID = Db.ToInteger(row["main_product_id"]);
                        puja.CategoryID = Db.ToInteger(row["category_id"]);
                        puja.Category = Db.ToString(row["category"]);
                        puja.Code = Db.ToString(row["code"]);
                        puja.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            puja.Name = Db.ToString(row["hindi_name"]);

                        puja.Description = Db.ToString(row["description"]);
                        puja.MRP = Db.ToDecimal(row["mrp"]);
                        puja.Discount = Db.ToDecimal(row["discount"]);
                        //puja.DiscountedPrice = puja.MRP - Math.Round(puja.MRP * (puja.Discount / 100));

                        foreach (var pp in response.PricePercentage)
                        {
                            if (pp.CategoryID == puja.CategoryID)
                            {
                                var total = puja.MRP + Math.Round(puja.MRP * (pp.Percentage / 100));
                                puja.DiscountedPrice = total - Math.Round(total * (puja.Discount / 100));
                                puja.DiscountedPrice = decimal.Round(Convert.ToDecimal(puja.DiscountedPrice), 2);
                            }
                        }
                        puja.ImagePath = CommonMethods.PujaRetPath;//Db.ToString(row["image_path"]);
                        puja.SamagriID = Db.ToInteger(row["samagri_id"]);
                        puja.Duration = Db.ToInteger(row["duration"]);
                        puja.DurationText = CommonMethods.GetDurationText(puja.Duration);

                        if (puja.CategoryID == Convert.ToInt32(PujaCategory.PUJA))
                        {
                            pujaList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.PATH))
                        {
                            pathList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.CORP))
                        {
                            corpList.Add(puja);
                        }
                        else if (puja.CategoryID == Convert.ToInt32(PujaCategory.COMBOSPACKAGESERVCIE))
                        {
                            servicePackageList.Add(puja);
                        }
                    }
                }
                response.PujaList = pujaList;
                response.PathList = pathList;
                response.CorporatePujaList = corpList;
                response.ServicePackageList = servicePackageList;
            }
            if (ds != null && ds.Tables.Count > 1)
            {
                string img = string.Empty;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        UserDetails user = new UserDetails();
                        user.UserID = Db.ToInteger(row["adm_user_id"]);
                        user.Username = Db.ToString(row["username"]);
                        user.Mobile = Db.ToString(row["mob_number"]);
                        user.Email = Db.ToString(row["email"]);
                        user.UserTypeID = Db.ToInteger(row["user_type_id"]);
                        user.UserType = Db.ToString(row["user_type"]);
                        user.Description = Db.ToString(row["description"]);
                        user.ImageName = Db.ToString(row["ImageName"]);
                        if (!string.IsNullOrEmpty(user.ImageName))
                            user.ImageName = string.Format(CommonMethods.CustomerRetPath + user.ImageName);
                        else
                            user.ImageName = CommonMethods.CustomerIcon;
                        user.FirstName = Db.ToString(row["first_name"]);
                        user.LastName = Db.ToString(row["last_name"]);
                        user.WhatsappNo = Db.ToString(row["whatsapp_no"]);
                        user.Address = Db.ToString(row["address"]);
                        user.City = Db.ToString(row["city"]);
                        user.District = Db.ToString(row["district"]);
                        user.State = Db.ToString(row["state"]);
                        user.Rating = Db.ToInteger(row["rating"]);
                        user.Experience = Db.ToInteger(row["experience"]);
                        user.AstrologerExperience = Db.ToInteger(row["AstrologerExperience"]);
                        user.Latitude = Db.ToString(row["latitude"]);
                        user.Longitude = Db.ToString(row["longitude"]);
                        if (user.UserTypeID == Convert.ToInt32(Roles.AST))
                        {
                            astrologersList.Add(user);
                        }
                        if (user.UserTypeID == Convert.ToInt32(Roles.PRHT))
                        {
                            purohitsList.Add(user);
                        }
                    }
                }
                response.AstrologerList = astrologersList;
                response.PurohitList = purohitsList;
            }
            if (ds != null && ds.Tables.Count > 2)
            {
                string img = string.Empty;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        ZodiacMaster zodiac = new ZodiacMaster();
                        zodiac.ID = Db.ToInteger(row["mst_zodiacs_id"]);
                        zodiac.Name = Db.ToString(row["name"]);
                        zodiac.DateRange = Db.ToString(row["date_range"]);
                        zodiac.ImagePath = Db.ToString(row["image_path"]);
                        zodiac.Description = Db.ToString(row["description"]);
                        zodiacsList.Add(zodiac);
                    }
                }
                response.HoroscopeList = zodiacsList;
            }
            if (ds != null && ds.Tables.Count > 3)
            {
                string img = string.Empty;
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        SamagriCategoryMaster category = new SamagriCategoryMaster();
                        category.ID = Db.ToInteger(row["samagri_category_id"]);
                        category.Name = Db.ToString(row["name"]);
                        category.MRP = Db.ToDecimal(row["mrp"]);
                        category.Discount = Db.ToDecimal(row["discount"]);
                        category.DiscountedPrice = category.MRP - Math.Round(category.MRP * (category.Discount / 100));
                        samagriList.Add(category);
                    }
                }
                response.SamagriList = samagriList;
            }
            if (ds != null && ds.Tables.Count > 4)
            {
                string img = string.Empty;
                if (ds.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        SamagriMaster samagri = new SamagriMaster();
                        samagri.ID = Db.ToInteger(row["samagri_product_id"]);
                        samagri.SamagriCategoryID = Db.ToInteger(row["samagri_category_id"]);
                        samagri.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            samagri.Name = Db.ToString(row["name_hindi"]);

                        samagri.HindiName = Db.ToString(row["name_hindi"]);
                        samagri.Code = Db.ToString(row["code"]);
                        samagri.Quantity = Db.ToString(row["quantity"]);
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.PUJA))
                        {
                            poojanSamagriList.Add(samagri);
                        }
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.ANYA))
                        {
                            anyaSamagriList.Add(samagri);
                        }
                        if (samagri.SamagriCategoryID == Convert.ToInt32(SamagriCategory.GHAR))
                        {
                            gharSamagriList.Add(samagri);
                        }
                    }
                }
                foreach (var item in samagriList)
                {
                    if (item.ID == Convert.ToInt32(SamagriCategory.PUJA))
                    {
                        item.SamagriList = poojanSamagriList;
                    }
                    if (item.ID == Convert.ToInt32(SamagriCategory.ANYA))
                    {
                        item.SamagriList = anyaSamagriList;
                    }
                    if (item.ID == Convert.ToInt32(SamagriCategory.GHAR))
                    {
                        item.SamagriList = gharSamagriList;
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 5)
            {
                string img = string.Empty;
                if (ds.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[5].Rows)
                    {
                        TimeSlotMaster time = new TimeSlotMaster();
                        time.ID = Db.ToInteger(row["time_slot_id"]);
                        time.TimeSlot = Db.ToString(row["name"]);
                        time.StartTime = Db.ToDateTime(row["start_time"]);
                        timeSlotsList.Add(time);
                    }
                }
                response.TimeSlotsList = timeSlotsList;
            }
            if (ds != null && ds.Tables.Count > 6)
            {
                string img = string.Empty;
                if (ds.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[6].Rows)
                    {
                        ConsultationMedium medium = new ConsultationMedium();
                        medium.ID = Db.ToInteger(row["consultation_medium_id"]);
                        medium.Code = Db.ToString(row["code"]);
                        medium.Name = Db.ToString(row["name"]);
                        consultationMediumList.Add(medium);
                    }
                }
                response.ConsultationMediumList = consultationMediumList;
            }
            if (ds != null && ds.Tables.Count > 7)
            {
                string img = string.Empty;
                if (ds.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[7].Rows)
                    {
                        ConsultationType type = new ConsultationType();
                        type.ID = Db.ToInteger(row["consultation_type_id"]);
                        type.MediumID = Db.ToInteger(row["consultation_medium_id"]);
                        type.Name = Db.ToString(row["name"]);
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.HVST))
                        {
                            homeVisitList.Add(type);
                        }
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.PCAL))
                        {
                            phoneCallList.Add(type);
                        }
                        if (type.MediumID == Convert.ToInt32(ConsultationMediumEnum.VCAL))
                        {
                            videoCallList.Add(type);
                        }
                    }
                }
                foreach (var item in consultationMediumList)
                {
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.HVST))
                    {
                        item.ConsultationTypeList = homeVisitList;
                    }
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.PCAL))
                    {
                        item.ConsultationTypeList = phoneCallList;
                    }
                    if (item.ID == Convert.ToInt32(ConsultationMediumEnum.VCAL))
                    {
                        item.ConsultationTypeList = videoCallList;
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 8)
            {
                string img = string.Empty;
                if (ds.Tables[8].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[8].Rows)
                    {
                        ServiceMaster srv = new ServiceMaster();
                        srv.ID = Db.ToInteger(row["mst_services_id"]);
                        srv.Name = Db.ToString(row["name"]);

                        if (request.Language == "Hindi")
                            srv.Name = Db.ToString(row["hindi_name"]);

                        srv.Description = Db.ToString(row["description"]);
                        srv.FormValue = Db.ToString(row["form_value"]);
                        servicesList.Add(srv);
                    }
                }
                response.ServicesList = servicesList;
            }
            if (ds != null && ds.Tables.Count > 9)
            {
                string img = string.Empty;
                if (ds.Tables[9].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[9].Rows)
                    {
                        DurationSlotMaster duration = new DurationSlotMaster();
                        duration.ID = Db.ToInteger(row["duration_slot_id"]);
                        duration.Name = Db.ToString(row["name"]);
                        duration.CategoryID = Db.ToInteger(row["mst_category_id"]);
                        duration.Duration = Db.ToInteger(row["duration"]);
                        duration.DurationText = CommonMethods.GetDurationText(duration.Duration);
                        duration.MRP = Db.ToDecimal(row["mrp"]);
                        duration.Discount = Db.ToDecimal(row["discount"]);
                        //  duration.DiscountedPrice = duration.MRP - Math.Round(duration.MRP * (duration.Discount / 100));
                        foreach (var pp in response.PricePercentage)
                        {
                            if (pp.CategoryID == duration.CategoryID)
                            {
                                var total = duration.MRP + Math.Round(duration.MRP * (pp.Percentage / 100));
                                duration.DiscountedPrice = total - Math.Round(total * (duration.Discount / 100));
                            }
                        }

                        durationSlotsList.Add(duration);
                    }
                }
                response.DurationSlotsList = durationSlotsList;
            }
            if (ds != null && ds.Tables.Count > 10)
            {
                string img = string.Empty;
                if (ds.Tables[10].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[10].Rows)
                    {
                        StateMaster state = new StateMaster();
                        state.ID = Db.ToInteger(row["mst_state_id"]);
                        state.Code = Db.ToString(row["code"]);
                        state.Description = Db.ToString(row["description"]);
                        state.Name = Db.ToString(row["name"]);
                        stateMasterList.Add(state);
                    }
                }
                response.StateMasterList = stateMasterList;
            }
            if (ds != null && ds.Tables.Count > 11)
            {
                string img = string.Empty;
                if (ds.Tables[11].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[11].Rows)
                    {
                        DistrictMaster district = new DistrictMaster();
                        district.ID = Db.ToInteger(row["mst_district_id"]);
                        district.Code = Db.ToString(row["code"]);
                        district.Description = Db.ToString(row["description"]);
                        district.Name = Db.ToString(row["name"]);
                        district.StateID = Db.ToInteger(row["mst_state_id"]);
                        district.LocationCategory = Db.ToString(row["location_category"]);

                        districtMasterList.Add(district);
                    }
                }
                response.DistrictMasterList = districtMasterList;
            }
            if (ds != null && ds.Tables.Count > 12 && ds.Tables[12].Rows.Count > 0)
            {
                response.ReferalCode = Convert.ToString(ds.Tables[12].Rows[0]["referal_code"]);
            }
            if (ds != null && ds.Tables.Count > 13)
            {
                if (ds.Tables[13].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[13].Rows)
                    {
                        Packages package = new Packages();
                        package.ID = Db.ToInteger(row["PackageId"]);
                        package.Name = Db.ToString(row["Name"]);
                        package.Code = Db.ToString(row["Code"]);
                        package.MRP = Db.ToDecimal(row["MRP"]);
                        package.Discount = Db.ToDecimal(row["Discount"]);
                        package.Description = Db.ToString(row["Description"]);
                        package.Note = Db.ToString(row["Note"]);
                        package.NoOfPandit = Db.ToInteger(row["NoOfPandit"]);
                        package.Duration = Db.ToInteger(row["Duration"]);
                        package.SamagriId = Db.ToInteger(row["SamagriId"]);
                        package.CategoryID = Db.ToInteger(row["mst_category_id"]);
                        // package.DiscountedPrice = package.MRP - Math.Round(package.MRP * (package.Discount / 100));

                        foreach (var pp in response.PricePercentage)
                        {
                            if (pp.CategoryID == package.CategoryID)
                            {
                                var total = package.MRP + Math.Round(package.MRP * (pp.Percentage / 100));
                                package.DiscountedPrice = total - Math.Round(total * (package.Discount / 100));
                            }
                        }


                        var reasons = ds.Tables[14].Select("PackageId = " + package.ID.ToString());
                        if (reasons != null && reasons.Any())
                        {
                            foreach (DataRow row2 in reasons)
                            {
                                var reason = new Procedureinvolve();
                                reason.Name = Db.ToString(row2["name"]);
                                package.Procedureinvolve.Add(reason);
                            }
                        }

                        PackageList.Add(package);
                    }
                }
                response.PackageList = PackageList;
            }
            if (ds != null && ds.Tables.Count > 15)
            {
                if (ds.Tables[15].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[15].Rows)
                    {
                        ProhitDuration d = new ProhitDuration();
                        d.ID = Db.ToInteger(row["ID"]);
                        d.Name = Db.ToString(row["Name"]);
                        d.Duration = Db.ToInteger(row["Duration"]);
                        ProhitDuration.Add(d);
                    }
                }
                response.ProhitDuration = ProhitDuration;
            }
            if (ds != null && ds.Tables.Count > 17 && ds.Tables[17].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[17].Rows)
                {
                    var p = new PremiumMembership();
                    p.PremiumMembershipID = Db.ToInteger(row["PremiumMembershipID"]);
                    p.Name = Db.ToString(row["Name"]);
                    p.Code = Db.ToString(row["Code"]);
                    p.Description = Db.ToString(row["Description"]);
                    p.DurationInMonths = Db.ToInteger(row["DurationInMonths"]);
                    p.MRP = Db.ToDecimal(row["MRP"]);
                    p.Discount = Db.ToDecimal(row["Discount"]);
                    p.DiscountedPrice = p.MRP - Math.Round(p.MRP * (p.Discount / 100));
                    PremiumMembership.Add(p);
                }
                response.PremiumMembership = PremiumMembership;
            }
            if (ds != null && ds.Tables.Count > 18 && ds.Tables[18].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[18].Rows)
                {
                    var l = new LanguageMaster();
                    l.ID = Db.ToInteger(row["ID"]);
                    l.Name = Db.ToString(row["Name"]);
                    LanguageMaster.Add(l);
                }
                response.LanguageMaster = LanguageMaster;
            }
        }

        #region code
        //               // puja.DiscountedPrice = puja.MRP - Math.Round(puja.MRP * (puja.Discount / 100));
        //                foreach (var pp in response.PricePercentage)
        //                {
        //                    if (pp.CategoryID == puja.CategoryID)
        //                    {
        //                        var total = puja.MRP + Math.Round(puja.MRP * (pp.Percentage / 100));
        //                        puja.DiscountedPrice =  total - Math.Round(total * (puja.Discount / 100));
        //                    }
        //                }
        #endregion

        public List<TimeSlotMaster> GetTimeSlots()
        {

            var times = new List<TimeSlotMaster>();
            DbParam[] param = new DbParam[0];
            DataSet ds = Db.GetDataSet("usp_get_time_slot", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var t = new TimeSlotMaster();
                    t.ID = Db.ToInteger(row["time_slot_id"]);
                    t.TimeSlot = Db.ToString(row["name"]);
                    t.StartTime = Db.ToDateTime(row["start_time"]);
                    times.Add(t);
                }
            }

            return times;
        }


        public List<UserDetails> GetPurohitAstroProfileDetail(UserLoginEntity entity)
        {
            List<UserDetails> response = new List<UserDetails>();
            DbParam[] param = new DbParam[5];
            param[0] = new DbParam("@user_id", entity.UserID, SqlDbType.Int);
            param[1] = new DbParam("@user_type", entity.UserRole, SqlDbType.VarChar);
            param[2] = new DbParam("@latitude", entity.Latitude.ToString(), SqlDbType.VarChar);
            param[3] = new DbParam("@longitude", entity.Longitude.ToString(), SqlDbType.VarChar);
            param[4] = new DbParam("@service_id", entity.ServiceID, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_ast_profile", param);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UserDetails user = new UserDetails();
                        user.Specializations = new List<Specialization>();
                        user.Services = new List<Service>();

                        user.UserID = Db.ToInteger(row["adm_user_id"]);
                        user.Username = Db.ToString(row["username"]);
                        user.Mobile = Db.ToString(row["mob_number"]);
                        user.Email = Db.ToString(row["email"]);
                        user.UserType = Db.ToString(row["user_type"]);
                        user.Badge = Db.ToString(row["badge"]);
                        user.Description = Db.ToString(row["description"]);
                        user.FirstName = Db.ToString(row["first_name"]);
                        user.LastName = Db.ToString(row["last_name"]);
                        user.WhatsappNo = Db.ToString(row["whatsapp_no"]);
                        user.Address = Db.ToString(row["address"]);
                        user.City = Db.ToString(row["city"]);
                        user.District = Db.ToString(row["district"]);
                        user.State = Db.ToString(row["state"]);
                        user.Rating = Db.ToInteger(row["rating"]);
                        user.Experience = Db.ToInteger(row["experience"]);
                        user.Latitude = Db.ToString(row["latitude"]);
                        user.Longitude = Db.ToString(row["longitude"]);

                        user.ImageName = Db.ToString(row["ImageName"]);
                        if (!string.IsNullOrEmpty(user.ImageName))
                            user.ImageName = string.Format(CommonMethods.CustomerRetPath + user.ImageName);
                        else
                            user.ImageName = CommonMethods.CustomerIcon;

                        var specialization = ds.Tables[1].Select("adm_user_id = " + user.UserID.ToString());
                        if (specialization != null && specialization.Any())
                        {
                            foreach (DataRow row1 in specialization)
                            {
                                Specialization s = new Specialization();
                                s.UserID = Db.ToInteger(row1["adm_user_id"]);
                                s.code = Db.ToString(row1["code"]);
                                s.name = Db.ToString(row1["name"]);
                                user.Specializations.Add(s);
                            }
                        }

                        var service = ds.Tables[2].Select("ast_id = " + user.UserID.ToString());
                        if (service != null && service.Any())
                        {
                            foreach (DataRow row2 in service)
                            {
                                Service s = new Service();
                                s.AstID = Db.ToInteger(row2["ast_id"]);
                                s.ServiceID = Db.ToInteger(row2["service_id"]);
                                s.MRP = Convert.ToDecimal(Db.ToString(row2["price"]));
                                s.Discount = Convert.ToDecimal(Db.ToString(row2["discount"]));
                                s.DiscountedPrice = s.MRP - Math.Round(s.MRP * (s.Discount / 100));
                                user.Services.Add(s);
                            }
                        }
                        response.Add(user);
                    }
                }
            }
            return response;
        }

        public List<OtpList> GetOTPList()
        {
            var output = _databaseContext.adm_user_otp.Where(x => x.is_verified == "N").OrderByDescending(x => x.adm_user_otp_id).Take(100).ToList().ConvertAll(x => new OtpList
            {
                OTP = x.encrypted_otp == null ? "" : x.encrypted_otp.Length == 12 ? EncryptDecrypt.Decrypt(x.encrypted_otp) : EncryptDecrypt.DecryptBcon(x.encrypted_otp),
                MobileNumber = x.mob_number
            });
            return output;
        }

        public Userprofile GetUserProfile(int user_id)
        {
            var profile = new Userprofile();


            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", user_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_user_profile", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                profile.UserID = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                profile.Username = Db.ToString(ds.Tables[0].Rows[0]["Username"]);
                profile.MobileNumber = Db.ToString(ds.Tables[0].Rows[0]["MobileNumber"]);
                profile.Email = Db.ToString(ds.Tables[0].Rows[0]["Email"]);
                profile.Description = Db.ToString(ds.Tables[0].Rows[0]["Description"]);
                profile.UserType = Db.ToString(ds.Tables[0].Rows[0]["UserType"]);
                profile.ReferalCode = Db.ToString(ds.Tables[0].Rows[0]["ReferalCode"]);
                profile.ImageName = Db.ToString(ds.Tables[0].Rows[0]["ImageName"]);
                if (!string.IsNullOrEmpty(profile.ImageName))
                    profile.ImageName = string.Format(CommonMethods.CustomerRetPath + profile.ImageName);
                else
                    profile.ImageName = CommonMethods.CustomerIcon;
                profile.FirstName = Db.ToString(ds.Tables[0].Rows[0]["FirstName"]);
                profile.LastName = Db.ToString(ds.Tables[0].Rows[0]["LastName"]);
                profile.WhatsappNo = Db.ToString(ds.Tables[0].Rows[0]["WhatsappNo"]);
                profile.Address = Db.ToString(ds.Tables[0].Rows[0]["Address"]);
                profile.City = Db.ToString(ds.Tables[0].Rows[0]["City"]);
                profile.Landmark = Db.ToString(ds.Tables[0].Rows[0]["Landmark"]);
                profile.DOB = Db.ToString(ds.Tables[0].Rows[0]["DOB"]);
                profile.TOB = Db.ToString(ds.Tables[0].Rows[0]["TOB"]);

                profile.AadharCardNumber = Db.ToString(ds.Tables[0].Rows[0]["AadharCardNumber"]);
                profile.Experience = Db.ToInteger(ds.Tables[0].Rows[0]["Experience"]);
                profile.AstrologerExperience = Db.ToInteger(ds.Tables[0].Rows[0]["AstrologerExperience"]);

                profile.PinCode = Db.ToString(ds.Tables[0].Rows[0]["PinCode"]);
                profile.Latitude = Db.ToString(ds.Tables[0].Rows[0]["Latitude"]);
                profile.Longitude = Db.ToString(ds.Tables[0].Rows[0]["Longitude"]);
                profile.StateId = Db.ToInteger(ds.Tables[0].Rows[0]["StateId"]);
                profile.DistrictId = Db.ToInteger(ds.Tables[0].Rows[0]["DistrictId"]);
                profile.StateName = Db.ToString(ds.Tables[0].Rows[0]["StateName"]);
                profile.DistrictName = Db.ToString(ds.Tables[0].Rows[0]["DistrictName"]);
                profile.Gender = Db.ToString(ds.Tables[0].Rows[0]["Gender"]);
                profile.Profession = Db.ToString(ds.Tables[0].Rows[0]["Profession"]);
                profile.RoleId = Db.ToInteger(ds.Tables[0].Rows[0]["RoleId"]);
                profile.RoleName = Db.ToString(ds.Tables[0].Rows[0]["RoleName"]);
                profile.Badge = Db.ToString(ds.Tables[0].Rows[0]["Badge"]);

            }

            return profile;
        }

        public string InsertCustomer(Userprofile _object, int user_Id)
        {
            string result = string.Empty;
            DbParam[] param = new DbParam[19];

            param[0] = new DbParam("@Ids", user_Id, SqlDbType.NVarChar);
            param[1] = new DbParam("@FirstName", _object.FirstName, SqlDbType.NVarChar);
            param[2] = new DbParam("@LastName", _object.LastName, SqlDbType.NVarChar);
            param[3] = new DbParam("@ReferalCode", _object.ReferalCode, SqlDbType.NVarChar);
            param[4] = new DbParam("@DOB", _object.DOB, SqlDbType.NVarChar);
            param[5] = new DbParam("@TOB", _object.TOB, SqlDbType.Time);
            param[6] = new DbParam("@Gender", _object.Gender, SqlDbType.NVarChar);
            param[7] = new DbParam("@Profession", _object.Profession, SqlDbType.NVarChar);
            param[8] = new DbParam("@AadharCardNumber", _object.AadharCardNumber, SqlDbType.NVarChar);
            param[9] = new DbParam("@MobileNumber", _object.MobileNumber, SqlDbType.NVarChar);
            param[10] = new DbParam("@Email", _object.Email, SqlDbType.NVarChar);
            param[11] = new DbParam("@Address", _object.Address, SqlDbType.NVarChar);
            param[12] = new DbParam("@Landmark", _object.Landmark, SqlDbType.NVarChar);
            param[13] = new DbParam("@State_Id", _object.StateId, SqlDbType.Int);
            param[14] = new DbParam("@District_Id", _object.DistrictId, SqlDbType.Int);
            param[15] = new DbParam("@City", _object.City, SqlDbType.NVarChar);
            param[16] = new DbParam("@PinCode", _object.PinCode, SqlDbType.NVarChar);
            param[17] = new DbParam("@Latitude", _object.Latitude, SqlDbType.NVarChar);
            param[18] = new DbParam("@Longitude", _object.Longitude, SqlDbType.NVarChar);

            DataSet ds = Db.GetDataSet("usp_insert_customer", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = Db.ToString(ds.Tables[0].Rows[0]["retval"]);
            }

            return result;
        }

        public string UpdateUserProfile(Userprofile _object)
        {
            string result = string.Empty;

            DbParam[] param = new DbParam[17];
            param[0] = new DbParam("@user_id", _object.UserID, SqlDbType.Int);
            param[1] = new DbParam("@User_name", _object.Username, SqlDbType.NVarChar);
            param[2] = new DbParam("@First_Name", _object.FirstName, SqlDbType.NVarChar);
            param[3] = new DbParam("@Last_Name", _object.LastName, SqlDbType.NVarChar);
            param[4] = new DbParam("@Email", _object.Email, SqlDbType.NVarChar);
            param[5] = new DbParam("@Mobile_Number", _object.MobileNumber, SqlDbType.NVarChar);
            param[6] = new DbParam("@DOB", _object.DOB, SqlDbType.NVarChar);
            param[7] = new DbParam("@TOB", _object.TOB, SqlDbType.Time);
            param[8] = new DbParam("@Gender", _object.Gender, SqlDbType.NVarChar);
            param[9] = new DbParam("@Profession", _object.Profession, SqlDbType.NVarChar);
            param[10] = new DbParam("@State_Id", _object.StateId, SqlDbType.Int);
            param[11] = new DbParam("@District_Id", _object.DistrictId, SqlDbType.Int);
            param[12] = new DbParam("@City", _object.City, SqlDbType.NVarChar);
            param[13] = new DbParam("@Address", _object.Address, SqlDbType.NVarChar);
            param[14] = new DbParam("@Landmark", _object.Landmark, SqlDbType.NVarChar);
            param[15] = new DbParam("@PinCode", _object.PinCode, SqlDbType.NVarChar);
            param[16] = new DbParam("@ImageName", _object.ImageName, SqlDbType.NVarChar);

            DataSet ds = Db.GetDataSet("usp_update_user_profile", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
            }

            return result;
        }

        public string UpdateProhitProfile(PurohitRegisterEntity entity)
        {
            DataTable table = new DataTable();
            DataTable datatable = new DataTable();
            string result = string.Empty;

            // puja_path
            table.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("puja_path_Id", typeof(int))
            });

            foreach (var p in entity.PurohitPujaPath)
            {
                table.Rows.Add(p.Id, p.puja_path_Id);
            }

            datatable.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("service_Id", typeof(int))
            });

            foreach (var p in entity.AstrologerServices)
            {
                datatable.Rows.Add(p.Id, p.service_Id);
            }

            DataSet dataSet = new DataSet();
            DbParam[] param = new DbParam[19];

            param[0] = new DbParam("@user_id", entity.prohit_id, SqlDbType.Int);
            param[1] = new DbParam("@purohit_name", entity.purohit_name, SqlDbType.NVarChar);
            param[2] = new DbParam("@mobile_number", entity.mobile_number, SqlDbType.NVarChar);
            param[3] = new DbParam("@alternate_number", entity.alternate_number, SqlDbType.NVarChar);
            param[4] = new DbParam("@address", entity.address, SqlDbType.NVarChar);
            param[5] = new DbParam("@stateId", entity.stateId, SqlDbType.Int);
            param[6] = new DbParam("@districtId", entity.districtId, SqlDbType.Int);
            param[7] = new DbParam("@landmark", entity.landmark, SqlDbType.NVarChar);
            param[8] = new DbParam("@pin_code", entity.pin_code, SqlDbType.Int);

            param[9] = new DbParam("@astro_experience", entity.astro_experience, SqlDbType.Int);
            param[10] = new DbParam("@purohit_experience", entity.purohit_experience, SqlDbType.Int);
            param[11] = new DbParam("@is_prohit", entity.is_prohit, SqlDbType.NVarChar);
            param[12] = new DbParam("@is_astro", entity.is_astro, SqlDbType.NVarChar);
            param[13] = new DbParam("@temple_name", entity.temple_name, SqlDbType.NVarChar);

            param[14] = new DbParam("@latitude", entity.latitude, SqlDbType.NVarChar);
            param[15] = new DbParam("@longitude", entity.longitude, SqlDbType.NVarChar);
            param[16] = new DbParam("@ImageName", entity.ImageName, SqlDbType.NVarChar);

            param[17] = new DbParam("@tbl_PujaPath", table, SqlDbType.Structured);
            param[18] = new DbParam("@tbl_AstrologerServices", datatable, SqlDbType.Structured);
            dataSet = Db.GetDataSet("usp_update_purohit_register", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = Convert.ToString(dataSet.Tables[0].Rows[0]["retval"]);
            }
            return result;
        }

        public PurohitEntity GetProhitProfile(int prohit_id)
        {
            var profile = new PurohitEntity();


            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", prohit_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_prohit_profile", param);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                profile.ProhitId = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                profile.Username = Db.ToString(ds.Tables[0].Rows[0]["Username"]);
                profile.MobileNumber = Db.ToString(ds.Tables[0].Rows[0]["MobileNumber"]);
                profile.Email = Db.ToString(ds.Tables[0].Rows[0]["Email"]);
                profile.Description = Db.ToString(ds.Tables[0].Rows[0]["Description"]);
                profile.ReferalCode = Db.ToString(ds.Tables[0].Rows[0]["ReferalCode"]);
                profile.ImageName = Db.ToString(ds.Tables[0].Rows[0]["ImageName"]);
                profile.FirstName = Db.ToString(ds.Tables[0].Rows[0]["FirstName"]);
                profile.LastName = Db.ToString(ds.Tables[0].Rows[0]["LastName"]);
                profile.WhatsappNo = Db.ToString(ds.Tables[0].Rows[0]["WhatsappNo"]);
                profile.Address = Db.ToString(ds.Tables[0].Rows[0]["Address"]);
                profile.City = Db.ToString(ds.Tables[0].Rows[0]["City"]);
                profile.Landmark = Db.ToString(ds.Tables[0].Rows[0]["Landmark"]);
                profile.DOB = Db.ToString(ds.Tables[0].Rows[0]["DOB"]);
                profile.TOB = Db.ToString(ds.Tables[0].Rows[0]["TOB"]);
                profile.AadharCardNumber = Db.ToString(ds.Tables[0].Rows[0]["AadharCardNumber"]);
                profile.Experience = Db.ToInteger(ds.Tables[0].Rows[0]["Experience"]);
                profile.AstrologerExperience = Db.ToInteger(ds.Tables[0].Rows[0]["AstrologerExperience"]);
                profile.PinCode = Db.ToString(ds.Tables[0].Rows[0]["PinCode"]);
                profile.Latitude = Db.ToString(ds.Tables[0].Rows[0]["Latitude"]);
                profile.Longitude = Db.ToString(ds.Tables[0].Rows[0]["Longitude"]);
                profile.StateId = Db.ToInteger(ds.Tables[0].Rows[0]["StateId"]);
                profile.DistrictId = Db.ToInteger(ds.Tables[0].Rows[0]["DistrictId"]);
                profile.StateName = Db.ToString(ds.Tables[0].Rows[0]["StateName"]);
                profile.DistrictName = Db.ToString(ds.Tables[0].Rows[0]["DistrictName"]);
                profile.Gender = Db.ToString(ds.Tables[0].Rows[0]["Gender"]);
                profile.Profession = Db.ToString(ds.Tables[0].Rows[0]["Profession"]);
                profile.temple_name = Db.ToString(ds.Tables[0].Rows[0]["temple_name"]);
                if (!string.IsNullOrEmpty(profile.ImageName))
                    profile.ImageName = string.Format(CommonMethods.CustomerRetPath + profile.ImageName);
                else
                    profile.ImageName = CommonMethods.CustomerIcon;
                profile.RoleId = Db.ToString(ds.Tables[0].Rows[0]["RoleId"]);
                profile.RoleName = Db.ToString(ds.Tables[0].Rows[0]["RoleName"]);
                profile.Badge = Db.ToString(ds.Tables[0].Rows[0]["Badge"]);
                profile.MembershipType = Db.ToString(ds.Tables[0].Rows[0]["membership_name"]);
                profile.MembershipId = Db.ToInteger(ds.Tables[0].Rows[0]["premium_membership_id"]);

                profile.UserType = Db.ToString(ds.Tables[0].Rows[0]["UserType"]);
                string[] type = profile.UserType.Split(',');


                foreach (var role in type)
                {
                    if (role.ToUpper().Contains("PRHT"))
                    {
                        profile.is_prohit = "Y";
                    }
                    if (role.ToUpper().Contains("AST"))
                    {
                        profile.is_astro = "Y";
                    }
                }
            }
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    PurohitPujaPath pujapath = new PurohitPujaPath();
                    pujapath.Id = Db.ToInteger(row["Id"]);
                    pujapath.puja_path_Id = Db.ToInteger(row["puja_path_Id"]);
                    profile.PurohitPujaPath.Add(pujapath);
                }
            }
            if (ds != null && ds.Tables[2].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    AstrologerServices service = new AstrologerServices();
                    service.Id = Db.ToInteger(row["Id"]);
                    service.service_Id = Db.ToInteger(row["service_Id"]);
                    profile.AstrologerServices.Add(service);
                }
            }
            return profile;
        }

        public string PurohitRegister(PurohitRegisterEntity entity)
        {
            DataTable table = new DataTable();
            DataTable datatable = new DataTable();
            string result = string.Empty;

            // puja_path
            table.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("puja_path_Id", typeof(int))
            });

            foreach (var p in entity.PurohitPujaPath)
            {
                table.Rows.Add(p.Id, p.puja_path_Id);
            }

            datatable.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("service_Id", typeof(int))
            });

            foreach (var p in entity.AstrologerServices)
            {
                datatable.Rows.Add(p.Id, p.service_Id);
            }

            DataSet dataSet = new DataSet();
            DbParam[] param = new DbParam[19];

            param[0] = new DbParam("@prohit_id", entity.prohit_id, SqlDbType.Int);
            param[1] = new DbParam("@purohit_name", entity.purohit_name, SqlDbType.NVarChar);
            param[2] = new DbParam("@mobile_number", entity.mobile_number, SqlDbType.NVarChar);
            param[3] = new DbParam("@alternate_number", entity.alternate_number, SqlDbType.NVarChar);
            param[4] = new DbParam("@address", entity.address, SqlDbType.NVarChar);
            param[5] = new DbParam("@referal_code", entity.referal_code, SqlDbType.NVarChar);
            param[6] = new DbParam("@stateId", entity.stateId, SqlDbType.Int);
            param[7] = new DbParam("@districtId", entity.districtId, SqlDbType.Int);
            param[8] = new DbParam("@landmark", entity.landmark, SqlDbType.NVarChar);
            param[9] = new DbParam("@pin_code", entity.pin_code, SqlDbType.Int);
            param[10] = new DbParam("@astro_experience", entity.astro_experience, SqlDbType.Int);
            param[11] = new DbParam("@purohit_experience", entity.purohit_experience, SqlDbType.Int);
            param[12] = new DbParam("@is_prohit", entity.is_prohit, SqlDbType.NVarChar);
            param[13] = new DbParam("@is_astro", entity.is_astro, SqlDbType.NVarChar);
            param[14] = new DbParam("@temple_name", entity.temple_name, SqlDbType.NVarChar);
            param[15] = new DbParam("@latitude", entity.latitude, SqlDbType.NVarChar);
            param[16] = new DbParam("@longitude", entity.longitude, SqlDbType.NVarChar);

            param[17] = new DbParam("@tbl_PujaPath", table, SqlDbType.Structured);
            param[18] = new DbParam("@tbl_AstrologerServices", datatable, SqlDbType.Structured);

            dataSet = Db.GetDataSet("usp_save_purohit_register", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = Convert.ToString(dataSet.Tables[0].Rows[0]["retval"]);
            }
            return result;
        }
    }
}