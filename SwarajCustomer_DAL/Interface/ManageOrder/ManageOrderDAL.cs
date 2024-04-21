using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SwarajCustomer_DAL.Interface.ManageOrder
{
    public class ManageOrderDAL : IManageOrderDAL
    {
        private SwarajTestEntities context;

        public ManageOrderDAL(SwarajTestEntities context)
        {
            this.context = context;
        }

        public string ConfirmProhit(int BookingID, string OrderNumber, int admin_id)
        {
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@booking_id", BookingID, SqlDbType.Int);
            param[1] = new DbParam("@order_number", OrderNumber, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", admin_id, SqlDbType.NVarChar);
            DataSet dataSet = Db.GetDataSet("usp_confirm_prohit_manage_order", param);
            DataRow row1 = dataSet.Tables[0].Rows[0];
            return Db.ToString(row1["result"]); ;
        }

        public M_ManageOrder Details(int ids, string ordernumber)
        {
            var response = new M_ManageOrder();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@ids", ids, SqlDbType.Int);
            param[1] = new DbParam("@ordernumber", ordernumber, SqlDbType.NVarChar);

            DataSet ds = Db.GetDataSet("usp_get_manage_order_details", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                response.SrNo = Db.ToInteger(ds.Tables[0].Rows[0]["SrNo"]);
                response.BookingID = Db.ToInteger(ds.Tables[0].Rows[0]["BookingID"]);
                response.OrderNumber = Db.ToString(ds.Tables[0].Rows[0]["OrderNumber"]);
                response.BookingDate = Db.ToString(ds.Tables[0].Rows[0]["BookingDate"]);
                response.PurohitID = Db.ToInteger(ds.Tables[0].Rows[0]["PurohitID"]);
                response.NoOfPandit = Db.ToInteger(ds.Tables[0].Rows[0]["NoOfPandit"]);

                response.UserID = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                response.PujaID = Db.ToInteger(ds.Tables[0].Rows[0]["PujaID"]);
                response.PujaName = Db.ToString(ds.Tables[0].Rows[0]["PujaName"]);
                response.PujaDate = Db.ToString(ds.Tables[0].Rows[0]["PujaDate"]);
                response.TimeSlot = Db.ToString(ds.Tables[0].Rows[0]["TimeSlot"]);
                response.IsSamagri = Db.ToString(ds.Tables[0].Rows[0]["IsSamagri"]);
                response.PujaMRP = Db.ToString(ds.Tables[0].Rows[0]["PujaMRP"]);
                response.PujaDiscount = Db.ToString(ds.Tables[0].Rows[0]["PujaDiscount"]);
                response.PujaDiscountedPrice = Db.ToString(ds.Tables[0].Rows[0]["PujaDiscountedPrice"]);
                response.WithSamagri = Db.ToString(ds.Tables[0].Rows[0]["WithSamagri"]);
                response.SamagriMRP = Db.ToString(ds.Tables[0].Rows[0]["SamagriMRP"]);
                response.SamagriDiscount = Db.ToString(ds.Tables[0].Rows[0]["SamagriDiscount"]);
                response.PurohitName = Db.ToString(ds.Tables[0].Rows[0]["PurohitName"]);
                response.CustName = Db.ToString(ds.Tables[0].Rows[0]["CustName"]);
                response.CustPhone = Db.ToString(ds.Tables[0].Rows[0]["CustPhone"]);
                response.CustAddress = Db.ToString(ds.Tables[0].Rows[0]["CustAddress"]);
                response.CustCity = Db.ToString(ds.Tables[0].Rows[0]["CustCity"]);
                response.CustState = Db.ToString(ds.Tables[0].Rows[0]["CustState"]);
                response.CustDistrict = Db.ToString(ds.Tables[0].Rows[0]["CustDistrict"]);
                response.CustPincode = Db.ToString(ds.Tables[0].Rows[0]["CustPincode"]);
                response.BookingType = Db.ToString(ds.Tables[0].Rows[0]["BookingType"]);
                //response.PurohitRating = Db.ToInteger(ds.Tables[0].Rows[0]["Rating"]);
                response.OTP = Db.ToString(ds.Tables[0].Rows[0]["OTP"]);
                response.WhatsApp = Db.ToString(ds.Tables[0].Rows[0]["WhatsApp"]);
                response.Skype = Db.ToString(ds.Tables[0].Rows[0]["Skype"]);
                response.EPujaEmail = Db.ToString(ds.Tables[0].Rows[0]["EPujaEmail"]);
                response.EPujaPhoneNo = Db.ToString(ds.Tables[0].Rows[0]["EPujaPhoneNo"]);
                response.PersionofContactName = Db.ToString(ds.Tables[0].Rows[0]["PersionofContactName"]);
                response.PersionofContactNumber = Db.ToString(ds.Tables[0].Rows[0]["PersionofContactNumber"]);
                response.Paid = Db.ToString(ds.Tables[0].Rows[0]["Paid"]);
                response.OrderStatus = Db.ToString(ds.Tables[0].Rows[0]["OrderStatus"]);
                response.IsActive = Db.ToString(ds.Tables[0].Rows[0]["IsActive"]);
                response.OTPConfirm = Db.ToString(ds.Tables[0].Rows[0]["OTPConfirm"]);
                response.AddToCart = Db.ToString(ds.Tables[0].Rows[0]["AddToCart"]);
                response.IsTemp = Db.ToString(ds.Tables[0].Rows[0]["IsTemp"]);
                response.Ression = Db.ToString(ds.Tables[0].Rows[0]["Ression"]);
                response.ConfirmStatus = Db.ToString(ds.Tables[0].Rows[0]["ConfirmStatus"]);
                response.RejectByProhit = Db.ToString(ds.Tables[0].Rows[0]["RejectByProhit"]);
                response.PaymentStatus = Db.ToString(ds.Tables[0].Rows[0]["PaymentStatus"]);
                response.PaymentMode = Db.ToString(ds.Tables[0].Rows[0]["PaymentMode"]);


            }
            return response;
        }

        public List<DropDownObject> GetProhit(string type)
        {
            var data = new List<DropDownObject>();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@BookingType", type, SqlDbType.NVarChar);
            DataTable dataTable = Db.GetDataTable("usp_get_manage_order_astro_prohit", param);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]); ;
                    data.Add(c);
                }
            }
            return data;
        }

        public IList<M_ManageOrder> GetManageOrderList(int pageIndex, int pageSize, string fromdate, string todate, string OrderStatus, string search, int State, int District, out int recordsCount)
        {
            var manageOrder = new List<M_ManageOrder>();
            DbParam[] param = new DbParam[8];
            param[0] = new DbParam("@page_index", pageIndex, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@from_date", fromdate, SqlDbType.NVarChar);
            param[3] = new DbParam("@to_date", todate, SqlDbType.NVarChar);
            param[4] = new DbParam("@OrderStatus", OrderStatus, SqlDbType.NVarChar);
            param[5] = new DbParam("@search", search, SqlDbType.NVarChar);
            param[6] = new DbParam("@state_id", State, SqlDbType.Int);
            param[7] = new DbParam("@district_id", District, SqlDbType.Int);
            DataSet dataSet = Db.GetDataSet("usp_get_manage_order_paging_list", param);
            recordsCount = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    var b = new M_ManageOrder();
                    b.SrNo = Db.ToInteger(row["SrNo"]);
                    b.BookingID = Db.ToInteger(row["BookingID"]);
                    b.OrderNumber = Db.ToString(row["OrderNumber"]);
                    b.BookingDate = Db.ToString(row["BookingDate"]);
                    b.PurohitID = Db.ToInteger(row["PurohitID"]);
                    b.NoOfPandit = Db.ToInteger(row["NoOfPandit"]);

                    b.UserID = Db.ToInteger(row["UserID"]);
                    b.PujaID = Db.ToInteger(row["PujaID"]);
                    b.PujaName = Db.ToString(row["PujaName"]);
                    b.PujaDate = Db.ToString(row["PujaDate"]);
                    b.TimeSlot = Db.ToString(row["TimeSlot"]);
                    b.IsSamagri = Db.ToString(row["IsSamagri"]);
                    b.PujaMRP = Db.ToString(row["PujaMRP"]);
                    b.PujaDiscount = Db.ToString(row["PujaDiscount"]);
                    b.PujaDiscountedPrice = Db.ToString(row["PujaDiscountedPrice"]);
                    b.WithSamagri = Db.ToString(row["WithSamagri"]);
                    b.SamagriMRP = Db.ToString(row["SamagriMRP"]);
                    b.SamagriDiscount = Db.ToString(row["SamagriDiscount"]);
                    b.PurohitName = Db.ToString(row["PurohitName"]);
                    b.CustName = Db.ToString(row["CustName"]);
                    b.CustPhone = Db.ToString(row["CustPhone"]);
                    b.CustAddress = Db.ToString(row["CustAddress"]);
                    b.CustCity = Db.ToString(row["CustCity"]);
                    b.CustState = Db.ToString(row["CustState"]);
                    b.CustDistrict = Db.ToString(row["CustDistrict"]);
                    b.CustPincode = Db.ToString(row["CustPincode"]);
                    b.BookingType = Db.ToString(row["BookingType"]);
                    // b.PurohitRating = Db.ToInteger(row["Rating"]);
                    b.OTP = Db.ToString(row["OTP"]);
                    b.WhatsApp = Db.ToString(row["WhatsApp"]);
                    b.Skype = Db.ToString(row["Skype"]);
                    b.EPujaEmail = Db.ToString(row["EPujaEmail"]);
                    b.EPujaPhoneNo = Db.ToString(row["EPujaPhoneNo"]);
                    b.PersionofContactName = Db.ToString(row["PersionofContactName"]);
                    b.PersionofContactNumber = Db.ToString(row["PersionofContactNumber"]);
                    if (Db.ToString(row["Paid"]) == "Y")
                    {
                        b.Paid = "Yes";
                    }
                    else
                    {
                        b.Paid = "No";
                    }
                    b.OrderStatus = Db.ToString(row["OrderStatus"]);
                    b.IsActive = Db.ToString(row["IsActive"]);
                    b.OTPConfirm = Db.ToString(row["OTPConfirm"]);
                    b.AddToCart = Db.ToString(row["AddToCart"]);
                    b.IsTemp = Db.ToString(row["IsTemp"]);
                    b.Ression = Db.ToString(row["Ression"]);
                    b.ConfirmStatus = Db.ToString(row["ConfirmStatus"]);
                    b.RejectByProhit = Db.ToString(row["RejectByProhit"]);
                    b.Buttion = b.BookingType == "Astrologer" ? "Assign Astrologer" : "Assign Purohit";


                    b.PaymentStatus = Db.ToString(row["PaymentStatus"]);
                    b.PaymentMode = Db.ToString(row["PaymentMode"]);
                    b.PaymentDetails = Db.ToString(row["PaymentDetails"]);
                    

                    manageOrder.Add(b);
                }
                DataRow row1 = dataSet.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
            }
            return manageOrder;
        }

        public M_Responce Update(M_UpdateProhits model, int user_id)
        {
            var result = new M_Responce();
            DataSet dataSet = new DataSet();
            DbParam[] param = new DbParam[5];
            param[0] = new DbParam("@booking_id", model.BookingID, SqlDbType.Int);
            param[1] = new DbParam("@order_number", model.OrderNumber, SqlDbType.NVarChar);
            param[2] = new DbParam("@Purohit_id", model.NewPurohitID, SqlDbType.Int);
            param[3] = new DbParam("@Ression", model.Ression, SqlDbType.NVarChar);
            param[4] = new DbParam("@user_id", user_id, SqlDbType.Int);

             if (model.BookingType == Db.ToString(BookingType.Astrologer))//astrologer
                dataSet = Db.GetDataSet("usp_update_manage_order_astro", param);
            else
                dataSet = Db.GetDataSet("usp_update_manage_order_puja", param);


            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }

        public M_Responce UpdatePackage(M_UpdatePackage model, int user_id)
        {
            var result = new M_Responce();
            DataTable package_multiple_prohits = new DataTable();
            List<string> MuiltipleNewPurohitIDs = model.NewPurohitID.Split(',').ToList();


            package_multiple_prohits.Columns.AddRange(new DataColumn[4]
            {
                new DataColumn("booking_id", typeof(int)),
                new DataColumn("order_number", typeof(string)),
                new DataColumn("user_id", typeof(int)),
                new DataColumn("muiltiple_purohit_id", typeof(int))

            });

            foreach (var ids in MuiltipleNewPurohitIDs)
            {
                package_multiple_prohits.Rows.Add(model.BookingID, model.OrderNumber, user_id, ids);
            }

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@booking_id", model.BookingID, SqlDbType.Int);
            param[1] = new DbParam("@order_number", model.OrderNumber, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            param[3] = new DbParam("@tbl_package_multiple_prohits", package_multiple_prohits, SqlDbType.Structured);
            //param[3] = new DbParam("@muiltiple_purohit_id", model.NewPurohitID, SqlDbType.NVarChar);
            //usp_update_manage_order_package_backup

            DataSet dataSet = Db.GetDataSet("usp_update_manage_order_package", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }
    }
}
