using SwarajCustomer_Common;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SwarajCustomer_DAL.Interface.DashBoard
{
    public class DashBoardDAL : IDashBoardDAL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public DashBoardDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public DashBoardViewModel GetDashBoardUserCount(string start_date, string end_date)
        {
            DataSet _dataset = new DataSet();
            DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
            M_DashBoardUserCount _usercount = new M_DashBoardUserCount();
            List<UpcomingBirthdaysList> birthdaysList = new List<UpcomingBirthdaysList>();
            List<MonthWiseRevenue> monthWiseRevenueList = new List<MonthWiseRevenue>();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@start_date", start_date, SqlDbType.VarChar);
            param[1] = new DbParam("@end_date", end_date, SqlDbType.VarChar);
            _dataset = Db.GetDataSet("usp_get_dashboard_info", param);

            if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
            {
                _usercount.UserCount = Db.ToInteger(_dataset.Tables[0].Rows[0]["UserCount"]);
                _usercount.ActiveUserCount = Db.ToInteger(_dataset.Tables[0].Rows[0]["ActiveUserCount"]);
                _usercount.ProhitAstroCount = Db.ToInteger(_dataset.Tables[0].Rows[0]["ProhitAstroCount"]);
                _usercount.ActiveProhitAstroCount = Db.ToInteger(_dataset.Tables[0].Rows[0]["ActiveProhitAstroCount"]);
                _usercount.TotalBooking = Db.ToInteger(_dataset.Tables[0].Rows[0]["TotalBooking"]);
                _usercount.TotalTodayBooking = Db.ToInteger(_dataset.Tables[0].Rows[0]["TotalTodayBooking"]);
                _usercount.TotalBookingUpcoming = Db.ToInteger(_dataset.Tables[0].Rows[0]["TotalBookingUpcoming"]);
                _usercount.TotalRevenue = Db.ToDecimal(_dataset.Tables[0].Rows[0]["TotalRevenue"]);
                _usercount.TotalRemainingRevenue = Db.ToDecimal(_dataset.Tables[0].Rows[0]["TotalRemainingRevenue"]);
                
                dashBoardViewModel.DashBoardUserCount = _usercount;
            }
            if (_dataset != null && _dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in _dataset.Tables[1].Rows)
                {
                    UpcomingBirthdaysList b = new UpcomingBirthdaysList();
                    b.adm_user_id = Db.ToInteger(row["adm_user_id"]);
                    b.ImageUrl = Db.ToString(row["image_url"]);
                    if (!string.IsNullOrEmpty(b.ImageUrl))
                        b.ImageUrl = string.Format(CommonMethods.CustomerRetPath + b.ImageUrl);
                    else
                        b.ImageUrl = CommonMethods.CustomerIcon;
                    b.CustomerName = Db.ToString(row["customer_name"]);
                    b.Contact = Db.ToString(row["mob_number"]);
                    b.DOB = Db.ToString(row["DOB"]);
                    b.Email = Db.ToString(row["Email"]);
                    b.BirthdayEmailSent = Db.ToInteger(row["BirthdayEmailSent"]);
                    b.Diff_In_Day = Db.ToInteger(row["Diff_In_Day"]);
                    b.UpComeingBirthdayEmailSent = Db.ToInteger(row["UpComeingBirthdayEmailSent"]);


                    // sent alert email to user please booking your astrologer
                    if (!string.IsNullOrEmpty(b.Email) && b.Diff_In_Day == 2 && b.UpComeingBirthdayEmailSent == 0)
                    {
                        UpcomeingBirthdayEmailSent(param, b);
                    }

                    // here send email to  today birthday  user !!
                    var todaybirthday = b.DOB.Substring(0, b.DOB.LastIndexOf(",") + 1).Replace(",", "").Trim();
                    var currentdate = DateTime.Now.ToString("MMM dd").Trim();

                    if (!string.IsNullOrEmpty(b.Email) && todaybirthday == currentdate && b.BirthdayEmailSent == 0)
                    {
                        BirthdayEmailSent(param, b, todaybirthday);
                    }

                    birthdaysList.Add(b);
                }
                dashBoardViewModel.UpcomingBirthdays = birthdaysList;
            }
            if (_dataset != null && _dataset.Tables.Count > 2 && _dataset.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in _dataset.Tables[2].Rows)
                {
                    MonthWiseRevenue revenue = new MonthWiseRevenue();
                    revenue.Month = Db.ToString(row["Month"]);
                    revenue.Amount = Db.ToDecimal(row["Amount"]);

                    monthWiseRevenueList.Add(revenue);
                }
                dashBoardViewModel.MonthWiseRevenueList = monthWiseRevenueList;
            }
            return dashBoardViewModel;
        }

        private static void BirthdayEmailSent(DbParam[] param, UpcomingBirthdaysList b, string todaybirthday)
        {
            StringBuilder sb = new StringBuilder();
            string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.HappyBirthdayHtmlPath));
            sb.Append(misc_template.Replace("[CustomerName]", b.CustomerName));

            bool IsSend = CommonMethods.SendHtmlMail(new string[] { b.Email }, new string[] { CommonMethods.EmailCC }, " Happy Birthday " + b.CustomerName + "  for " + todaybirthday + " !!", sb.ToString(), 0);

            if (IsSend)
            {
                DbParam[] x = new DbParam[1];
                x[0] = new DbParam("@adm_user_id", b.adm_user_id, SqlDbType.Int);
                Db.Update("usp_update_birthday_email_sent", x);
            }
        }

        private static void UpcomeingBirthdayEmailSent(DbParam[] param, UpcomingBirthdaysList b)
        {
            StringBuilder sb = new StringBuilder();
            string misc_template = File.ReadAllText(HttpContext.Current.Server.MapPath(CommonMethods.AdvanceBirthdayHtmlPath));
            sb.Append(misc_template.Replace("[CustomerName]", b.CustomerName).Replace("[BirthdayDate]", b.DOB));
            bool IsSend = CommonMethods.SendHtmlMail(new string[] { b.Email }, new string[] { CommonMethods.EmailCC }, " We Care for you from Puja Guru !!", sb.ToString(), 0);

            if (IsSend)
            {
                DbParam[] x = new DbParam[1];
                x[0] = new DbParam("@adm_user_id", b.adm_user_id, SqlDbType.Int);
                Db.Update("usp_update_upcomeing_birthday_email_sent", x);
            }
        }

        public List<UpcomingBirthdaysList> GetComeingBirthday()
        {
            var _list = new List<UpcomingBirthdaysList>();
            try
            {
                DbParam[] param = new DbParam[0];
                DataSet _dataset = Db.GetDataSet("usp_get_all_upcomeing_birthday", param);

                if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dataset.Tables[0].Rows)
                    {
                        UpcomingBirthdaysList b = new UpcomingBirthdaysList();
                        b.adm_user_id = Db.ToInteger(row["adm_user_id"]);
                        b.ImageUrl = Db.ToString(row["image_url"]);
                        if (!string.IsNullOrEmpty(b.ImageUrl))
                            b.ImageUrl = string.Format(CommonMethods.CustomerRetPath + b.ImageUrl);
                        else
                            b.ImageUrl = CommonMethods.CustomerIcon;
                        b.CustomerName = Db.ToString(row["customer_name"]);
                        b.Contact = Db.ToString(row["mob_number"]);
                        b.DOB = Db.ToString(row["DOB"]);
                        b.Email = Db.ToString(row["Email"]);
                        b.BirthdayEmailSent = Db.ToInteger(row["BirthdayEmailSent"]);
                        b.Diff_In_Day = Db.ToInteger(row["Diff_In_Day"]);
                        b.UpComeingBirthdayEmailSent = Db.ToInteger(row["UpComeingBirthdayEmailSent"]);
                        _list.Add(b);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error in Dashboard/GetComeingBirthday", ex);
            }
            return _list;
        }

        public List<DropDownObject> GetState()
        {
            List<DropDownObject> catagory = new List<DropDownObject>();
            DbParam[] param = new DbParam[0];
            DataTable dataTable = Db.GetDataTable("usp_get_mst_state", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]); ;
                    catagory.Add(c);
                }
            }
            return catagory;
        }

        public List<DropDownObject> GetDistrict(int ids)
        {
            List<DropDownObject> catagory = new List<DropDownObject>();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@mst_state_id", ids, SqlDbType.Int);
            DataTable dataTable = Db.GetDataTable("usp_get_mst_district", param);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var c = new DropDownObject();
                    c.Id = Db.ToInteger(row["Id"]);
                    c.Name = Db.ToString(row["Name"]); ;
                    catagory.Add(c);
                }
            }
            return catagory;
        }
    }
}
