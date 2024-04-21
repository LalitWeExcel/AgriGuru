using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_Common.Purohit;
using SwarajCustomer_DAL.Implementations;
using System;
using System.Collections.Generic;

namespace SwarajCustomer_BAL
{
    public class BookingBAL : IBookingBAL
    {
        private UOW unitOfWork = new UOW();

        public List<TimeSlotMaster> GetPujaBookedSlots(int pujaID, DateTime pujaDate)
        {
            return unitOfWork.BookingDALRepository.GetPujaBookedSlots(pujaID, pujaDate);
        }

        public List<TimeSlotMaster> GetAstroBookedSlots(int astroID, DateTime astroDate)
        {
            return unitOfWork.BookingDALRepository.GetAstroBookedSlots(astroID, astroDate);
        }
        public PaymentStatus BookingOrderStatus(CheckOutResponce e)
        {
            return unitOfWork.BookingDALRepository.BookingOrderStatus(e);
        }

        public Responce SavePujaOrder(PujaOrderDetails objPuja)
        {
            return unitOfWork.BookingDALRepository.SavePujaOrder(objPuja);
        }

        public Responce SaveAstroOrder(AstroOrderDetails objAstro)
        {
            return unitOfWork.BookingDALRepository.SaveAstroOrder(objAstro);
        }
        public Responce SavePackageOrder(PackageOrderDetails _object)
        {
            return unitOfWork.BookingDALRepository.SavePackageOrder(_object);
        }
        public Responce SaveIndependentAds(IndependentAdsDetails _object)
        {
            return unitOfWork.BookingDALRepository.SaveIndependentAds(_object);
        }

        public MyBookingsRes GetProhitBookings(int user_id)
        {
            return unitOfWork.BookingDALRepository.GetProhitBookings(user_id);
        }
       
        public MyBookingsRes GetMyBookings(MyBookingsReq objBookings)
        {
            return unitOfWork.BookingDALRepository.GetMyBookings(objBookings);
        }
        public PujaBooking GetBookings(string order_number)
        {
            return unitOfWork.BookingDALRepository.GetBookings(order_number);
        }
        public BookingOrderModel GetBookingsStatus(string order_number)
        {
            return unitOfWork.BookingDALRepository.GetBookingsStatus(order_number);
        }
      
        public string BookingCancel(Responce obj, int user_id)
        {
            return unitOfWork.BookingDALRepository.BookingCancel(obj, user_id);
        }
        public string BookingConfirm(Responce obj, int user_id)
        {
            return unitOfWork.BookingDALRepository.BookingConfirm(obj, user_id);
        }
    
        public string DeleteCartItem(Responce obj, int user_id)
        {
            return unitOfWork.BookingDALRepository.DeleteCartItem(obj, user_id);
        }
       
        public MyCartResponce MyCart(int user_id)
        {
            return unitOfWork.BookingDALRepository.MyCart(user_id);
        }
       public string CheckoutMyCart(List<CheckOutResponce> request, int user_id)
        {
            return unitOfWork.BookingDALRepository.CheckoutMyCart(request, user_id);
        }

        public string BookingVerification(OTPRequest request)
        {
            return unitOfWork.BookingDALRepository.BookingVerification(request);
        }

        public string GetservicesName(int ServiceID)
        {
            return unitOfWork.BookingDALRepository.GetservicesName(ServiceID);
        }
        public string GetProductName(int PujaID)
        {
            return unitOfWork.BookingDALRepository.GetProductName(PujaID);
        }
        public string GetIndependentAdsName(int id)
        {
            return unitOfWork.BookingDALRepository.GetIndependentAdsName(id);
        }
        
    }
}
