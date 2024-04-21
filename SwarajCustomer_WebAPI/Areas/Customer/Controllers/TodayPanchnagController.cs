using SwarajCustomer_Common;
using SwarajCustomer_WebAPI.Areas.Customer.Models;
using SwarajCustomer_WebAPI.Authorization;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.Customer.Controllers
{
	[RoutePrefix("TodayPanchnag")]
	[SessionFilterAttribute]
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
	public class TodayPanchnagController : Controller
	{
		public static string PanchangUrl = Convert.ToString(WebConfigurationManager.AppSettings.Get("PanchangUrl"));

		[HttpGet]
		public ActionResult Index()
		{
			GetBaseUrl();
			return View();
		}

		[HttpGet]
		public ActionResult _Index(string date, string time)
        {
            GetBaseUrl();
            date = CommonMethods.FormatDate(date, "dd-MM-yyyy", "dd/MM/yyyy");
            var webClient = new WebClient();
            var url = Panchang(date, time, webClient);
            var jsonData = webClient.DownloadData(url);

            var ser = new DataContractJsonSerializer(typeof(TodayPanchnag));
            var rootObject = (TodayPanchnag)ser.ReadObject(new MemoryStream(jsonData));
            var dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            rootObject.reqdate = dt.ToString("dddd, dd MMMM yyyy");
            return View("_Index", rootObject);
        }

        private static string Panchang(string date, string time, WebClient webClient)
        {
            var url = string.Format(PanchangUrl + "&date=" + date + "&time=" + time + "&tz=05:30&userid=jatinde&authcode=c9a8b3be9c5d052bb34e2f74fcf4fd20");
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            return url;
        }

        private void GetBaseUrl()
		{
			CommonMethods.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
		}
	}
}