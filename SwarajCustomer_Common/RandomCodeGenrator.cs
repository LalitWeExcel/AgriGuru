using System;
using System.Text;

namespace SwarajCustomer_Common
{
    public static class RandomCodeGenrator
    {
        // Generate a random number between two numbers    
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random Code    
        //public static string RandomCode()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(RandomString(10, true));
        //    //builder.Append(RandomNumber(1000, 9999));
        //    //builder.Append(RandomString(2, false));
        //    return builder.ToString();
        //}

        //  upper side  also function for RandomCode()  
        public static string RandomCode()  
        {
            int length = 10;


            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

    }
}
