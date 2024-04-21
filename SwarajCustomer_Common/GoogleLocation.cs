using GoogleMaps.LocationServices;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Configuration;

namespace SwarajCustomer_Common
{


    public  class GoogleLocation
    {

        public static string GoogleMapAPIKey = WebConfigurationManager.AppSettings.Get("GoogleMapAPIKey");
        public static string OpenStreetMapUrl = WebConfigurationManager.AppSettings.Get("OpenStreetMapUrl");


        //https://stackoverflow.com/questions/14354867/how-to-find-latitude-and-longitude-using-c-sharp
        //https://dotnetexpertteam.wordpress.com/2016/10/20/get-latitude-and-longitude-geocode-using-c/
        public static LatLongLongitude GetLatLongByAddress(string address)
        {
            var result = new LatLongLongitude();
            var locationService = new GoogleLocationService(GoogleMapAPIKey);
            var point = locationService.GetLatLongFromAddress(address);

            result.latitude = point.Latitude;
            result.longitude = point.Longitude;
            return result;
        }


        public static RootObject GetAddressByLatLong(double latitude, double longitude)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            var jsonData = webClient.DownloadData(OpenStreetMapUrl + "&lat=" + latitude + "&lon=" + longitude);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            //  you are modify own requirment
            return rootObject;

        }

        public class LatLongLongitude
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }
        public class Address
        {
            public string road { get; set; }
            public string suburb { get; set; }
            public string city { get; set; }
            public string state_district { get; set; }
            public string state { get; set; }
            public string postcode { get; set; }
            public string country { get; set; }
            public string country_code { get; set; }
        }
        public class RootObject
        {
            public string place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public string osm_id { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
            public Address address { get; set; }
        }

    }
}
