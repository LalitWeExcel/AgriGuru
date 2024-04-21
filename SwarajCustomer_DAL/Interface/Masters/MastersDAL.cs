using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;
using System.Data;

namespace SwarajCustomer_DAL.Interface.Master
{
    public class MastersDAL : IMastersDAL
    {
        private SwarajTestEntities context;

        public MastersDAL(SwarajTestEntities context)
        {
            this.context = context;
        }

        public int Update(M_MasterModel model, int admin_id)
        {
            DbParam[] param = new DbParam[7];
            param[0] = new DbParam("@Ids", model.CategoryId, SqlDbType.Int);
            param[1] = new DbParam("@name", model.Name, SqlDbType.NVarChar);
            param[2] = new DbParam("@description", model.Description, SqlDbType.NVarChar);
            param[3] = new DbParam("@admin_Id", admin_id, SqlDbType.Int);
            param[4] = new DbParam("@mrp", model.MRP, SqlDbType.Decimal);
            param[5] = new DbParam("@duration", model.Duration, SqlDbType.Int);
            param[6] = new DbParam("@IsActive", model.IsActive, SqlDbType.NVarChar);
            DataSet dataSet = Db.GetDataSet("usp_active_disactive_main_product", param);
            DataRow row1 = dataSet.Tables[0].Rows[0];
            return Db.ToInteger(row1["retval"]);
        }

        public List<DropDownObject> GetCategory()
        {
            List<DropDownObject> catagory = new List<DropDownObject>();
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

        public IList<M_MasterModel> GetMastersExcel(int Ids, out int recordsCount)
        {
            var _master = new List<M_MasterModel>();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@categoryId", Ids, SqlDbType.Int);

            DataSet dataSet = Db.GetDataSet("usp_get_masters_excel", param);
            recordsCount = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var m = new M_MasterModel(); ;
                    m.SrNo = Db.ToInteger(row["SrNo"]);
                    m.ProductId = Db.ToInteger(row["ProductId"]);
                    m.CategoryId = Db.ToInteger(row["CategoryId"]);
                    m.CategoryName = Db.ToString(row["CategoryName"]);
                    m.Name = Db.ToString(row["Name"]);
                    m.IsActive = Db.ToString(row["IsActive"]);
                    m.Description = Db.ToString(row["Description"]);
                    m.Code = Db.ToString(row["Code"]);
                    m.MRP = Db.ToDecimal(row["MRP"]);
                    m.Discount = Db.ToDecimal(row["Discount"]);
                    m.Duration = Db.ToInteger(row["Duration"]);
                    m.SamagriId = Db.ToInteger(row["SamagriId"]);
                    _master.Add(m);
                }
                DataRow row1 = dataSet.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }
            return _master;
        }

        public IList<M_MasterModel> GetMastersList(int page, int pageSize, int categoryId, out int recordsCount)
        {
            var _master = new List<M_MasterModel>();
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@page_index", page, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@categoryId", categoryId, SqlDbType.Int);

            DataSet dataSet = Db.GetDataSet("usp_get_masters_paging_list", param);
            recordsCount = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var m = new M_MasterModel();
                    m.SrNo = Db.ToInteger(row["SrNo"]);
                    m.ProductId = Db.ToInteger(row["ProductId"]);
                    m.CategoryId = Db.ToInteger(row["CategoryId"]);
                    //  Combos Packages
                    m.combos_packages_category_id = Db.ToInteger(row["combos_packages_category_id"]);
                    m.combos_packages_main_product_id = Db.ToInteger(row["combos_packages_main_product_id"]);
 
                    m.CategoryName = Db.ToString(row["CategoryName"]);
                    m.Name = Db.ToString(row["Name"]);
                    m.IsActive = Db.ToString(row["IsActive"]);
                    m.Description = Db.ToString(row["Description"]);
                    m.Code = Db.ToString(row["Code"]);
                    m.MRP = Db.ToDecimal(row["MRP"]);
                    m.Discount = Db.ToDecimal(row["Discount"]);
                    m.Duration = Db.ToInteger(row["Duration"]);
                    m.SamagriId = Db.ToInteger(row["SamagriId"]);

