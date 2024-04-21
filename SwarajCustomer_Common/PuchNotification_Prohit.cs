using SwarajCustomer_Common.Entities;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace SwarajCustomer_Common
{
    public  class PuchNotification_Prohit
    {
        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));

        #region push notification for  Prohit Astrologer booking Confrim By Admin 
        public static void Prohit_Astrologer_booking_Confrim_By_Admin_Notification(NotificationEnitity notification, Responce responce)
        {
            var ndm = new Notification_Data_Message();
            ndm.NotificationId = notification.contentsId;
            ndm.Info = responce.Message;
            var noti = new NotificationEnitity();
            noti.Details = ndm;
            noti.Type = 1;
            noti.RecordType = Convert.ToInt32(notification.contentsType);

            noti.Title = notification.contents;
            noti.BookingId = responce.booking_id;
            noti.OrderNumber = responce.order_number;

            System.Threading.Thread thsendgcm = new System.Threading.Thread(new System.Threading.ThreadStart(() => Prohit_Astrologer_booking_Confrim_By_Admin_Notification(notification.device_key, noti)));
            thsendgcm.Start();
        }
        public static string Prohit_Astrologer_booking_Confrim_By_Admin_Notification(string deviceKey, NotificationEnitity pn)
        {
            try
            {

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.UseDefaultCredentials = true;
                tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var json = "";
                var value = new JavaScriptSerializer().Serialize(pn);

                var title = pn.Title;
                var pnDetails = pn.Details;
                var Info = pnDetails.GetType().GetProperty("Info") != null ? pnDetails.GetType().GetProperty("Info").GetValue(pnDetails, null) : "";
                var NotificationId = pnDetails.GetType().GetProperty("NotificationId") != null ? pnDetails.GetType().GetProperty("NotificationId").GetValue(pnDetails, null) : "";

                if (pn.Type == 1)
                {
                    string str = "{\"body\"" + ":" + "\"" + Info + "\"" + ","
                         + "\"NotificationId\"" + ":" + "\"" + NotificationId + "\"" + ","
                         + "\"date\"" + ":" + "\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"" + ","
                         + "\"title\"" + ":" + "\"" + pn.Title + "\"" + ","
                         + "\"BookingId\"" + ":" + "\"" + pn.BookingId + "\"" + ","
                         + "\"OrderNumber\"" + ":" + "\"" + pn.OrderNumber + "\"" + ","
                         + "\"record_type\"" + ":" + "\"" + pn.RecordType + "\"" + ","
                         + "\"time\"" + ":" + "\"" + System.DateTime.Now.TimeOfDay.ToString() + "\"}";

                    var data = new
                    {
                        to = deviceKey,
                        data = new
                        {
                            body = str
                        }
                    };
                    json = new JavaScriptSerializer().Serialize(data);
                }
                else  //For iPhone
                {
                    var data = new
                    {
                        to = deviceKey,
                        data = new
                        {
                            //Push message
                            Message = Info,
                            Title = "",
                            PushId = "1"
                        }
                    };
                    json = new JavaScriptSerializer().Serialize(data);
                }

 
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationIdProhit));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderIdProhit));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return "true";
        }
        #endregion

    }
}
