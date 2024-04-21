using System;

namespace SwarajCustomer_Common
{
    public class Extensions
    {
        public static object UserDoesNotExists()
        {
            return string.Format("Invalid credentials, please try again.");
        }

        public static string Unauthorized()
        {
            return string.Format("You are not authorized to login into this application.");
        }

        public static string OTP_SMS_CUST(string OTP)
        {
            return string.Format("Dear user, " + OTP + " is your OTP.");
        }

        public static string ContactEmailSubject()
        {
            return string.Format("Contact Us - Request Received");
        }

        public static string ContactEmailBody(string username, string email, string phone, string remarks)
        {
            return string.Format("Hello Team,<br/><br/>" + username + " has requested to be contacted. Following are his details:<br/>Name: " + username + "<br/>Phone: " + phone + "<br/>Email: " + email + "<br/>Remarks: " + remarks + "<br/><br/>Thanks!");
        }

        public static string ContactRequestReceived()
        {
            return string.Format("Your request has been received. We will contact you shortly.");
        }
        public static string PujaOrderContent(string pujaName ,string  date,string Time, string order_number)
        {
            return string.Format("You have booked a '" + pujaName + "' for the '" + date + "' date and time '"+ Time + "'  and Order number is '" + order_number + "'");
        }
        public static string PujaOrderContentProhitAstro(string pujaName, string date, string Time, string order_number)
        {
            return string.Format("You  have a  booking of '" + pujaName + "' for the '" + date + "' date and time '" + Time + "'  and Order number is '" + order_number + "'");
        }
        public static string EPujaOrderContent(string pujaName, string date, string Time, string link)
        {
            return string.Format("You have e-puja a '" + pujaName + "' for the '" + date + "' date and time '" + Time + "'  and  the e-Puja link is  <a href="+"https://www.tutorialrepublic.com/"+"> click here !</a>" + "'");
        }

        public static string BookingCancelContent(string pujaName, string date, string Time, string order_number)
        {
            return string.Format("Your '" + pujaName + "' for the '" + date + "' date and time '" + Time + "'  and Order number is '" + order_number + "' has been succussfully cancel ");
        }
        public static string BookingConfirmContent(string pujaName, string date, string Time, string order_number)
        {
            return string.Format("Your '" + pujaName + "' for the '" + date + "' date and time '" + Time + "'  and Order number is '" + order_number + "' has been succussfully completed ");
        }

        public static string BookingConfirmByAdminContent(string pujaName, string date, string Time, string order_number)
        {
            return string.Format("Your '" + pujaName + "' for the '" + date + "' date and time '" + Time + "'  and Order number is '" + order_number + "' has been succussfully Confirm By Admin ");
        }

    }
}

