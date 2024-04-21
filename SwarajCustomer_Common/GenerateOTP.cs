using System;

namespace SwarajCustomer_Common
{
    public class GenerateRandom
    {
        public static string GenerateRandomOTP()
        {
            //Specifies OTP lenght
            int iOTPLength = 4;

            //OTP numbers
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }

            return sOTP;
        }

        public static string GenerateRandomNumber(int length)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string number = String.Empty;
            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                number += sTempChars;
            }

            return number;
        }
    }
}
