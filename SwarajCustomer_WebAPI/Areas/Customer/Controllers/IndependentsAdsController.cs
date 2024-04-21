﻿using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Customer;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
	[RoutePrefix("IndependentsAds")]
	[SessionFilterAttribute]
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
	public class IndependentsAdsController : Controller
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private INotificationsBAL _notifications = null;
		private IBookingBAL _bookingService = null;
		private IUserBAL _userService = null;


		[HttpGet] 
		public ActionResult Index(int TrnAdsId,string Name, string PujaMRP, string PujaDiscount, int DiscountInRupees, string PujaDiscountedPrice)
        {
			var model = new IndependentsAdsContent();
			GetBaseUrl();
			_userService = new UserBAL();
			_notifications = new NotificationsBAL();
			_bookingService = new BookingBAL();


			double Latitude = (double)Session[SystemVariables.Latitude];
			double Longitude = (double)Session[SystemVariables.Longitude];

			int user_Id = Convert.ToInt32(Session[SystemVariables.UserId]);
			Session[SystemVariables.M_Notifications] = _notifications.GetNotificationsByUser(user_Id);
			Session[SystemVariables.MyCart] = _bookingService.MyCart(user_Id);


			model.Masters = (SwarajCustomer_Common.Entities.Masters)Session[SystemVariables.Masters];
			model.Userprofile = _userService.GetUserProfile(user_Id);
			model.TrnAdsId = TrnAdsId;
			model.Name = Name;

			model.PujaMRP = PujaMRP;
			model.PujaDiscount = PujaDiscount;
			model.DiscountInRupees = DiscountInRupees;
			model.PujaDiscountedPrice = PujaDiscountedPrice;

			return View(model);
        }

		private void GetBaseUrl()
		{
			CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
		}
	}
}