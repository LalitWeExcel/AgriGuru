using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SwarajCustomer_DAL.Interface.Advertising
{
    public class AdvertisingDAL : IAdvertisingDAL
    {
        private SwarajTestEntities context;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdvertisingDAL(SwarajTestEntities context)
        {
            this.context = context;
        }






        public int Delete(int Id, string type)
        {
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@Id", Id, SqlDbType.Int);
            DataSet dataSet = Db.GetDataSet(type == "F" ? "usp_delete_manage_fav_vedios" : "usp_delete_advertising", param);
            DataRow row1 = dataSet.Tables[0].Rows[0];
            return Db.ToInteger(row1["retval"]); ;
        }

        public List<DropDownObject> GetTimeSlot()
        {
            List<DropDownObject> catagory = new List<DropDownObject>();
            DbParam[] param = new DbParam[0];
            DataTable dataTable = Db.GetDataTable("usp_get_mst_time_slot", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]);
                    catagory.Add(c);
                }
            }
            return catagory;
        }

        public List<DropDownObject> GetCategorys()
        {
            var catagory = new List<DropDownObject>();
            DbParam[] param = new DbParam[0];
            DataTable dataTable = Db.GetDataTable("usp_get_mst_category", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]);
                    catagory.Add(c);
                }
            }
            return catagory;
        }
        public List<DropDownObject> GetLanguages()
        {
            var catagory = new List<DropDownObject>();
            DbParam[] param = new DbParam[0];
            DataTable dataTable = Db.GetDataTable("usp_get_mst_language", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]);
                    catagory.Add(c);
                }
            }
            return catagory;
        }

        public List<MasterDropDown> GetMasterCategorys(int category_id)
        {
            List<MasterDropDown> catagory = new List<MasterDropDown>();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@category_id", category_id, SqlDbType.Int);
            DataTable dataTable = Db.GetDataTable("usp_get_main_product", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new MasterDropDown();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]);
                    c.MRP = Db.ToDecimal(row["MRP"]);
                    c.Duration = Db.ToInteger(row["Duration"]);
                    catagory.Add(c);
                }
            }
            return catagory;
        }
        public IList<M_Advertisement> GetAdvertisementList(int page, int pageSize, string search, string type, int state_id, int district_id, out int recordsCount)
        {
            var list = new List<M_Advertisement>();

            DbParam[] param = new DbParam[6];
            param[0] = new DbParam("@page_index", page, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@type", type, SqlDbType.NVarChar);
            param[3] = new DbParam("@search", search, SqlDbType.NVarChar);
            param[4] = new DbParam("@state_id", state_id, SqlDbType.Int);
            param[5] = new DbParam("@district_id", district_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_manage_advertising_list", param);
            recordsCount = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var ad = new M_Advertisement();
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
                    }
                    else
                    {
                        ad.DiscountedAmount = ad.ActualAmount - (ad.ActualAmount * ad.Discount / 100);
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
                    ad.Url = Db.ToString(row["url"]);
                    ad.IsActive = Db.ToString(row["is_active"]);
                    ad.StateName = Db.ToString(row["StateName"]);
                    ad.DistrictName = Db.ToString(row["DistrictName"]);
                    ad.ImageName = Db.ToString(row["ImageName"]);
                    if (!string.IsNullOrEmpty(ad.ImageName))
                        ad.ImageName = CommonMethods.GetAdvertismentImageUrl(ad.ImageName);
                    else
                        ad.ImageName = CommonMethods.AdvertismentIcon;


                    ad.VideoName = Db.ToString(row["VideoName"]);
                    if (!string.IsNullOrEmpty(ad.VideoName))
                        ad.VideoName = CommonMethods.GetAdvertismentVedioUrl(ad.VideoName);
                    else
                        ad.VideoName = CommonMethods.AdvertismentVideosIcon;

                    list.Add(ad);
                }
                DataRow row1 = ds.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }

            return list;
        }

        public M_ResponceResult SaveUpdate(M_SaveAdvertisement model, int adminUserId)
        {
            DataTable Multiple_DistrictArray = new DataTable();
            if (!string.IsNullOrEmpty(model.BookingDate))
                model.BookingDate = CommonMethods.FormatDate(model.BookingDate, "dd-MM-yyyy", "yyyy-MM-dd");

            if (model.Discount > 0)
                model.DiscountInRupees = 0;
            else if (model.DiscountInRupees > 0)
                model.Discount = 0;

            Multiple_DistrictArray.Columns.AddRange(new DataColumn[4]
            {
                new DataColumn("trn_ads_id", typeof(int)),
                new DataColumn("latitude", typeof(string)),
                new DataColumn("longitude", typeof(string)),
                new DataColumn("mst_district_id", typeof(int))
            });

            foreach (var item in model.MuiltipleDistrictList)
            {
                var GetLatLong = GoogleLocation.GetLatLongByAddress(model.StateName + " , " + item.DistrictName);

                Multiple_DistrictArray.Rows.Add(
                model.AdvertisementId,
                GetLatLong.latitude,
                GetLatLong.longitude,
                item.DistrictId);
            }



            var result = new M_ResponceResult();
            DbParam[] param = new DbParam[15];
            param[0] = new DbParam("@ids", model.AdvertisementId, SqlDbType.Int);
            param[1] = new DbParam("@CategoryId", model.CategoryId, SqlDbType.Int);
            param[2] = new DbParam("@main_product_id", model.MainProductId, SqlDbType.Int);
            param[3] = new DbParam("@Discount", Db.ToDecimal(model.Discount), SqlDbType.Decimal);
            param[4] = new DbParam("@DiscountInRupees", Db.ToInteger(model.DiscountInRupees), SqlDbType.Int);
            param[5] = new DbParam("@description", model.Description, SqlDbType.NVarChar);
            param[6] = new DbParam("@advertising_type", model.Type, SqlDbType.NVarChar);
            param[7] = new DbParam("@user_id", adminUserId, SqlDbType.Int);
            param[8] = new DbParam("@booking_date", model.BookingDate, SqlDbType.NVarChar);
            param[9] = new DbParam("@time_slot_id", model.TimeSlotid, SqlDbType.Int);
            param[10] = new DbParam("@is_global", model.IsGlobal, SqlDbType.NVarChar);
            param[11] = new DbParam("@Url", model.Url, SqlDbType.NVarChar);
            param[12] = new DbParam("@mst_state_id", model.StateId, SqlDbType.Int);
            param[13] = new DbParam("@ImageName", model.ImageName, SqlDbType.NVarChar);
            param[14] = new DbParam("@type_trn_ads_district", Multiple_DistrictArray, SqlDbType.Structured);

            DataSet dataSet = Db.GetDataSet("usp_mst_save_trn_ads", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToInteger(dataSet.Tables[0].Rows[0]["retval"]);
            }

            return result;
        }



        public M_SaveAdvertisement Get(int ids)
        {
            var _object = new M_SaveAdvertisement();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@ids", ids, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_manage_advertising", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _object.AdvertisementId = Db.ToInteger(ds.Tables[0].Rows[0]["AdvertisementId"]);
                _object.CategoryId = Db.ToInteger(ds.Tables[0].Rows[0]["CategoryId"]);
                _object.MainProductId = Db.ToInteger(ds.Tables[0].Rows[0]["MainProductId"]);


                _object.StateId = Db.ToInteger(ds.Tables[0].Rows[0]["StateId"]);
                _object.DistrictId = Db.ToString(ds.Tables[0].Rows[0]["DistrictId"]);

                _object.Discount = Db.ToDecimal(ds.Tables[0].Rows[0]["Discount"]);
                _object.DiscountInRupees = Db.ToInteger(ds.Tables[0].Rows[0]["DiscountInRupees"]);
                _object.Description = Db.ToString(ds.Tables[0].Rows[0]["Description"]);

                _object.Type = Db.ToString(ds.Tables[0].Rows[0]["advertising_type"]);
                _object.IsActive = Db.ToString(ds.Tables[0].Rows[0]["IsActive"]);
                _object.BookingDate = Db.ToString(ds.Tables[0].Rows[0]["BookingDate"]);
                _object.TimeSlotid = Db.ToInteger(ds.Tables[0].Rows[0]["TimeSlotid"]);
                _object.IsGlobal = Db.ToString(ds.Tables[0].Rows[0]["IsGlobal"]);
                _object.Url = Db.ToString(ds.Tables[0].Rows[0]["Url"]);

                _object.MRP = Db.ToDecimal(ds.Tables[0].Rows[0]["MRP"]);
                _object.Duration = Db.ToInteger(ds.Tables[0].Rows[0]["Duration"]);

                _object.ImageName = Db.ToString(ds.Tables[0].Rows[0]["ImageName"]);
                _object.VideoName = Db.ToString(ds.Tables[0].Rows[0]["VideoName"]);
                _object.Title = Db.ToString(ds.Tables[0].Rows[0]["Title"]);



                if (!string.IsNullOrEmpty(_object.ImageName))
                    _object.ImageName = CommonMethods.GetAdvertismentImageUrl(_object.ImageName);
                else
                    _object.ImageName = CommonMethods.AdvertismentIcon;


                if (!string.IsNullOrEmpty(_object.VideoName))
                    _object.VideoName = CommonMethods.GetAdvertismentVedioUrl(_object.VideoName);
                else
                    _object.VideoName = CommonMethods.AdvertismentVideosIcon;

            }
            return _object;
        }

        public M_ResponceResult SaveUpdateIndependentads(M_SaveIndependentads model, int adminUserId)
        {

            if (model.Discount > 0)
                model.DiscountInRupees = 0;
            else if (model.DiscountInRupees > 0)
                model.Discount = 0;

            var result = new M_ResponceResult();
            DbParam[] param = new DbParam[9];

            param[0] = new DbParam("@ids", model.AdvertisementId, SqlDbType.Int);
            param[1] = new DbParam("@Title", model.Title, SqlDbType.NVarChar);
            param[2] = new DbParam("@MRP", Db.ToDecimal(model.MRP), SqlDbType.Decimal);
            param[3] = new DbParam("@Discount", Db.ToDecimal(model.Discount), SqlDbType.Decimal);
            param[4] = new DbParam("@DiscountInRupees", Db.ToInteger(model.DiscountInRupees), SqlDbType.Int);
            param[5] = new DbParam("@description", model.Description, SqlDbType.NVarChar);
            param[6] = new DbParam("@user_id", adminUserId, SqlDbType.Int);
            param[7] = new DbParam("@ImageName", model.ImageName, SqlDbType.NVarChar);
            param[8] = new DbParam("@VideoName", model.VideoName, SqlDbType.NVarChar);

            DataSet dataSet = Db.GetDataSet("usp_save_independent_trn_ads", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToInteger(dataSet.Tables[0].Rows[0]["retval"]);
            }

            return result;
        }

        public int ActivateDeactivate(string Ids, string status, int admin_id, string type)
        {
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@Id", Ids, SqlDbType.NVarChar);
            param[1] = new DbParam("@admin_Id", admin_id, SqlDbType.Int);
            param[2] = new DbParam("@IsActive", status, SqlDbType.NVarChar);

            DataSet dataSet = Db.GetDataSet(type == "F" ? "usp_active_disactive_manage_fav_videos" : "usp_active_disactive_trn_ads", param);
            DataRow row1 = dataSet.Tables[0].Rows[0];
            return Db.ToInteger(row1["retval"]);
        }

        #region Manage Fav Videos
        public IList<M_ManageFavVideos> ManageFavVideosList(int page, int pageSize, string search, int languageId, out int recordsCount)
        {
            var vedios = new List<M_ManageFavVideos>();

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@page_index", page, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@search", search, SqlDbType.NVarChar);
            param[3] = new DbParam("@languageId", languageId, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_manage_fav_vedios_list", param);

            recordsCount = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var v = new M_ManageFavVideos();
                    v.SrNo = Db.ToInteger(row["SrNo"]);
                    v.ManageFavVideosId = Db.ToInteger(row["ManageFavVideosId"]);
                    v.AdvertisingType = Db.ToString(row["AdvertisingType"]);
                    v.Name = Db.ToString(row["Name"]);
                    v.Description = Db.ToString(row["Description"]);
                    v.IsActive = Db.ToString(row["IsActive"]);
                    v.VideoName = Db.ToString(row["VideoName"]);
                    if (!string.IsNullOrEmpty(v.VideoName))
                        v.VideoName = CommonMethods.GetAdvertismentVedioUrl(v.VideoName);
                    else
                        v.VideoName = CommonMethods.AdvertismentVideosIcon;
                    vedios.Add(v);
                }
                DataRow row1 = ds.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }

            return vedios;
        }

        public M_ResponceResult SaveManageFavVideos(M_ManageFavVideos model, int adminUserId)
        {
            var result = new M_ResponceResult();
            DbParam[] param = new DbParam[6];

            param[0] = new DbParam("@ids", model.ManageFavVideosId, SqlDbType.Int);
            param[1] = new DbParam("@name", model.Name, SqlDbType.NVarChar);
            param[2] = new DbParam("@description", model.Description, SqlDbType.NVarChar);
            param[3] = new DbParam("@user_id", adminUserId, SqlDbType.Int);
            param[4] = new DbParam("@VideoName", model.VideoName, SqlDbType.NVarChar);
            param[5] = new DbParam("@language_id", model.LanguageId, SqlDbType.Int);


            DataSet dataSet = Db.GetDataSet("usp_save_manage_fav_videos", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToInteger(dataSet.Tables[0].Rows[0]["retval"]);
            }
            return result;
        }

        public IList<M_ManageFavVideosRequest> GetFavVideos(int languageId)
        {
            return context.usp_api_manage_fav_vedios_list(languageId).ToList().ConvertAll(x => new M_ManageFavVideosRequest
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                LanguageId = x.LanguageId,
                LanguageName = x.LanguageName,
                IsActive = x.IsActive,
                AdvertisingType = x.AdvertisingType,
                VideoName = x.VideoName == "N/A" ? CommonMethods.AdvertismentVideosIcon : CommonMethods.AdvertismentVideosRetPath + x.VideoName
            }); 
        }
        #endregion
    }
}
