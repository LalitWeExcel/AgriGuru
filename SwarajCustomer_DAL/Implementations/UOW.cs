using SwarajCustomer_DAL.Interface;
using SwarajCustomer_DAL.EDMX;
using System;
using SwarajCustomer_DAL.Interface.DashBoard;
using SwarajCustomer_DAL.Interface.ManageUser;
using SwarajCustomer_DAL.Interface.Master;
using SwarajCustomer_DAL.Interface.Advertising;
using SwarajCustomer_DAL.Interface.ManageOrder;
using SwarajCustomer_DAL.Interface.ManagePayment;

namespace SwarajCustomer_DAL.Implementations
{
    public class UOW : IUOW, IDisposable
    {
        private SwarajTestEntities context = new SwarajTestEntities();
        private bool _disposed = false;
        private ILoginDAL _LoginDAL;
        private IUserDAL _UserDAL;
        private IBookingDAL _BookingDAL;
        private ISearchDAL _SearchDAL;
        private IContactDAL _ContactDAL;
        private IFeedBackDAL _FeedBackDAL;
        private INotificationsDAL _NotificationsDAL;


        // WEB SERVICE
        private IDashBoardDAL _DashBoardDAL;
        private IManageUserDAL _ManageUserDAL;
        private IMastersDAL   _MastersDAL;
        private IAdvertisingDAL _AdvertisingDAL;
        private IManageOrderDAL _ManageOrderDAL;
        private IManagePaymentDAL _ManagePaymentDAL;
        

        public ILoginDAL LoginDALRepository
        {
            get
            {

                if (this._LoginDAL == null)
                {
                    this._LoginDAL = new LoginDAL(context);
                }
                return _LoginDAL;
            }
        }

        public IUserDAL UserDALRepository
        {
            get
            {

                if (this._UserDAL == null)
                {
                    this._UserDAL = new UserDAL(context);
                }
                return _UserDAL;
            }
        }

        public IBookingDAL BookingDALRepository
        {
            get
            {

                if (this._BookingDAL == null)
                {
                    this._BookingDAL = new BookingDAL(context);
                }
                return _BookingDAL;
            }
        }

        public ISearchDAL SearchDALRepository
        {
            get
            {

                if (this._SearchDAL == null)
                {
                    this._SearchDAL = new SearchDAL(context);
                }
                return _SearchDAL;
            }
        }

        public IContactDAL ContactDALRepository
        {
            get
            {

                if (this._ContactDAL == null)
                {
                    this._ContactDAL = new ContactDAL(context);
                }
                return _ContactDAL;
            }
        }

        public IFeedBackDAL FeedBackDALRepository
        {
            get
            {

                if (this._FeedBackDAL == null)
                {
                    this._FeedBackDAL = new FeedBackDAL(context);
                }
                return _FeedBackDAL;
            }
        }
        public INotificationsDAL NotificationsRepository
        {
            get
            {

                if (this._NotificationsDAL == null)
                {
                    this._NotificationsDAL = new NotificationsDAL(context);
                }
                return _NotificationsDAL;
            }
        }

        public IDashBoardDAL DashBoardRepository
        {
            get
            {

                if (this._DashBoardDAL == null)
                {
                    this._DashBoardDAL = new DashBoardDAL(context);
                }
                return _DashBoardDAL;
            }
        }
        public IManageUserDAL ManageUserRepository
        {
            get
            {

                if (this._ManageUserDAL == null)
                {
                    this._ManageUserDAL = new ManageUserDAL(context);
                }
                return _ManageUserDAL;
            }
        }

        public IMastersDAL MasterRepository
        {
            get
            {

                if (this._MastersDAL == null)
                {
                    this._MastersDAL = new MastersDAL(context);
                }
                return _MastersDAL;
            }
        }
        public IAdvertisingDAL AdvertisingRepository
        {
            get
            {

                if (this._AdvertisingDAL == null)
                {
                    this._AdvertisingDAL = new AdvertisingDAL(context);
                }
                return _AdvertisingDAL;
            }
        }
        public IManageOrderDAL ManageOrdeRepository
        {
            get
            {

                if (this._ManageOrderDAL == null)
                {
                    this._ManageOrderDAL = new ManageOrderDAL(context);
                }
                return _ManageOrderDAL;
            }
        }
        public IManagePaymentDAL ManagePaymentRepository
        {
            get
            {

                if (this._ManagePaymentDAL == null)
                {
                    this._ManagePaymentDAL = new ManagePaymentDAL(context);
                }
                return _ManagePaymentDAL;
            }
        }


        public void Commit()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing && context != null)
            {
                context.Dispose();
            }

            _disposed = true;
        }
    }
}
