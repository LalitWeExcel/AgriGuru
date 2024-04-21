using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Purohit;
using System;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface
{
    public interface IBookingBAL
    {
        List<TimeSlotMaster> GetPujaBookedSlots(int pujaID, DateTime pujaDate);

        List<TimeSlotMaster> GetAstroBookedSlots(int astroID, DateTime astroDate);
        PaymentStatus BookingOrderStatus(CheckOutResponce check);
        Responce SavePujaOrder(PujaOrderDetails objPuja);

        Responce SaveAstroOrder(AstroOrderDetails objAstro);
        Responce SavePackageOrder(PackageOrderDetails _object);
        Responce SaveIndependentAds(IndependentAdsDetails _object);
        string BookingCancel(Responce obj, int user_id);
        string DeleteCartItem(Responce obj, int user_id);
        string BookingConfirm(Responce obj, int user_id);
        
        MyBookingsRes GetMyBookings(MyBookingsReq objBookings);
        MyBookingsRes GetProhitBookings(int prohit_id);
        PujaBooking GetBookings(string order_number);
        BookingOrderModel GetBookingsStatus(string order_number);

        MyCartResponce MyCart(int user_id);
        string CheckoutMyCart(List<CheckOutResponce> request, int user_id);

        string GetservicesName(int ServiceID);
        string GetProductName(int PujaID);
        string GetIndependentAdsName(int ID);

        #region  Prohit App 
        string BookingVerification(OTPRequest request);
        #endregion
    }
}