                    // location wize price increccc
                    m.A_LocationWisePrice = m.MRP + System.Math.Round(m.MRP * (Db.ToDecimal(row["A_LocationWisePrice"]) / 100));
                    m.B_LocationWisePrice = m.MRP + System.Math.Round(m.MRP * (Db.ToDecimal(row["B_LocationWisePrice"]) / 100));
                    m.C_LocationWisePrice = m.MRP + System.Math.Round(m.MRP * (Db.ToDecimal(row["C_LocationWisePrice"]) / 100));



                    _master.Add(m);
                }
                DataRow row1 = dataSet.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }
            return _master;
        }

        public M_ResponceResult Save(M_MasterModel data, int adminUserId)
        {
            var result = new M_ResponceResult();
            DbParam[] param = new DbParam[8];
            param[0] = new DbParam("@category_id", data.CategoryId, SqlDbType.Int);
            param[1] = new DbParam("@name", data.Name, SqlDbType.NVarChar);
            param[2] = new DbParam("@description", data.Description, SqlDbType.NVarChar);
            param[3] = new DbParam("@mrp", data.MRP, SqlDbType.Decimal);
            param[4] = new DbParam("@discount", data.Discount, SqlDbType.Decimal);
            param[5] = new DbParam("@duration", data.Duration, SqlDbType.Int);
            param[6] = new DbParam("@samagri_id", data.SamagriId, SqlDbType.Int);
            param[7] = new DbParam("@user_id", adminUserId, SqlDbType.Int);

            DataSet dataSet = Db.GetDataSet("usp_mst_save_main_product", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToInteger(dataSet.Tables[0].Rows[0]["retval"]);
            }

            return result;
        }

        public List<BulkUpload_Validation> SaveServicesBulkUpload(string FileContent, int ID, int UserId)
        {
           var responce = new List<BulkUpload_Validation>();
            DataSet ds = new DataSet();
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@xmolObj", FileContent, SqlDbType.Xml);
            param[1] = new DbParam("@userId", UserId, SqlDbType.Int);
            param[2] = new DbParam("@categoryId", ID, SqlDbType.Int);

            ds = Db.GetDataSet("usp_DM_bulkupload", param, CommonMethods.CmdTimeout);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var result = new BulkUpload_Validation();
                    result.RowNo = Db.ToString(row["RowNo"]);
                    result.ErrorMessage = Db.ToString(row["ErrorMessage"]);
                    responce.Add(result);
                }
            }
            return responce;
        }

        public M_ResponceResult SaveUpdateCombosPackages(M_CombosPackagesModel data, int admim_id)
        {
            var result = new M_ResponceResult();
            DbParam[] param = new DbParam[10];
            param[0] = new DbParam("@Ids", data.Id, SqlDbType.Int);
            param[1] = new DbParam("@combos_packages_category_id", data.combos_packages_category_id, SqlDbType.Int);
            param[2] = new DbParam("@combos_packages_main_product_id", data.combos_packages_main_product_id, SqlDbType.Int);
            param[3] = new DbParam("@Name", data.Name, SqlDbType.NVarChar);
            param[4] = new DbParam("@Description", data.Description, SqlDbType.NVarChar);
            param[5] = new DbParam("@IsActive", data.IsActive, SqlDbType.NVarChar);
            param[6] = new DbParam("@MRP", data.MRP, SqlDbType.Decimal);
            param[7] = new DbParam("@Discount", data.Discount, SqlDbType.Decimal);
            param[8] = new DbParam("@Duration", data.Duration, SqlDbType.Int);
            param[9] = new DbParam("@admim_id", admim_id, SqlDbType.Int);

            DataSet dataSet = Db.GetDataSet("usp_mst_save_update_combos_packages", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToInteger(dataSet.Tables[0].Rows[0]["retval"]);
            }

            return result;
        }
    }
}
