using SwarajCustomer_Common.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace SwarajCustomer_Common
{
    public class CommonMethods
    {
        public static string BaseUrl { get; set; }

        public static string ConStr = ConfigurationManager.AppSettings["ConStr"];

        public static string FullDateIndia = Convert.ToString(WebConfigurationManager.AppSettings.Get("FullDateIndia"));
        public static string FullDateDB = Convert.ToString(WebConfigurationManager.AppSettings.Get("FullDateDB"));
        public static string MinDate = Convert.ToString(WebConfigurationManager.AppSettings.Get("MinDate"));
        public static string MinDateDB = Convert.ToString(WebConfigurationManager.AppSettings.Get("MinDateDB"));
        public static string DD_MM_YYYY = Convert.ToString(WebConfigurationManager.AppSettings.Get("DD_MM_YYYY"));

        public static string InvalidLogin = Convert.ToString(WebConfigurationManager.AppSettings.Get("InvalidLogin"));
        public static int CommandTimeoutExec = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeoutExec"]);
        //Email Settings
        public static string SMTPServer = Convert.ToString(WebConfigurationManager.AppSettings.Get("SMTPServer"));
        public static int SMTPPort = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("SMTPPort"));
        public static int SMTPTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("SMTPTimeout"));
        public static string FromEmail = Convert.ToString(WebConfigurationManager.AppSettings.Get("FromEmail"));
        public static string EmailCC = Convert.ToString(WebConfigurationManager.AppSettings.Get("EmailCC"));
        
        public static string EmailDisplayName = Convert.ToString(WebConfigurationManager.AppSettings.Get("EmailDisplayName"));
        public static string FromPassword = Convert.ToString(WebConfigurationManager.AppSettings.Get("FromPassword"));
        public static int EmailRetryAttempt = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("EmailRetryAttempt"));
        public static Boolean IsSSL = Convert.ToBoolean(WebConfigurationManager.AppSettings.Get("IsSSL"));

        public static string ApplicationIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdCust"));
        public static string SenderIdCust = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdCust"));

        public static string ApplicationIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("ApplicationIdProhit"));
        public static string SenderIdProhit = Convert.ToString(WebConfigurationManager.AppSettings.Get("SenderIdProhit"));


        public static string CheckoutMyCartHtmlPath = WebConfigurationManager.AppSettings.Get("CheckoutMyCartHtmlPath");
        public static string SaveOrderHtmlPath = WebConfigurationManager.AppSettings.Get("SaveOrderHtmlPath");
        public static string HappyBirthdayHtmlPath = WebConfigurationManager.AppSettings.Get("HappyBirthdayHtmlPath");
        public static string AdvanceBirthdayHtmlPath = WebConfigurationManager.AppSettings.Get("AdvanceBirthdayHtmlPath");
        public static string AddToCartHtmlPath = WebConfigurationManager.AppSettings.Get("AddToCartHtmlPath");
        


        public static string KEY_ID = Convert.ToString(WebConfigurationManager.AppSettings.Get("KEY_ID"));
        public static string KEY_SECRET = Convert.ToString(WebConfigurationManager.AppSettings.Get("KEY_SECRET"));
        public static string PaymentGateWayUrl = Convert.ToString(WebConfigurationManager.AppSettings.Get("PaymentGateWayUrl"));
        public static string CheckoutMyCartPaymentGateWayUrl = Convert.ToString(WebConfigurationManager.AppSettings.Get("CheckoutMyCartPaymentGateWayUrl"));
		public static string PanchangUrl = Convert.ToString(WebConfigurationManager.AppSettings.Get("PanchangUrl"));
		public static string WorldWeatherOnLineUrl = Convert.ToString(WebConfigurationManager.AppSettings.Get("WorldWeatherOnLineUrl"));
		public static string WorldWeatherOnLineKey = Convert.ToString(WebConfigurationManager.AppSettings.Get("WorldWeatherOnLineKey"));
		

		public static string CustomerSavePath = Convert.ToString(WebConfigurationManager.AppSettings.Get("CustomerSavePath"));
        public static string CustomerRetPath = Convert.ToString(WebConfigurationManager.AppSettings.Get("CustomerRetPath"));
        public static string CustomerIcon = Convert.ToString(WebConfigurationManager.AppSettings.Get("CustomerIcon"));
        public static string BookingIcon = Convert.ToString(WebConfigurationManager.AppSettings.Get("BookingIcon"));
        public static string PujaRetPath = Convert.ToString(WebConfigurationManager.AppSettings.Get("PujaRetPath"));

        public static string AdvertismentSavePath = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentSavePath"));
        public static string AdvertismentRetPath = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentRetPath"));
        public static string AdvertismentIcon = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentIcon"));
        public static string ImageSizeLimitText = Convert.ToString(WebConfigurationManager.AppSettings.Get("ImageSizeLimitText"));
        public static string ImageFileExtensions = Convert.ToString(WebConfigurationManager.AppSettings.Get("ImageFileExtensions"));
        public static int ImageSizeMaximum = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("ImageSizeMaximum"));
        public static int ImageSizeMinimum = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("ImageSizeMinimum"));


        public static string AdvertismentVideosSavePath = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentVideosSavePath"));
        public static string AdvertismentVideosRetPath = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentVideosRetPath"));
        public static string AdvertismentVideosIcon = Convert.ToString(WebConfigurationManager.AppSettings.Get("AdvertismentVideosIcon"));
        public static string VideoSizeLimitText = Convert.ToString(WebConfigurationManager.AppSettings.Get("VideoSizeLimitText"));
        public static string VideoFileExtensions = Convert.ToString(WebConfigurationManager.AppSettings.Get("VideoFileExtensions"));
        public static int VideoSizeMaximum = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("VideoSizeMaximum"));
        public static int VideoSizeMinimum = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("VideoSizeMinimum"));

        public const string EXCEL_2003_EXT = ".xls";
        public const string EXCEL_2007_EXT = ".xlsx";
        public static string EXCEL_2003_CONN_STR = Convert.ToString(WebConfigurationManager.AppSettings.Get("Excel2003ConString"));
        public static string EXCEL_2007_CONN_STR = Convert.ToString(WebConfigurationManager.AppSettings.Get("Excel2007ConString"));
        public static string TempExcelUploadPath = Convert.ToString(WebConfigurationManager.AppSettings.Get("TempExcelUploadPath"));
        public static int CmdTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("CmdTimeout"));
        public static string BulkUpload_Template = Convert.ToString(WebConfigurationManager.AppSettings.Get("BulkUpload_Template"));

        public static IList<string> ImageExtensions()
        {
            return ImageFileExtensions.Split(',').ToList();
        }

        public static IList<string> VideoExtensions()
        {
            return VideoFileExtensions.Split(',').ToList();
        }

        public static string GetAdvertismentImageUrl(string ImageName)
        {
            if (BaseUrl.Contains("localhost"))
            {
                return BaseUrl + AdvertismentSavePath + ImageName;
            }
            else
            {
                return BaseUrl + AdvertismentSavePath + ImageName;
            }
        }

        public static string GetCustomerImageUrl(string ImageName)
        {
            if (BaseUrl.Contains("localhost"))
            {
                return BaseUrl + CustomerSavePath + ImageName;
            }
            else
            {
                return BaseUrl + CustomerSavePath + ImageName;
            }
        }

        public static string GetAdvertismentVedioUrl(string VideoName)
        {
            if (BaseUrl.Contains("localhost"))
            {
                return BaseUrl + AdvertismentVideosSavePath + VideoName;
            }
            else
            {
                return BaseUrl + AdvertismentVideosSavePath + VideoName;
            }
        }

        public static string FormatDate(string str, string frmFormat, string toFormat)
        {
            string result = string.Empty;
            DateTime d = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(str))
                DateTime.TryParseExact(str, new[] { frmFormat }, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
            result = d.ToString(toFormat, CultureInfo.InvariantCulture);
            return result;
        }

        public static string FormatDateNull(string str, string frmFormat, string toFormat)
        {
            string result = null;
            DateTime d = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(str))
            {
                DateTime.TryParseExact(str, new[] { frmFormat }, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
                if (d != DateTime.MinValue)
                    result = d.ToString(toFormat, CultureInfo.InvariantCulture);
            }
            return result;
        }

        public static bool IsValidDate(string strDate, string format)
        {
            bool IsValid = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(strDate))
                {
                    DateTime.ParseExact(strDate, format, CultureInfo.InvariantCulture);
                    IsValid = true;
                }
            }
            catch
            {
                IsValid = false;
            }
            return IsValid;
        }
        public static string Base64Encode(string plainText)
        {
           // string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes("inputStr"));
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string plainText)
        {
            //string inputStr = Encoding.UTF8.GetString(Convert.FromBase64String(plainText));
            var base64EncodedBytes = System.Convert.FromBase64String(plainText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string GetUnicodeString(string str)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    byte[] ArrayOFBytes = Encoding.Unicode.GetBytes(str);
                    int v = 0;
                    for (v = 0; v <= ArrayOFBytes.Length - 1; v++)
                    {
                        if (v % 2 == 0)
                        {
                            int t = ArrayOFBytes[v];
                            ArrayOFBytes[v] = ArrayOFBytes[v + 1];
                            ArrayOFBytes[v + 1] = Convert.ToByte(t);
                        }
                    }
                    string c = BitConverter.ToString(ArrayOFBytes);
                    c = c.Replace("-", "");
                    result = result + c;
                }
            }
            catch (Exception ex)
            {
              
            }
            return result;
        }

        public static string GetDurationText(int minutes)
        {
            string durationText = "";

            TimeSpan spWorkMin = TimeSpan.FromMinutes(minutes);
            if ((int)spWorkMin.TotalDays > 0)
            {
                durationText += (int)spWorkMin.TotalDays + "d ";
            }
            if (spWorkMin.Hours > 0)
            {
                durationText += spWorkMin.Hours + "h ";
            }
            if (spWorkMin.Minutes > 0)
            {
                durationText += spWorkMin.Minutes + "m ";
            }

            return durationText.Trim();
        }

        public static string MakeExpiryHash(DateTime expiry)
        {
            const string salt = "Make Expiry Hash";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + expiry.ToString("s"));
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }


        public static bool SendHtmlMail(string[] mailTo, string[] mailCc, string subject, string body, int cnt)
        {
            bool sent = false;

            try
            {
                using (SmtpClient client = new SmtpClient(SMTPServer))
                {
                    client.Credentials = new NetworkCredential(FromEmail, FromPassword);
                    client.Port = SMTPPort;
                    client.EnableSsl = IsSSL;
                    client.Timeout = SMTPTimeout;
                    using (MailMessage mail = new MailMessage())
                    {
                        string to = mailTo != null ? string.Join(",", mailTo) : null;
                        string cc = mailCc != null ? string.Join(",", mailCc) : null;
                        mail.From = new MailAddress(FromEmail, EmailDisplayName);
                        mail.To.Add(to);
                        if (cc != null)
                            mail.CC.Add(cc);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        client.Send(mail);
                        sent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                 sent = false;
                #region hide old code
                //using (SmtpClient client = new SmtpClient(SMTPServer))
                //{
                //    client.Credentials = new NetworkCredential(FromEmail, FromPassword);
                //    client.Port = SMTPPort;
                //    client.EnableSsl = IsSSL;
                //    client.Timeout = SMTPTimeout;
                //    using (MailMessage mail = new MailMessage())
                //    {
                //        string to = mailTo != null ? string.Join(",", mailTo) : null;
                //        string cc = mailCc != null ? string.Join(",", mailCc) : null;
                //        mail.From = new MailAddress(FromEmail, EmailDisplayName);
                //        mail.To.Add(to);
                //        if (cc != null)
                //            mail.CC.Add(cc);
                //        mail.Subject = subject;
                //        mail.Body = body;
                //        mail.IsBodyHtml = true;

                //        client.Send(mail);
                //        sent = true;
                //    }
                //}
                #endregion
            }
            return sent;
        }

        #region push notification for Save Puja Order  and Save Astro Order
        public static void OrderNotification(NotificationEnitity notification, Responce responce,string ApplicationId,string SenderId)
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

            System.Threading.Thread thsendgcm = new System.Threading.Thread(new System.Threading.ThreadStart(() => CommonMethods.OrderNotification(notification.device_key, noti, ApplicationId, SenderId)));
            thsendgcm.Start();
        }

        public static string OrderNotification(string deviceKey, NotificationEnitity pn, string ApplicationId, string SenderId)
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
                            //Push title
                            Title = "",
                            //Unique id
                            PushId = "1"
                        }
                    };
                    json = new JavaScriptSerializer().Serialize(data);
                }
          
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationId));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
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




        public static string GetExcelConnectionString(string excelFilePath, bool hasHeader = true)
        {
            string fileExtension = Path.GetExtension(excelFilePath).ToLower();
            string header = hasHeader ? "Yes" : "No";

            string excelFileConStr = string.Empty;
            switch (fileExtension)
            {
                case EXCEL_2003_EXT:
                    excelFileConStr = String.Format(EXCEL_2003_CONN_STR, excelFilePath, header);
                    break;
                case EXCEL_2007_EXT:
                    excelFileConStr = String.Format(EXCEL_2007_CONN_STR, excelFilePath, header);
                    break;
            }
            return excelFileConStr;
        }

        public static DataSet ImportExcelToDataSet(string excelConnectString, string[] ColumnNames, string TableName = "Table", int Row = 0)
        {
            DataSet dataSetExcel = null;
            using (OleDbConnection oledbExcelConnection = new OleDbConnection(excelConnectString))
            {
                try
                {
                    //log.Info("STEP 2  excelConnectString : - " + excelConnectString);
                    oledbExcelConnection.Open();

                    DataTable excelSheetSchema = oledbExcelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (excelSheetSchema != null && excelSheetSchema.Rows.Count > 0)
                    {
                        string excelWorkSheetName = excelSheetSchema.Rows[Row]["TABLE_NAME"].ToString();
                        //log.Info("STEP 3  excelWorkSheetName : - " + excelWorkSheetName);

                        DataTable myColumns = oledbExcelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, excelWorkSheetName, null });
                        string[] excelColumns = myColumns.AsEnumerable().Select(row => row.Field<string>("COLUMN_NAME").Trim()).ToArray();
                        bool areCoulmnNamesEqual = (excelColumns.Intersect(ColumnNames, StringComparer.OrdinalIgnoreCase).Count() == ColumnNames.Length); //false   
                        if (!areCoulmnNamesEqual)
                            throw new InvalidOperationException("Invalid Column names " + string.Join(",", excelColumns.Except(ColumnNames, StringComparer.OrdinalIgnoreCase).ToArray()) + " . Please upload valid file!");
                        dataSetExcel = new DataSet();
                        DataTable dtExcelWorkSheet = ImportWorkSheetToDataTable(oledbExcelConnection, excelWorkSheetName, TableName);
                        dataSetExcel.Tables.Add(dtExcelWorkSheet);
                    }
                }
                catch (Exception ex)
                {
                    //log.Info("STEP 4  Error -01 : - " + ex.Message);
                    throw ex;
                }
                finally
                {
                    if (oledbExcelConnection.State == ConnectionState.Open)
                        oledbExcelConnection.Close();
                }
            }
            return dataSetExcel;
        }

        private static DataTable ImportWorkSheetToDataTable(OleDbConnection excelFileConn, string workSheetName, string TableName)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", workSheetName), excelFileConn);
            DataTable dtExcelWorkSheet = new DataTable(TableName);
            da.Fill(dtExcelWorkSheet);
            return dtExcelWorkSheet;
        }
        //https://stackoverflow.com/questions/7023140/how-to-remove-empty-rows-from-datatable
        public static DataTable RmoveEmptyRows(DataTable dt)
        {
            List<int> rowIndexesToBeDeleted = new List<int>();
            int indexCount = 0;
            foreach (var row in dt.Rows)
            {
                var r = (DataRow)row;
                int emptyCount = 0;
                int itemArrayCount = r.ItemArray.Length;
                foreach (var i in r.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;

                if (emptyCount == itemArrayCount) rowIndexesToBeDeleted.Add(indexCount);

                indexCount++;
            }

            int count = 0;
            foreach (var i in rowIndexesToBeDeleted)
            {
                dt.Rows.RemoveAt(i - count);
                count++;
            }

            return dt;
        }

   

    }
}