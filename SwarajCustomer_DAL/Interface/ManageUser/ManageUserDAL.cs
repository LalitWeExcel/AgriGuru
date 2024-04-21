using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_DAL.Interface.ManageUser
{
    public class ManageUserDAL : IManageUserDAL
    {
        private SwarajTestEntities context;

        public ManageUserDAL(SwarajTestEntities context)
        {
            this.context = context;
        }

        public int ActivateDeactivate(string Ids,  string status, int admin_id)
        {
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@Id", Ids, SqlDbType.NVarChar);
            param[1] = new DbParam("@admin_Id", admin_id, SqlDbType.Int);
            param[2] = new DbParam("@IsActive", status, SqlDbType.NVarChar);
            DataSet dataSet  = Db.GetDataSet("usp_active_disactive_adm_user", param);
            DataRow row1 = dataSet.Tables[0].Rows[0];
            return Db.ToInteger(row1["retval"]);
        }


        public M_ManageUserDetails GetManageUserDetails(int user_id)
        {
            M_ManageUserDetails response = new M_ManageUserDetails();
            response.Services = new List<Service>();
            response.Specializations = new List<Specialization>();
            response.Createdby = new List<M_Createdby>();

            response.ManageUserRoles = new List<M_ManageUserRoles>();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", user_id, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_manage_user_details", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                response.adm_user_id = Db.ToInteger(ds.Tables[0].Rows[0]["adm_user_id"]);
                response.username = Db.ToString(ds.Tables[0].Rows[0]["username"]);
                response.mob_number = Db.ToString(ds.Tables[0].Rows[0]["mob_number"]);
                response.email = Db.ToString(ds.Tables[0].Rows[0]["email"]);
                response.user_type = Db.ToString(ds.Tables[0].Rows[0]["user_type"]);
                response.description = Db.ToString(ds.Tables[0].Rows[0]["description"]);
                response.referal_code = Db.ToString(ds.Tables[0].Rows[0]["referal_code"]);
                response.ImageName = Db.ToString(ds.Tables[0].Rows[0]["ImageName"]);
                if (!string.IsNullOrEmpty(response.ImageName))
                    response.ImageName = CommonMethods.GetCustomerImageUrl(response.ImageName);
                   /* response.ImageName = string.Format(CommonMethods.CustomerRetPath + response.ImageName)*/
                else
                    response.ImageName = CommonMethods.CustomerIcon;
                response.is_active = Db.ToString(ds.Tables[0].Rows[0]["is_active"]);
                response.user_details_id = Db.ToInteger(ds.Tables[0].Rows[0]["user_details_id"]);
                response.first_name = Db.ToString(ds.Tables[0].Rows[0]["first_name"]);
                response.last_name = Db.ToString(ds.Tables[0].Rows[0]["last_name"]);
                response.whatsapp_no = Db.ToString(ds.Tables[0].Rows[0]["whatsapp_no"]);
                response.address = Db.ToString(ds.Tables[0].Rows[0]["address"]);
                response.city = Db.ToString(ds.Tables[0].Rows[0]["city"]);
                response.pincode = Db.ToString(ds.Tables[0].Rows[0]["pincode"]);
                
                response.mst_district_id = Db.ToInteger(ds.Tables[0].Rows[0]["mst_district_id"]);
                response.mst_state_id = Db.ToInteger(ds.Tables[0].Rows[0]["mst_state_id"]);
                response.district = Db.ToString(ds.Tables[0].Rows[0]["district"]);
                response.state = Db.ToString(ds.Tables[0].Rows[0]["state"]);
                response.export_in_pooja = Db.ToString(ds.Tables[0].Rows[0]["export_in_pooja"]);
                response.ready_for_travel = Db.ToString(ds.Tables[0].Rows[0]["ready_for_travel"]);
                response.year_of_exp = Db.ToInteger(ds.Tables[0].Rows[0]["year_of_exp"]);
                response.marital_status = Db.ToString(ds.Tables[0].Rows[0]["marital_status"]);
                response.DOB = Db.ToString(ds.Tables[0].Rows[0]["DOB"]);
                response.TOB = Db.ToString(ds.Tables[0].Rows[0]["TOB"]);
                response.PlaceOfBirth = Db.ToString(ds.Tables[0].Rows[0]["place_of_birth"]);
                response.aadhar_number_name = Db.ToString(ds.Tables[0].Rows[0]["aadhar_number_name"]);
                response.qualification = Db.ToInteger(ds.Tables[0].Rows[0]["qualification"]);
                response.health_status = Db.ToInteger(ds.Tables[0].Rows[0]["health_status"]);
                response.landmark = Db.ToString(ds.Tables[0].Rows[0]["landmark"]);
                response.rating = Db.ToInteger(ds.Tables[0].Rows[0]["rating"]);
                response.latitude = Db.ToString(ds.Tables[0].Rows[0]["latitude"]);
                response.longitude = Db.ToString(ds.Tables[0].Rows[0]["longitude"]);
                response.astrologer_experience = Db.ToInteger(ds.Tables[0].Rows[0]["astrologer_experience"]);
                response.gender = Db.ToString(ds.Tables[0].Rows[0]["gender"]);
                response.profession = Db.ToString(ds.Tables[0].Rows[0]["profession"]);
                
               
                response.temple_name = Db.ToString(ds.Tables[0].Rows[0]["temple_name"]);

                var roles = ds.Tables[1].Select("adm_user_id = " + response.adm_user_id.ToString());
                if (roles != null && roles.Any())
                {
                    foreach (DataRow row1 in ds.Tables[1].Rows)
                    {
                        M_ManageUserRoles r = new M_ManageUserRoles();
                        r.adm_user_role_id = Db.ToInteger(row1["adm_user_role_id"]);
                        r.adm_user_id = Db.ToInteger(row1["adm_user_id"]);
                        r.adm_role_id = Db.ToInteger(row1["adm_role_id"]);
                        r.role_name = Db.ToString(row1["role_name"]);
                        response.ManageUserRoles.Add(r);
                    }
                }

                var specialization = ds.Tables[2].Select("adm_user_id = " + response.adm_user_id.ToString());
                if (specialization != null && specialization.Any())
                {
                    foreach (DataRow row1 in ds.Tables[2].Rows)
                    {
                        Specialization s = new Specialization();
                        s.UserID = Db.ToInteger(row1["adm_user_id"]);
                        s.code = Db.ToString(row1["code"]);
                        s.name = Db.ToString(row1["name"]);
                        response.Specializations.Add(s);
                    }
                }

                var service = ds.Tables[3].Select("ast_id = " + response.adm_user_id.ToString());
                if (service != null && service.Any())
                {
                    foreach (DataRow row2 in ds.Tables[3].Rows)
                    {
                        Service s = new Service();
                        s.AstID = Db.ToInteger(row2["ast_id"]);
                        s.ServiceID = Db.ToInteger(row2["service_id"]);
                        s.Name = Db.ToString(row2["name"]);
                        s.MRP = Db.ToDecimal(Db.ToString(row2["price"]));
                        s.Discount = Db.ToDecimal(Db.ToString(row2["discount"]));
                        s.DiscountedPrice = s.MRP - Math.Round(s.MRP * (s.Discount / 100));
                        response.Services.Add(s);
                    }
                }
                var c_by = ds.Tables[4].Select("c_by = " + response.adm_user_id.ToString());
                if (c_by != null && c_by.Any())
                {
                    foreach (DataRow row2 in ds.Tables[4].Rows)
                    {
                        M_Createdby c = new M_Createdby();
                        c.Ids = Db.ToInteger(row2["Ids"]);
                        c.Name = Db.ToString(row2["Name"]);
                        response.Createdby.Add(c);
                    }
                }
            }

            return response;
        }

        public IList<M_ManageUser> GetManageUserList(int page, int pageSize, string search, int role_id, out int recordsCount)
        {
            var manageuser = new List<M_ManageUser>();
            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@page_index", page, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@role_id", role_id, SqlDbType.Int);
            param[3] = new DbParam("@search", search, SqlDbType.NVarChar);

            DataSet dataSet = Db.GetDataSet("usp_get_manage_user_paging_list", param);
            recordsCount = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    M_ManageUser _user = new M_ManageUser();
                    _user.SrNo = Db.ToInteger(row["SrNo"]);
                    _user.UserId = Db.ToInteger(row["adm_user_id"]);
                    _user.UserName = Db.ToString(row["username"]);
                    _user.MobileNumber = Db.ToString(row["mob_number"]);
                    _user.Email = Db.ToString(row["email"]);
                    _user.ReferalCode = Db.ToString(row["referal_code"]);
                    _user.IsActive = Db.ToString(row["is_active"]);
                    _user.UserType = Db.ToString(row["user_type"]);
                    _user.ImageName = Db.ToString(row["ImageName"]);
                    if (!string.IsNullOrEmpty(_user.ImageName))
                        _user.ImageName = CommonMethods.GetCustomerImageUrl(_user.ImageName);
                       // response.ImageName = string.Format(CommonMethods.CustomerRetPath + response.ImageNam
                    else
                        _user.ImageName = CommonMethods.CustomerIcon;
                    manageuser.Add(_user);
                }
                DataRow row1 = dataSet.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }
            return manageuser;
        }

        public IList<M_ManageUserDetails> GetManageUserExcel( string search, int role_id, out int recordsCount)
        {
            var manageuser = new List<M_ManageUserDetails>();
            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@role_id", role_id, SqlDbType.Int);
            param[1] = new DbParam("@search", search, SqlDbType.NVarChar);

            DataSet dataSet = Db.GetDataSet("usp_get_manage_user_excel", param);
            recordsCount = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
             
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    var response = new M_ManageUserDetails();
                    response.SrNo = Db.ToInteger(row["SrNo"]);
                    response.adm_user_id = Db.ToInteger(row["adm_user_id"]);
                    response.username = Db.ToString(row["username"]);
                    response.mob_number = Db.ToString(row["mob_number"]);
                    response.email = Db.ToString(row["email"]);
                    response.user_type = Db.ToString(row["user_type"]);
                    response.description = Db.ToString(row["description"]);
                    response.referal_code = Db.ToString(row["referal_code"]);

                    response.is_active = Db.ToString(row["is_active"]);
                    response.user_details_id = Db.ToInteger(row["user_details_id"]);
                    response.first_name = Db.ToString(row["first_name"]);
                    response.last_name = Db.ToString(row["last_name"]);
                    response.whatsapp_no = Db.ToString(row["whatsapp_no"]);
                    response.address = Db.ToString(row["address"]);
                    response.city = Db.ToString(row["city"]);
       
                    response.district = Db.ToString(row["district"]);
                    response.state = Db.ToString(row["state"]);
                    response.export_in_pooja = Db.ToString(row["export_in_pooja"]);
                    response.ready_for_travel = Db.ToString(row["ready_for_travel"]);
                    response.year_of_exp = Db.ToInteger(row["year_of_exp"]);
                    response.marital_status = Db.ToString(row["marital_status"]);
                    response.DOB = Db.ToString(row["DOB"]);
                    response.aadhar_number_name = Db.ToString(row["aadhar_number_name"]);
                    response.qualification = Db.ToInteger(row["qualification"]);
                    response.health_status = Db.ToInteger(row["health_status"]);
                    response.landmark = Db.ToString(row["landmark"]);

                    response.rating = Db.ToInteger(row["rating"]);
                    response.astrologer_experience = Db.ToInteger(row["astrologer_experience"]);
                    response.TOB = Db.ToString(row["TOB"]);
                    response.temple_name = Db.ToString(row["temple_name"]);

                
                    manageuser.Add(response);
                }
                DataRow totel = dataSet.Tables[1].Rows[0];
                if (totel != null)
                {
                    recordsCount = Db.ToInteger(totel["count"]);
                }
            }
            return manageuser;
        }
        
         public IList<CommonEntity> GetAllUser()
        {
            var user = new List<CommonEntity>();
            DbParam[] param = new DbParam[0];
            DataSet dataSet = Db.GetDataSet("usp_get_all_users", param);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var response = new CommonEntity();
                    response.ID = Db.ToInteger(row["Id"]);
                    response.Name = Db.ToString(row["Name"]);
                    user.Add(response);
                }
            }
            return user;
        }

        public IList<M_HourlyTime> GetHourlyTimeList()
        {
            var time = new List<M_HourlyTime>();
            DbParam[] param = new DbParam[0];
            DataSet dataSet = Db.GetDataSet("usp_get_all_hourly_time", param);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var response = new M_HourlyTime();
                    response.ID = Db.ToInteger(row["ID"]);
                    response.Time = Db.ToString(row["Time"]);
                    time.Add(response);
                }
            }
            return time;
        }

        public IList<CommonEntity> GetRoleMaster()
        {
            IList<CommonEntity> rolesList = new List<CommonEntity>();
            var roles = context.adm_role.Where(x => x.is_active == "Y" && !x.code.Equals("ADMIN")).OrderBy(x=>x.sort_order).ToList();
            if (roles!=null && roles.Count()>0)
            {
                foreach (var item in roles)
                {
                    CommonEntity role = new CommonEntity();
                    role.ID = item.adm_role_id;
                    role.Name = item.name;
                    rolesList.Add(role);
                }
            }
            return rolesList;
        }


        public string Update(M_ManageUserDetails _object, int adminId)
        {
            string result = string.Empty;

            DbParam[] param = new DbParam[26];
            param[0] = new DbParam("@user_id", _object.adm_user_id, SqlDbType.Int);
            param[1] = new DbParam("@User_name", _object.username, SqlDbType.NVarChar);
            param[2] = new DbParam("@description", _object.description, SqlDbType.NVarChar);
            param[3] = new DbParam("@is_active", _object.is_active, SqlDbType.NVarChar);
            param[4] = new DbParam("@First_Name", _object.first_name, SqlDbType.NVarChar);
            param[5] = new DbParam("@Last_Name", _object.last_name, SqlDbType.NVarChar);
            param[6] = new DbParam("@whatsapp_no", _object.whatsapp_no, SqlDbType.NVarChar);
            param[7] = new DbParam("@Address", _object.address, SqlDbType.NVarChar);
            param[8] = new DbParam("@City", _object.city, SqlDbType.NVarChar);
            param[9] = new DbParam("@State_Id", _object.mst_state_id, SqlDbType.Int);
            param[10] = new DbParam("@District_Id", _object.mst_district_id, SqlDbType.Int);
            param[11] = new DbParam("@PinCode", _object.pincode, SqlDbType.NVarChar);
            param[12] = new DbParam("@Landmark", _object.landmark, SqlDbType.NVarChar);
            param[13] = new DbParam("@Gender", _object.gender, SqlDbType.NVarChar);
            param[14] = new DbParam("@export_in_pooja", _object.export_in_pooja, SqlDbType.NVarChar);
            param[15] = new DbParam("@ready_for_travel", _object.ready_for_travel, SqlDbType.NVarChar);
            param[16] = new DbParam("@marital_status", _object.marital_status, SqlDbType.NVarChar);
            param[17] = new DbParam("@DOB", _object.DOB, SqlDbType.NVarChar);
            param[18] = new DbParam("@TOB", _object.TOB, SqlDbType.Time);
            param[19] = new DbParam("@POB", _object.PlaceOfBirth, SqlDbType.NVarChar);
            param[20] = new DbParam("@aadhar_number_name", _object.aadhar_number_name, SqlDbType.NVarChar);
            param[21] = new DbParam("@health_status", _object.health_status, SqlDbType.Int);
            param[22] = new DbParam("@Profession", _object.profession, SqlDbType.NVarChar);
            param[23] = new DbParam("@temple_name", _object.temple_name, SqlDbType.NVarChar);
            

            param[24] = new DbParam("@ImageName", _object.ImageName, SqlDbType.NVarChar);
            param[25] = new DbParam("@adminId", adminId, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_update_manage_user", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
            }

            return result;
        }

        public IList<M_ManageUser> GetPurohitCustomerLinkingList(int page, int pageSize, string search, int  user_id, out int recordsCount,
             out int totalProhits, out int totalAstrollgers, out int totalCustomers)
        {
            var manageuser = new List<M_ManageUser>();
            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@page_index", page, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            param[3] = new DbParam("@search", search, SqlDbType.NVarChar);

            DataSet dataSet = Db.GetDataSet("usp_get_prohit_customer_linking_paging_list", param);
            recordsCount = 0;
            totalProhits = 0;
            totalAstrollgers = 0;
            totalCustomers = 0;
            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    M_ManageUser _user = new M_ManageUser();
                    _user.SrNo = Db.ToInteger(row["SrNo"]);
                    _user.UserId = Db.ToInteger(row["adm_user_id"]);
                    _user.UserName = Db.ToString(row["username"]);
                    _user.MobileNumber = Db.ToString(row["mob_number"]);
                    _user.Email = Db.ToString(row["email"]);
                    _user.ReferalCode = Db.ToString(row["referal_code"]);
                    _user.IsActive = Db.ToString(row["is_active"]);
                    _user.UserType = Db.ToString(row["user_type"]);
                    _user.Createdby = Db.ToString(row["Createdby"]);
                    _user.ImageName = Db.ToString(row["ImageName"]);
                    
                    if (!string.IsNullOrEmpty(_user.ImageName))
                        _user.ImageName = string.Format(CommonMethods.CustomerRetPath + _user.ImageName);
                    else
                        _user.ImageName = CommonMethods.CustomerIcon;
                    manageuser.Add(_user);
                }
                DataRow count = dataSet.Tables[1].Rows[0];
                DataRow prohit = dataSet.Tables[2].Rows[0];
                DataRow astrolger = dataSet.Tables[3].Rows[0];
                DataRow customer = dataSet.Tables[4].Rows[0];

                if (count != null)
                {
                    recordsCount = Db.ToInteger(count["count"]);
                }
       
                if (prohit != null)
                {
                    totalProhits = Db.ToInteger(prohit["totalProhits"]);
                }

                if (astrolger != null)
                {
                    totalAstrollgers = Db.ToInteger(astrolger["totalAstrollgers"]);
                }
         
                if (customer != null)
                {
                    totalCustomers = Db.ToInteger(customer["totalCustomers"]);
                }
            }
            return manageuser;
        }
    }
}
