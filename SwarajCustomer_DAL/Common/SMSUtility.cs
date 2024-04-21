using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SwarajCustomer_DAL.Common
{
    public class SMSUtility
    {
        public static string SendSMS(string PhoneNumbers, string SMSText)
        {
            if (!string.IsNullOrEmpty(PhoneNumbers))
            {
                SMSEntity smsobj = new SMSEntity();
                StringBuilder sb = new StringBuilder();
                using (SwarajTestEntities db = new SwarajTestEntities())
                {
                    smsobj = db.adm_application_SMS_gateway_configuration.Where(x => x.SMS_Gateway_ID == 1 && x.Is_Active == true)
                             .Select(y => new SMSEntity
                             {
                                 SMS_Gateway_API = y.SMS_Gateway_API,
                                 SMS_Text = y.SMS_Text,
                                 SMS_Contact_No = y.SMS_Contact_No,
                                 SMS_Gateway_Provider_Name = y.SMS_Gateway_Provider_Name
                             }).FirstOrDefault();

                    if (smsobj != null)
                    {
                        try
                        {
                            sb.Append(smsobj.SMS_Gateway_API);
                            if (sb.Length > 0)
                            {
                                sb.Replace(smsobj.SMS_Contact_No, PhoneNumbers);
                                sb.Replace(smsobj.SMS_Text, SMSText);
                                string path = sb.ToString();
                                object req = (HttpWebRequest)WebRequest.Create(path);
                                WebResponse response = ((HttpWebRequest)req).GetResponse();
                                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                                DoSMSTracking(PhoneNumbers, SMSText);
                                return streamReader.ReadToEnd();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return string.Empty;
        }

        private static void DoSMSTracking(string PhoneNumbers, string SMSText)
        {
            using (SwarajTestEntities db = new SwarajTestEntities())
            {
                if (!string.IsNullOrEmpty(PhoneNumbers))
                {
     
                }
            }
        }
    }
}
