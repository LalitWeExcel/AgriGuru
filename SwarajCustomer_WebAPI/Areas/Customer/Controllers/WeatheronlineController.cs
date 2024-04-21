using SwarajCustomer_BAL.Interface.ManageUser;
using SwarajCustomer_Common;
using SwarajCustomer_Common.Utility;
using SwarajCustomer_WebAPI.Areas.Customer.Models;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
    [RoutePrefix("Weatheronline")]
	[SessionFilterAttribute]
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
	public class WeatheronlineController : Controller
	{
		//https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
		public static string Url = Convert.ToString(WebConfigurationManager.AppSettings.Get("WorldWeatherOnLineUrl"));
		public static string Key = Convert.ToString(WebConfigurationManager.AppSettings.Get("WorldWeatherOnLineKey"));

		private IManageUserBAL _manageUserService ;

		[HttpGet]
		public ActionResult Index()
		{
			_manageUserService = new ManageUserBAL();
			GetBaseUrl();
			double Latitude = (double)Session[SystemVariables.Latitude];
			double Longitude = (double)Session[SystemVariables.Longitude];

			var currentLocation = string.Format(Url + Latitude + "%2C" + Longitude + "&num_of_days=7&format=json&tp=1&lang=&key=" + Key);

			var webClient = new WebClient();
			webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
			var jsonData = webClient.DownloadData(currentLocation);
			var ser = new DataContractJsonSerializer(typeof(Weatheronlineobject));
			var rootObject = (Weatheronlineobject)ser.ReadObject(new MemoryStream(jsonData));

			var xx = GoogleLocation.GetAddressByLatLong(Latitude, Longitude).address;
			rootObject.Location = xx.suburb + "," + xx.state_district + "," + xx.state;
			rootObject.DateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");

		
			return View(rootObject);
		}
		private void GetBaseUrl()
		{
			CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
		}

	}
}