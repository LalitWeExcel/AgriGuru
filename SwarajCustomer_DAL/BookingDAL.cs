using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Purohit;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace SwarajCustomer_DAL
{
    public class BookingDAL : IBookingDAL
    {
        Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public BookingDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<TimeSlotMaster> GetPujaBookedSlots(int pujaID, DateTime pujaDate)
        {
            List<TimeSlotMaster> timeSlotsList = new List<TimeSlotMaster>();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@puja_date", pujaDate, SqlDbType.DateTime);
            param[1] = new DbParam("@puja_id", pujaID, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_puja_booked_slots", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TimeSlotMaster time = new TimeSlotMaster();
                        time.ID = Db.ToInteger(row["time_slot_id"]);
                        time.TimeSlot = Db.ToString(row["name"]);
                        time.StartTime = Db.ToDateTime(row["start_time"]);
                        timeSlotsList.Add(time);
                    }
                }
            }

            return timeSlotsList;
        }

        public List<TimeSlotMaster> GetAstroBookedSlots(int astroID, DateTime astroDate)
        {
            List<TimeSlotMaster> timeSlotsList = new List<TimeSlotMaster>();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@astro_date", astroDate, SqlDbType.DateTime);
            param[1] = new DbParam("@astro_id", astroID, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_astro_booked_slots", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TimeSlotMaster time = new TimeSlotMaster();
                        time.ID = Db.ToInteger(row["time_slot_id"]);
                        time.TimeSlot = Db.ToString(row["time_slot"]);
                        time.StartTime = Db.ToDateTime(row["start_time"]);
                        time.DurationSlot = Db.ToString(row["duration_slot"]);
                        time.Duration = Db.ToInteger(row["duration"]);
                        time.DurationText = CommonMethods.GetDurationText(time.Duration);
                        timeSlotsList.Add(time);
                    }
                }
            }

            return timeSlotsList;
        }

        public Responce SavePujaOrder(PujaOrderDetails objPuja)
        {
            Responce responces = new Responce();
            DataTable puja_list = new DataTable();
            puja_list.Columns.AddRange(new DataColumn[28]
            {
                new DataColumn("puja_id", typeof(int)),
                new DataColumn("user_id", typeof(int)),
                new DataColumn("time_slot_id", typeof(int)),
                new DataColumn("puja_date", typeof(string)),
                new DataColumn("puja_mrp", typeof(string)),
                new DataColumn("puja_discount", typeof(string)),
                new DataColumn("puja_discount_price", typeof(string)),
                new DataColumn("is_samagri", typeof(bool)),
                new DataColumn("samagri_mrp", typeof(string)),
                new DataColumn("samagri_discount", typeof(string)),
                new DataColumn("samagri_discount_price", typeof(string)),
                new DataColumn("purohit_id", typeof(int)),
                new DataColumn("first_name", typeof(string)),
                new DataColumn("last_name", typeof(string)),
                new DataColumn("phone", typeof(string)),
                new DataColumn("address", typeof(string)),
                new DataColumn("city_name", typeof(string)),
                new DataColumn("district_id", typeof(int)),
                new DataColumn("state_id", typeof(int)),
                new DataColumn("landmark", typeof(string)),
                new DataColumn("pincode", typeof(string)),
                new DataColumn("is_for_other", typeof(string)),
                new DataColumn("latitude", typeof(string)),
                new DataColumn("longitude", typeof(string)),
                new DataColumn("otp", typeof(string)),
                new DataColumn("IsPaid", typeof(string)),
                new DataColumn("EPujaEmail", typeof(string)),
                new DataColumn("EPujaPhoneNo", typeof(string))
        });

            foreach (PujaDetails objDC in objPuja.PujasList)
            {
                puja_list.Rows.Add(
                    objDC.PujaID,
                    objPuja.CustomerDetail.Cust_Id,
                    objDC.PujaTime,
                    objDC.PujaDate,
                    objDC.PujaMRP,
                    objDC.PujaDiscount,
                    objDC.PujaDiscountedPrice,
                    objDC.IsSamagri,
                    objDC.SamagriMRP,
                    objDC.SamagriDiscount,
                    objDC.SamagriDiscountedPrice,
                    objDC.PurohitID,
                    objDC.FirstName,
                    objDC.LastName,
                    objDC.Phone,
                    objDC.Address,
                    objDC.CityName,
                    objDC.DistrictID,
                    objDC.StateID,
                    objDC.Landmark,
                    objDC.Pincode,
                    objDC.IsForOther,
                    objDC.Latitude,
                    objDC.Longitude,
                    GenerateRandom.GenerateRandomOTP(),
                    objDC.IsPaid,
                    objDC.EPujaEmail,
                    objDC.EPujaPhoneNo
                );
            }

            string orderNumber = GetOrderNumber();

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@order_number", orderNumber, SqlDbType.VarChar);
            param[1] = new DbParam("@cust_id", objPuja.CustomerDetail.Cust_Id, SqlDbType.Int);
            param[2] = new DbParam("@IsAddToCart", objPuja.IsAddToCart, SqlDbType.VarChar);
            param[3] = new DbParam("@tbl_puja_details", puja_list, SqlDbType.Structured);
            DataSet ds = Db.GetDataSet("usp_save_puja_order", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    responces.result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                    responces.booking_id = Db.ToInteger(ds.Tables[0].Rows[0]["booking_id"]);
                    responces.order_number = Db.ToString(ds.Tables[0].Rows[0]["order_number"]);
                }
            }

            return responces;
        }

        public Responce SaveAstroOrder(AstroOrderDetails objAstro)
        {
            Responce responces = new Responce();

            DataTable astro_list = new DataTable();
            astro_list.Columns.AddRange(new DataColumn[31]
            {
                new DataColumn("service_id", typeof(int)),
                new DataColumn("user_id", typeof(int)),
                new DataColumn("time_slot_id", typeof(int)),
                new DataColumn("duration_slot_id", typeof(int)),
                new DataColumn("astro_date", typeof(string)),
                new DataColumn("astro_mrp", typeof(string)),
                new DataColumn("astro_discount", typeof(string)),
                new DataColumn("astro_discount_price", typeof(string)),
                new DataColumn("astrologer_id", typeof(int)),
                new DataColumn("first_name", typeof(string)),
                new DataColumn("last_name", typeof(string)),
                new DataColumn("phone", typeof(string)),
                new DataColumn("address", typeof(string)),
                new DataColumn("city_name", typeof(string)),
                new DataColumn("district_id", typeof(int)),
                new DataColumn("state_id", typeof(int)),
                new DataColumn("landmark", typeof(string)),
                new DataColumn("pincode", typeof(string)),
                new DataColumn("is_for_other", typeof(string)),
                new DataColumn("latitude", typeof(string)),
                new DataColumn("longitude", typeof(string)),
                new DataColumn("otp", typeof(string)),
                new DataColumn("consultation_medium_id", typeof(int)),
                new DataColumn("consultation_type_id", typeof(int)),
                new DataColumn("whatsapp", typeof(string)),
                new DataColumn("skype", typeof(string)),
                new DataColumn("gender", typeof(string)),
                new DataColumn("dob", typeof(string)),
                new DataColumn("tob", typeof(string)),
                new DataColumn("pob", typeof(string)),
                new DataColumn("IsPaid", typeof(string))
        });

            foreach (AstroDetails objDC in objAstro.OrderList)
            {
                astro_list.Rows.Add(
                    objDC.ServiceID,
                    objAstro.CustomerDetail.Cust_Id,
                    objDC.BookingTime,
                    objDC.BookingDuration,
                    objDC.BookingDate,
                    objDC.AstroMRP,
                    objDC.AstroDiscount,
                    objDC.AstroDiscountedPrice,
                    objDC.AstrologerID,
                    objDC.FirstName,
                    objDC.LastName,
                    objDC.Phone,
                    objDC.Address,
                    objDC.CityName,
                    objDC.DistrictID,
                    objDC.StateID,
                    objDC.Landmark,
                    objDC.Pincode,
                    objDC.IsForOther,
                    objDC.Latitude,
                    objDC.Longitude,
                    GenerateRandom.GenerateRandomOTP(),
                    objDC.ConsultationMediumID,
                    objDC.ConsultationTypeID,
                    objDC.Whatsapp,
                    objDC.Skype,
                    objDC.Gender,
                    objDC.DOB,
                    objDC.TOB,
                    objDC.POB,
                    objDC.IsPaid
                );
            }

            string orderNumber = GetOrderNumber();

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@order_number", orderNumber, SqlDbType.VarChar);
            param[1] = new DbParam("@cust_id", objAstro.CustomerDetail.Cust_Id, SqlDbType.Int);
            param[2] = new DbParam("@IsAddToCart", objAstro.IsAddToCart, SqlDbType.VarChar);
            param[3] = new DbParam("@tbl_astro_details", astro_list, SqlDbType.Structured);
            DataSet ds = Db.GetDataSet("usp_save_astro_order", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    responces.result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                    responces.booking_id = Db.ToInteger(ds.Tables[0].Rows[0]["booking_id"]);
                    responces.order_number = Db.ToString(ds.Tables[0].Rows[0]["order_number"]);
                }
            }

            return responces;
        }

        public Responce SavePackageOrder(PackageOrderDetails _objects)
        {
            var responces = new Responce();
            var package_list = new DataTable();
            package_list.Columns.AddRange(new DataColumn[26]
            {

                new DataColumn("puja_id", typeof(int)),
                new DataColumn("package_id", typeof(int)),
                new DataColumn("prohit_duration_id", typeof(int)),
                new DataColumn("user_id", typeof(int)),
                new DataColumn("time_slot_id", typeof(int)),
                new DataColumn("puja_date", typeof(string)),
                new DataColumn("puja_mrp", typeof(string)),
                new DataColumn("puja_discount", typeof(string)),
                new DataColumn("puja_discount_price", typeof(string)),
                new DataColumn("persionof_contact_name", typeof(string)),
                new DataColumn("persionof_contact_number", typeof(string)),
                new DataColumn("purohit_id", typeof(int)),
                new DataColumn("first_name", typeof(string)),
                new DataColumn("last_name", typeof(string)),
                new DataColumn("phone_number", typeof(string)),
                new DataColumn("address", typeof(string)),
                new DataColumn("city_name", typeof(string)),
                new DataColumn("district_id", typeof(int)),
                new DataColumn("state_id", typeof(int)),
                new DataColumn("landmark", typeof(string)),
                new DataColumn("pincode", typeof(string)),
                new DataColumn("is_for_other", typeof(string)),
                new DataColumn("latitude", typeof(string)),
                new DataColumn("longitude", typeof(string)),
                new DataColumn("otp", typeof(string)),
                new DataColumn("IsPaid", typeof(string))
        });

            foreach (var objDC in _objects.PackageList)
            {
                package_list.Rows.Add(
                    objDC.MainProductId,
                    objDC.PackageId,
                    objDC.ProhitDurationId,
                    _objects.CustomerDetail.Cust_Id,
                    objDC.PackageTime,
                    objDC.PackageDate,
                    objDC.PackageMRP,
                    objDC.PackageDiscount,
                    objDC.PackageDiscountedPrice,
                    objDC.PersionofContactName,
                    objDC.PersionofContactNumber,
                    objDC.Purohit_Id,
                    objDC.FirstName,
                    objDC.LastName,
                    objDC.PhoneNumber,
                    objDC.Address,
                    objDC.CityName,
                    objDC.DistrictID,
                    objDC.StateID,
                    objDC.Landmark,
                    objDC.Pincode,
                    objDC.IsForOther,
                    objDC.Latitude,
                    objDC.Longitude,
                    GenerateRandom.GenerateRandomOTP(),
                    objDC.IsPaid
                );
            }

            string orderNumber = GetOrderNumber();

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@order_number", orderNumber, SqlDbType.VarChar);
            param[1] = new DbParam("@cust_id", _objects.CustomerDetail.Cust_Id, SqlDbType.Int);
            param[2] = new DbParam("@IsAddToCart", _objects.IsAddToCart, SqlDbType.VarChar);
            param[3] = new DbParam("@tbl_package_details", package_list, SqlDbType.Structured);
            DataSet ds = Db.GetDataSet("usp_save_package_order", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                responces.result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                responces.booking_id = Db.ToInteger(ds.Tables[0].Rows[0]["booking_id"]);
                responces.order_number = Db.ToString(ds.Tables[0].Rows[0]["order_number"]);
            }

            return responces;
        }

        public Responce SaveIndependentAds(IndependentAdsDetails _objects)
        {
            Responce responces = new Responce();
            DataTable puja_list = new DataTable();
            puja_list.Columns.AddRange(new DataColumn[23]
            {
                new DataColumn("trn_ads_id", typeof(int)),
                new DataColumn("user_id", typeof(int)),
                new DataColumn("time_slot_id", typeof(int)),
                new DataColumn("puja_date", typeof(string)),
                new DataColumn("puja_mrp", typeof(string)),
                new DataColumn("puja_discount", typeof(string)),
                new DataColumn("puja_discount_price", typeof(string)),
                new DataColumn("purohit_id", typeof(int)),
                new DataColumn("first_name", typeof(string)),
                new DataColumn("last_name", typeof(string)),
                new DataColumn("phone", typeof(string)),
                new DataColumn("address", typeof(string)),
                new DataColumn("email", typeof(string)),
                new DataColumn("city_name", typeof(string)),
                new DataColumn("district_id", typeof(int)),
                new DataColumn("state_id", typeof(int)),
                new DataColumn("landmark", typeof(string)),
                new DataColumn("pincode", typeof(string)),
                new DataColumn("is_for_other", typeof(string)),
                new DataColumn("latitude", typeof(string)),
                new DataColumn("longitude", typeof(string)),
                new DataColumn("otp", typeof(string)),
                new DataColumn("IsPaid", typeof(string))
        });

            foreach (IndependentAds objDC in _objects.IndependentAdsList)
            {
                   puja_list.Rows.Add(
                    objDC.TrnAdsId,
                    _objects.CustomerDetail.Cust_Id,
                    objDC.PujaTime,
                    objDC.PujaDate,
                    objDC.PujaMRP,
                    objDC.PujaDiscount,
                    objDC.PujaDiscountedPrice,
                    objDC.PurohitID,
                    objDC.FirstName,
                    objDC.LastName,
                    objDC.Phone,
                    objDC.Address,
                    objDC.Email,
                    objDC.CityName,
                    objDC.DistrictID,
                    objDC.StateID,
                    objDC.Landmark,
                    objDC.Pincode,
                    objDC.IsForOther,
                    objDC.Latitude,
                    objDC.Longitude,
                    GenerateRandom.GenerateRandomOTP(),
                    objDC.IsPaid
                );
            }

            string orderNumber = GetOrderNumber();

            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@order_number", orderNumber, SqlDbType.VarChar);
            param[1] = new DbParam("@cust_id", _objects.CustomerDetail.Cust_Id, SqlDbType.Int);
            param[2] = new DbParam("@IsAddToCart", _objects.IsAddToCart, SqlDbType.VarChar);
            param[3] = new DbParam("@tbl_independentAds", puja_list, SqlDbType.Structured);
            DataSet ds = Db.GetDataSet("usp_save_independent_ads_order", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                    responces.result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                    responces.booking_id = Db.ToInteger(ds.Tables[0].Rows[0]["booking_id"]);
                    responces.order_number = Db.ToString(ds.Tables[0].Rows[0]["order_number"]);
            }

            return responces;
        }

        public MyBookingsRes GetProhitBookings(int prohit_id)
        {
            MyBookingsRes bookings = new MyBookingsRes();
            List<MyBookings> upcomingBookings = new List<MyBookings>();
            List<MyBookings> completedBookings = new List<MyBookings>();
            List<MyBookings> cancelledBookings = new List<MyBookings>();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@prohit_id", prohit_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_my_bookings_prohit", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PujaBooking puja = new PujaBooking();
                        puja.BookingID = Db.ToInteger(row["BookingID"]);
                        puja.OrderNumber = Db.ToString(row["OrderNumber"]);
                        puja.PujaID = Db.ToInteger(row["PujaID"]);
                        puja.PujaName = Db.ToString(row["PujaName"]);
                        puja.BookingDate = Db.ToString(row["PujaDate"]);
                        puja.PujaTime = Db.ToString(row["TimeSlot"]);
                        puja.PujaMRP = Db.ToString(row["PujaMRP"]);
                        puja.PujaDiscount = Db.ToString(row["PujaDiscount"]);
                        puja.PujaDiscountedPrice = Db.ToString(row["PujaDiscountedPrice"]);
                        puja.IsSamagri = Db.ToString(row["WithSamagri"]);
                        puja.SamagriMRP = Db.ToString(row["SamagriMRP"]);
                        puja.SamagriDiscount = Db.ToString(row["SamagriDiscount"]);
                        puja.SamagriDiscountedPrice = Db.ToString(row["SamagriDiscountedPrice"]);
                        puja.PurohitID = Db.ToInteger(row["PurohitID"]);
                        puja.UserID = Db.ToInteger(row["UserID"]);
                        puja.Paid = Db.ToString(row["Paid"]);
                        puja.PurohitName = Db.ToString(row["PurohitName"]);
                        puja.OrderStatus = Db.ToString(row["OrderStatus"]);
                        puja.OTP = Db.ToString(row["OTP"]);
                        puja.OTPConfirm = Db.ToString(row["OTPConfirm"]);
                        puja.PurohitRating = Db.ToString(row["Rating"]);
                        puja.WhatsApp = Db.ToString(row["WhatsApp"]);
                        puja.Skype = Db.ToString(row["Skype"]);
                        puja.BookingType = Db.ToString(row["BookingType"]);
                        puja.EPujaEmail = Db.ToString(row["EPujaEmail"]);
                        puja.EPujaPhoneNo = Db.ToString(row["EPujaPhoneNo"]);
                        puja.PersionofContactName = Db.ToString(row["PersionofContactName"]);
                        puja.PersionofContactNumber = Db.ToString(row["PersionofContactNumber"]);


                        #region rzp_payment
                        puja.status = Db.ToString(row["status"]);
                        puja.RazorPayDate = CommonMethods.FormatDate(Db.ToString(row["RazorPayDate"]), "yyyy-MM-dd", "dd-MM-yyyy");
                        puja.payment_status = Db.ToString(row["payment_status"]);
                        puja.rzp_amount = Db.ToDecimal(row["rzp_amount"]);
                        puja.RemainingAmount = Db.ToDecimal(row["remaining_amount"]);
                        puja.TotalAmount = decimal.Add(puja.rzp_amount, puja.RemainingAmount);
                        #endregion


                        CustomerBooking cust = new CustomerBooking();
                        cust.CustName = Db.ToString(row["CustName"]);
                        cust.CustPhone = Db.ToString(row["CustPhone"]);
                        cust.CustAddress = Db.ToString(row["CustAddress"]);
                        cust.CustCity = Db.ToString(row["CustCity"]);
                        cust.CustPincode = Db.ToString(row["CustPincode"]);
                        cust.CustState = Db.ToString(row["CustState"]);

                        MyBookings booking = new MyBookings();
                        booking.OrderDetails = puja;
                        booking.CustomerDetails = cust;

                        if (puja.OrderStatus.ToLower().Equals("upcoming"))
                        {
                            upcomingBookings.Add(booking);
                        }
                        if (puja.OrderStatus.ToLower().Equals("completed"))
                        {
                            completedBookings.Add(booking);
                        }
                        if (puja.OrderStatus.ToLower().Equals("cancelled"))
                        {
                            cancelledBookings.Add(booking);
                        }
                    }
                }
            }
            bookings.UpcomingBookings = upcomingBookings;
            bookings.CompletedBookings = completedBookings;
            bookings.CancelledBookings = cancelledBookings;

            return bookings;
        }
        public MyBookingsRes GetMyBookings(MyBookingsReq objBookings)
        {
            MyBookingsRes bookings = new MyBookingsRes();
            List<MyBookings> upcomingBookings = new List<MyBookings>();
            List<MyBookings> completedBookings = new List<MyBookings>();
            List<MyBookings> cancelledBookings = new List<MyBookings>();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", objBookings.UserID, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_my_bookings", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PujaBooking puja = new PujaBooking();
                        puja.BookingID = Db.ToInteger(row["BookingID"]);
                        puja.OrderNumber = Db.ToString(row["OrderNumber"]);
                        puja.PujaID = Db.ToInteger(row["PujaID"]);
                        puja.PujaName = Db.ToString(row["PujaName"]);
                        puja.BookingDate = Db.ToString(row["PujaDate"]);
                        puja.PujaTime = Db.ToString(row["TimeSlot"]);
                        puja.PujaMRP = Db.ToString(row["PujaMRP"]);
                        puja.PujaDiscount = Db.ToString(row["PujaDiscount"]);
                        puja.PujaDiscountedPrice = Db.ToString(row["PujaDiscountedPrice"]);
                        puja.IsSamagri = Db.ToString(row["WithSamagri"]);
                        puja.SamagriMRP = Db.ToString(row["SamagriMRP"]);
                        puja.SamagriDiscount = Db.ToString(row["SamagriDiscount"]);
                        puja.SamagriDiscountedPrice = Db.ToString(row["SamagriDiscountedPrice"]);
                        puja.PurohitID = Db.ToInteger(row["PurohitID"]);
                        puja.UserID = Db.ToInteger(row["UserID"]);
                        puja.Paid = Db.ToString(row["Paid"]);
                        puja.PurohitName = Db.ToString(row["PurohitName"]);
                        puja.OrderStatus = Db.ToString(row["OrderStatus"]);
                        puja.OTP = Db.ToString(row["OTP"]);
                        puja.OTPConfirm = Db.ToString(row["OTPConfirm"]);
                        puja.PurohitRating = Db.ToString(row["Rating"]);
                        puja.WhatsApp = Db.ToString(row["WhatsApp"]);
                        puja.Skype = Db.ToString(row["Skype"]);
                        puja.BookingType = Db.ToString(row["BookingType"]);
                        puja.EPujaEmail = Db.ToString(row["EPujaEmail"]);
                        puja.EPujaPhoneNo = Db.ToString(row["EPujaPhoneNo"]);
                        puja.PersionofContactName = Db.ToString(row["PersionofContactName"]);
                        puja.PersionofContactNumber = Db.ToString(row["PersionofContactNumber"]);

                        #region rzp_payment
                        puja.status = Db.ToString(row["status"]);
                        puja.RazorPayDate = CommonMethods.FormatDate(Db.ToString(row["RazorPayDate"]), "yyyy-MM-dd", "dd-MM-yyyy");
                        puja.payment_status = Db.ToString(row["payment_status"]);
                        puja.rzp_amount = Db.ToDecimal(row["rzp_amount"]);
                        puja.RemainingAmount = Db.ToDecimal(row["remaining_amount"]);
                        puja.TotalAmount = decimal.Add(puja.rzp_amount, puja.RemainingAmount);
                        #endregion

                        CustomerBooking cust = new CustomerBooking();
                        cust.CustName = Db.ToString(row["CustName"]);
                        cust.CustPhone = Db.ToString(row["CustPhone"]);
                        cust.CustAddress = Db.ToString(row["CustAddress"]);
                        cust.CustCity = Db.ToString(row["CustCity"]);
                        cust.CustPincode = Db.ToString(row["CustPincode"]);
                        cust.CustState = Db.ToString(row["CustState"]);

                        MyBookings booking = new MyBookings();
                        booking.OrderDetails = puja;
                        booking.CustomerDetails = cust;

                        if (puja.OrderStatus.ToLower().Equals("upcoming"))
                        {
                            upcomingBookings.Add(booking);
                        }
                        if (puja.OrderStatus.ToLower().Equals("completed"))
                        {
                            completedBookings.Add(booking);
                        }
                        if (puja.OrderStatus.ToLower().Equals("cancelled"))
                        {
                            cancelledBookings.Add(booking);
                        }
                    }
                }
            }
            bookings.UpcomingBookings = upcomingBookings;
            bookings.CompletedBookings = completedBookings;
            bookings.CancelledBookings = cancelledBookings;

            return bookings;
        }
        public MyCartResponce MyCart(int user_id)
        {

            var bookings = new MyCartResponce();
            var MyCart = new List<MyBookings>();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", user_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_get_MyCart_Bookings", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var puja = new PujaBooking();
                        puja.BookingID = Db.ToInteger(row["BookingID"]);
                        puja.OrderNumber = Db.ToString(row["OrderNumber"]);
                        puja.PujaID = Db.ToInteger(row["PujaID"]);
                        puja.PujaName = Db.ToString(row["PujaName"]);
                        puja.BookingDate = Db.ToString(row["PujaDate"]);
                        puja.PujaTime = Db.ToString(row["TimeSlot"]);
                        puja.PujaMRP = Db.ToString(row["PujaMRP"]);
                        puja.PujaDiscount = Db.ToString(row["PujaDiscount"]);
                        puja.PujaDiscountedPrice = Db.ToString(row["PujaDiscountedPrice"]);
                        puja.IsSamagri = Db.ToString(row["WithSamagri"]);
                        puja.SamagriMRP = Db.ToString(row["SamagriMRP"]);
                        puja.SamagriDiscount = Db.ToString(row["SamagriDiscount"]);
                        puja.SamagriDiscountedPrice = Db.ToString(row["SamagriDiscountedPrice"]);
                        puja.PurohitID = Db.ToInteger(row["PurohitID"]);
                        puja.UserID = Db.ToInteger(row["UserID"]);
                        puja.Paid = Db.ToString(row["Paid"]);
                        puja.PurohitName = Db.ToString(row["PurohitName"]);
                        puja.OrderStatus = Db.ToString(row["OrderStatus"]);
                        puja.OTP = Db.ToString(row["OTP"]);
                        puja.OTPConfirm = Db.ToString(row["OTPConfirm"]);
                        puja.PurohitRating = Db.ToString(row["Rating"]);
                        puja.WhatsApp = Db.ToString(row["WhatsApp"]);
                        puja.Skype = Db.ToString(row["Skype"]);
                        puja.BookingType = Db.ToString(row["BookingType"]);
                        puja.EPujaEmail = Db.ToString(row["EPujaEmail"]);
                        puja.EPujaPhoneNo = Db.ToString(row["EPujaPhoneNo"]);
                        puja.PersionofContactName = Db.ToString(row["PersionofContactName"]);
                        puja.PersionofContactNumber = Db.ToString(row["PersionofContactNumber"]);

                        var customer = new CustomerBooking();
                        customer.CustName = Db.ToString(row["CustName"]);
                        customer.CustPhone = Db.ToString(row["CustPhone"]);
                        customer.CustAddress = Db.ToString(row["CustAddress"]);
                        customer.CustCity = Db.ToString(row["CustCity"]);
                        customer.CustPincode = Db.ToString(row["CustPincode"]);
                        customer.CustState = Db.ToString(row["CustState"]);

                        MyBookings booking = new MyBookings();
                        booking.OrderDetails = puja;
                        booking.CustomerDetails = customer;
                        MyCart.Add(booking);
                    }
                }
            }

            bookings.MyCart = MyCart;
            return bookings;
        }
        private string GetOrderNumber()
        {
            string orderNumber = "";

            orderNumber = Guid.NewGuid().ToString();
            orderNumber = orderNumber.Replace("-", "");
            orderNumber = orderNumber.Substring(0, 20);

            return orderNumber;
        }
        public string BookingCancel(Responce request, int user_id)
        {
            string result = string.Empty;
            DbParam[] param = new DbParam[5];
            param[0] = new DbParam("@booking_id", request.booking_id, SqlDbType.Int);
            param[1] = new DbParam("@order_number", request.order_number, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            param[3] = new DbParam("@reject_by_prohit", request.RejectByProhit, SqlDbType.NVarChar);
            param[4] = new DbParam("@ression", request.Message, SqlDbType.NVarChar);
            DataSet ds = Db.GetDataSet("usp_cancel_my_bookings", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                }
            }
            return result;
        }
        public string BookingConfirm(Responce request, int user_id)
        {
            string result = string.Empty;
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@booking_id", request.booking_id, SqlDbType.Int);
            param[1] = new DbParam("@order_number", request.order_number, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_confirm_my_bookings", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                }
            }
            return result;
        }
        public BookingOrderModel GetBookingsStatus(string order_number)
        {
            var result = new BookingOrderModel();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@order_number", order_number, SqlDbType.NVarChar);
            DataSet ds = Db.GetDataSet("usp_get_order_number_bookings", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result.BookingID = Db.ToInteger(ds.Tables[0].Rows[0]["BookingID"]);
                result.OrderNumber = Db.ToString(ds.Tables[0].Rows[0]["OrderNumber"]);
                result.PujaID = Db.ToInteger(ds.Tables[0].Rows[0]["PujaID"]);
                result.PujaName = Db.ToString(ds.Tables[0].Rows[0]["PujaName"]);
                result.BookingDate = Db.ToString(ds.Tables[0].Rows[0]["PujaDate"]);
                result.PujaTime = Db.ToString(ds.Tables[0].Rows[0]["TimeSlot"]);
                result.PurohitID = Db.ToInteger(ds.Tables[0].Rows[0]["PurohitID"]);
                result.UserID = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                result.BookingType = Db.ToString(ds.Tables[0].Rows[0]["BookingType"]);
                result.PurohitName = Db.ToString(ds.Tables[0].Rows[0]["PurohitName"]);
                result.status = Db.ToString(ds.Tables[0].Rows[0]["status"]);
                result.RazorPayDate = CommonMethods.FormatDate(Db.ToString(ds.Tables[0].Rows[0]["RazorPayDate"]), "yyyy-MM-dd", "dd-MM-yyyy");
                result.payment_status = Db.ToString(ds.Tables[0].Rows[0]["payment_status"]);
                result.PaymentMode = Db.ToString(ds.Tables[0].Rows[0]["PaymentMode"]);
                result.PaymentDetails = Db.ToString(ds.Tables[0].Rows[0]["PaymentDetails"]);
                result.rzp_amount = Db.ToDecimal(ds.Tables[0].Rows[0]["rzp_amount"]);
                result.RemainingAmount = Db.ToDecimal(ds.Tables[0].Rows[0]["remaining_amount"]);
                result.TotalAmount = decimal.Add(result.rzp_amount, result.RemainingAmount);
                result.CustName = Db.ToString(ds.Tables[0].Rows[0]["CustName"]);
                result.CustPhone = Db.ToString(ds.Tables[0].Rows[0]["CustPhone"]);
                result.CustEmail = Db.ToString(ds.Tables[0].Rows[0]["CustEmail"]);
                result.CustAddress = Db.ToString(ds.Tables[0].Rows[0]["CustAddress"]);
                result.CustCity = Db.ToString(ds.Tables[0].Rows[0]["CustCity"]);
                result.CustPincode = Db.ToString(ds.Tables[0].Rows[0]["CustPincode"]);
                result.CustState = Db.ToString(ds.Tables[0].Rows[0]["CustState"]);
            }
            return result;
        }

        public PujaBooking GetBookings(string order_number)
        {
            var result = new PujaBooking();
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@order_number", order_number, SqlDbType.NVarChar);
            DataSet ds = Db.GetDataSet("usp_get_order_number_bookings", param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result.BookingID = Db.ToInteger(ds.Tables[0].Rows[0]["BookingID"]);
                result.OrderNumber = Db.ToString(ds.Tables[0].Rows[0]["OrderNumber"]);
                result.PujaID = Db.ToInteger(ds.Tables[0].Rows[0]["PujaID"]);
                result.PujaName = Db.ToString(ds.Tables[0].Rows[0]["PujaName"]);
                result.BookingDate = Db.ToString(ds.Tables[0].Rows[0]["PujaDate"]);
                result.PujaTime = Db.ToString(ds.Tables[0].Rows[0]["TimeSlot"]);
                result.PujaMRP = Db.ToString(ds.Tables[0].Rows[0]["PujaMRP"]);
                result.PujaDiscount = Db.ToString(ds.Tables[0].Rows[0]["PujaDiscount"]);
                result.PujaDiscountedPrice = Db.ToString(ds.Tables[0].Rows[0]["PujaDiscountedPrice"]);
                result.IsSamagri = Db.ToString(ds.Tables[0].Rows[0]["WithSamagri"]);
                result.SamagriMRP = Db.ToString(ds.Tables[0].Rows[0]["SamagriMRP"]);
                result.SamagriDiscount = Db.ToString(ds.Tables[0].Rows[0]["SamagriDiscount"]);
                result.SamagriDiscountedPrice = Db.ToString(ds.Tables[0].Rows[0]["SamagriDiscountedPrice"]);
                result.PurohitID = Db.ToInteger(ds.Tables[0].Rows[0]["PurohitID"]);
                result.UserID = Db.ToInteger(ds.Tables[0].Rows[0]["UserID"]);
                result.Paid = Db.ToString(ds.Tables[0].Rows[0]["Paid"]);
                result.PurohitName = Db.ToString(ds.Tables[0].Rows[0]["PurohitName"]);
                result.OrderStatus = Db.ToString(ds.Tables[0].Rows[0]["OrderStatus"]);
                result.OTP = Db.ToString(ds.Tables[0].Rows[0]["OTP"]);
                result.OTPConfirm = Db.ToString(ds.Tables[0].Rows[0]["OTPConfirm"]);
                result.PurohitRating = Db.ToString(ds.Tables[0].Rows[0]["Rating"]);
                result.WhatsApp = Db.ToString(ds.Tables[0].Rows[0]["WhatsApp"]);
                result.Skype = Db.ToString(ds.Tables[0].Rows[0]["Skype"]);
                result.BookingType = Db.ToString(ds.Tables[0].Rows[0]["BookingType"]);
                result.EPujaEmail = Db.ToString(ds.Tables[0].Rows[0]["EPujaEmail"]);
                result.EPujaPhoneNo = Db.ToString(ds.Tables[0].Rows[0]["EPujaPhoneNo"]);
                result.PersionofContactName = Db.ToString(ds.Tables[0].Rows[0]["PersionofContactName"]);
                result.PersionofContactNumber = Db.ToString(ds.Tables[0].Rows[0]["PersionofContactNumber"]);

            }
            return result;
        }
        public string DeleteCartItem(Responce request, int user_id)
        {
            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@booking_id", request.booking_id, SqlDbType.Int);
            param[1] = new DbParam("@order_number", request.order_number, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            DataSet ds = Db.GetDataSet("usp_delete_my_cart", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    request.result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                }
            }
            return request.result;
        }
        public string CheckoutMyCart(List<CheckOutResponce> request, int user_id)
        {
            string result = string.Empty;
            DataTable _cartlist = new DataTable();
            _cartlist.Columns.AddRange(new DataColumn[2]
            {
                new DataColumn("BookingID", typeof(int)),
                new DataColumn("OrderNumber", typeof(string))
            });

            foreach (var item in request)
            {
                _cartlist.Rows.Add(item.BookingID, item.OrderNumber);
            }

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@user_id", user_id, SqlDbType.Int);
            param[1] = new DbParam("@tbl_chekout_mycart", _cartlist, SqlDbType.Structured);
            DataSet ds = Db.GetDataSet("usp_checkout_my_cart", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                }
            }
            return result;
        }
        public string BookingVerification(OTPRequest request)
        {
            string result = string.Empty;
            DataTable _cartlist = new DataTable();

            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@booking_id", request.BookingId, SqlDbType.Int);
            param[1] = new DbParam("@order_number", request.OrderNumber, SqlDbType.NVarChar);
            param[2] = new DbParam("@OTP", request.OTP.Trim(), SqlDbType.NVarChar);

            DataSet ds = Db.GetDataSet("usp_my_booking_otp_verification", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["result"]);
                }
            }
            return result;
        }


        public string GetservicesName(int ServiceID)
        {
            string result = "";
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@ServiceID", ServiceID, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_mst_services_name", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["name"]);
                }
            }
            return result;
        }
        public string GetProductName(int PujaID)
        {
            string result = "";
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@PujaID", PujaID, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_main_product_name", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["name"]);
                }
            }
            return result;
        }
        public string GetIndependentAdsName(int id)
        {
            string result = "";
            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@ID", id, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_trn_ads_name", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = Db.ToString(ds.Tables[0].Rows[0]["name"]);
                }
            }
            return result;
        }
        

        public PaymentStatus BookingOrderStatus(CheckOutResponce r)
        {
            var status = new PaymentStatus();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@booking_id", r.BookingID, SqlDbType.Int);
            param[1] = new DbParam("@order_number", r.OrderNumber, SqlDbType.NVarChar);
            var ds = Db.GetDataSet("usp_booking_order_status", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    status.paymentStatus = Db.ToString(ds.Tables[0].Rows[0]["paymentStatus"]);
                }
            }
            return status;
        }
    }
}
