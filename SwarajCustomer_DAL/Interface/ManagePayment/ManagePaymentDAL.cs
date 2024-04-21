using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace SwarajCustomer_DAL.Interface.ManagePayment
{
    public class ManagePaymentDAL : IManagePaymentDAL
    {
        private SwarajTestEntities context;

        public ManagePaymentDAL(SwarajTestEntities context)
        {
            this.context = context;
        }



        public IList<M_ManagePayment> GetManagePaymentList(int pageIndex, int pageSize, string fromdate, string todate, string paymentstatus, string paymentmode, string search, out int recordsCount,out int totelSuccessPayment, out int totelFailedPayment, out decimal totelRevenue, int state_id, int district_id)
        {
            var managepayment = new List<M_ManagePayment>();
            DbParam[] param = new DbParam[9];
            param[0] = new DbParam("@page_index", pageIndex, SqlDbType.Int);
            param[1] = new DbParam("@page_size", pageSize, SqlDbType.Int);
            param[2] = new DbParam("@from_date", fromdate, SqlDbType.NVarChar);
            param[3] = new DbParam("@to_date", todate, SqlDbType.NVarChar);
            param[4] = new DbParam("@paymentstatus", paymentstatus, SqlDbType.NVarChar);
            param[5] = new DbParam("@paymentmode", paymentmode, SqlDbType.NVarChar);
            param[6] = new DbParam("@search", search, SqlDbType.NVarChar);
            param[7] = new DbParam("@state_id", state_id, SqlDbType.Int);
            param[8] = new DbParam("@district_id", district_id, SqlDbType.Int);

            DataSet dataSet = Db.GetDataSet("usp_get_manage_payment_paging_list", param);
            recordsCount = 0;
            totelSuccessPayment = 0;
            totelFailedPayment = 0;
            totelRevenue = 0;

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var p = new M_ManagePayment();
                    p.SrNo = Db.ToInteger(row["SrNo"]);
                    p.BookingID = Db.ToInteger(row["BookingID"]);
                    p.OrderNumber = Db.ToString(row["OrderNumber"]);
                    p.BookingDate = Db.ToString(row["BookingDate"]);
                    p.UserID = Db.ToInteger(row["UserID"]);
                    p.PurohitID = Db.ToInteger(row["PurohitID"]);
                    p.PujaID = Db.ToInteger(row["PujaID"]);
                    p.PujaName = Db.ToString(row["PujaName"]);
                    p.TimeSlot = Db.ToString(row["TimeSlot"]);
                    p.PurohitName = Db.ToString(row["PurohitName"]);
                    p.CustName = Db.ToString(row["CustName"]);
                    p.CustPhone = Db.ToString(row["CustPhone"]);
                    p.BookingType = Db.ToString(row["BookingType"]);
                    p.Paid = Db.ToString(row["Paid"]);
                    p.OrderStatus = Db.ToString(row["OrderStatus"]);
                    p.IsActive = Db.ToString(row["IsActive"]);
                    p.OTPConfirm = Db.ToString(row["OTPConfirm"]);
                    p.AddToCart = Db.ToString(row["AddToCart"]);
                    p.CustCity = Db.ToString(row["CustCity"]);
                    p.CustState = Db.ToString(row["CustState"]);
                    p.CustDistrict = Db.ToString(row["CustDistrict"]);
 
                    p.RazorPayDate = CommonMethods.FormatDate(Db.ToString(row["RazorPayDate"]), "yyyy-MM-dd", "dd-MM-yyyy");
                    p.trn_razorpay_order_id = Db.ToInteger(row["trn_razorpay_order_id"]);
                    p.rzp_paymentid = Db.ToString(row["rzp_paymentid"]);
                    p.rzp_orderid = Db.ToString(row["rzp_orderid"]);
                    p.rzp_amount = Db.ToDecimal(row["rzp_amount"]);
                    p.RemainingAmount = Db.ToDecimal(row["remaining_amount"]);
                    p.TotalAmount = decimal.Add(p.rzp_amount, p.RemainingAmount);

                    p.payment_status = Db.ToString(row["payment_status"]);
                    p.error_code = Db.ToString(row["error_code"]);
                    p.error_desc = Db.ToString(row["error_desc"]);
                    p.txn_id = Db.ToString(row["txn_id"]);
                    p.payment_mode = Db.ToString(row["payment_mode"]);
                    p.status = Db.ToString(row["status"]);
                    p.payment_details = Db.ToString(row["payment_details"]);
                    p.full_payment = Db.ToString(row["full_payment"]);
                   

                    managepayment.Add(p);
                }
                DataRow row1 = dataSet.Tables[1].Rows[0];
                if (row1 != null)
                {
                    recordsCount = Db.ToInteger(row1["count"]);
                }
                DataRow row2 = dataSet.Tables[2].Rows[0];
                if (row2 != null)
                {
                    totelSuccessPayment = Db.ToInteger(row2["totelSuccessPayment"]);
                }
                DataRow row3 = dataSet.Tables[3].Rows[0];
                if (row3 != null)
                {
                    totelFailedPayment = Db.ToInteger(row3["totelFailedPayment"]);
                }
                DataRow row4 = dataSet.Tables[4].Rows[0];
                if (row3 != null)
                {
                    totelRevenue = Db.ToDecimal(row4["totelRevenue"]);
                }
            }
            return managepayment;
        }

        public M_Responce Save(M_SaveManagePayment model)
        {
            var result = new M_Responce();


            DbParam[] param = new DbParam[15];
            param[0] = new DbParam("@rzp_paymentid", model.rzp_paymentid, SqlDbType.NVarChar);
            param[1] = new DbParam("@rzp_orderid", model.rzp_orderid, SqlDbType.NVarChar);
            param[2] = new DbParam("@rzp_amount", model.rzp_amount, SqlDbType.Decimal);
            param[3] = new DbParam("@user_id", model.user_id, SqlDbType.Int);
            param[4] = new DbParam("@booking_id", model.booking_id, SqlDbType.Int);
            param[5] = new DbParam("@order_number", model.order_number, SqlDbType.NVarChar);

            param[6] = new DbParam("@payment_status", model.payment_status, SqlDbType.NVarChar);
            param[7] = new DbParam("@error_code", model.error_code, SqlDbType.NVarChar);
            param[8] = new DbParam("@error_desc", model.error_desc, SqlDbType.NVarChar);
            param[9] = new DbParam("@txn_id", model.txn_id, SqlDbType.NVarChar);
            param[10] = new DbParam("@payment_mode", model.payment_mode, SqlDbType.NVarChar);
            param[11] = new DbParam("@payment_details", model.payment_details, SqlDbType.NVarChar);
            param[12] = new DbParam("@status", model.status, SqlDbType.NVarChar);
            param[13] = new DbParam("@full_payment", model.full_payment, SqlDbType.NVarChar);
            param[14] = new DbParam("@remaining_amount", model.remaining_amount, SqlDbType.Decimal);
            
            DataSet dataSet = Db.GetDataSet("usp_save_razorpay_order", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
                result.Template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.AddToCartHtmlPath));
            }

            return result;
        }


        public M_Responce CheckoutMyCartPaymentSave(M_SaveManagePayment model, string username,string email)
        {

            StringBuilder sb = new StringBuilder();
            string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.CheckoutMyCartHtmlPath));
            var ordernumber = string.Join(",", model.order_number);
            sb.Append(misc_template.Replace("[CustomerName]", username).Replace("[OrderNumber]", ordernumber));

            var result = new M_Responce();
            DbParam[] param = new DbParam[15];
            param[0] = new DbParam("@rzp_paymentid", model.rzp_paymentid, SqlDbType.NVarChar);
            param[1] = new DbParam("@rzp_orderid", model.rzp_orderid, SqlDbType.NVarChar);
            param[2] = new DbParam("@rzp_amount", model.rzp_amount, SqlDbType.Decimal);
            param[3] = new DbParam("@user_id", model.user_id, SqlDbType.Int);
            param[4] = new DbParam("@booking_id", model.booking_id, SqlDbType.Int);
            param[5] = new DbParam("@order_number", model.order_number, SqlDbType.NVarChar);

            param[6] = new DbParam("@payment_status", model.payment_status, SqlDbType.NVarChar);
            param[7] = new DbParam("@error_code", model.error_code, SqlDbType.NVarChar);
            param[8] = new DbParam("@error_desc", model.error_desc, SqlDbType.NVarChar);
            param[9] = new DbParam("@txn_id", model.txn_id, SqlDbType.NVarChar);
            param[10] = new DbParam("@payment_mode", model.payment_mode, SqlDbType.NVarChar);
            param[11] = new DbParam("@payment_details", model.payment_details, SqlDbType.NVarChar);
            param[12] = new DbParam("@status", model.status, SqlDbType.NVarChar);
            param[13] = new DbParam("@full_payment", model.full_payment, SqlDbType.NVarChar);
            param[14] = new DbParam("@remaining_amount", model.remaining_amount, SqlDbType.Decimal);

            DataSet dataSet = Db.GetDataSet("usp_save_razorpay_order", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
                result.Template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.AddToCartHtmlPath));

                //if (result.Result.Contains("successfully"))
                //    CommonMethods.SendHtmlMail(new string[] { email }, new string[] { CommonMethods.EmailCC }, result.Result, sb.ToString(), 0);

            }
            return result;
        }

        public M_Responce SavePremiumMembership(M_SavePremiummemberShip model)
        {
            var result = new M_Responce();
            DbParam[] param = new DbParam[13];
            param[0] = new DbParam("@rzp_paymentid", model.rzp_paymentid, SqlDbType.NVarChar);
            param[1] = new DbParam("@rzp_orderid", model.rzp_orderid, SqlDbType.NVarChar);
            param[2] = new DbParam("@rzp_amount", model.rzp_amount, SqlDbType.Decimal);
            param[3] = new DbParam("@user_id", model.user_id, SqlDbType.Int);
            param[4] = new DbParam("@premium_membership_id", model.premium_membership_id, SqlDbType.Int);
            param[5] = new DbParam("@role_name", model.role_name, SqlDbType.NVarChar);
            param[6] = new DbParam("@payment_status", model.payment_status, SqlDbType.NVarChar);
            param[7] = new DbParam("@error_code", model.error_code, SqlDbType.NVarChar);
            param[8] = new DbParam("@error_desc", model.error_desc, SqlDbType.NVarChar);
            param[9] = new DbParam("@txn_id", model.txn_id, SqlDbType.NVarChar);
            param[10] = new DbParam("@payment_mode", model.payment_mode, SqlDbType.NVarChar);
            param[11] = new DbParam("@payment_details", model.payment_details, SqlDbType.NVarChar);
            param[12] = new DbParam("@status", model.status, SqlDbType.NVarChar);


            DataSet dataSet = Db.GetDataSet("usp_save_razorpay_premium_prohits", param);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result.Id = Db.ToInteger(dataSet.Tables[0].Rows[0]["Id"]);
                result.Result = Db.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }
    }
}
