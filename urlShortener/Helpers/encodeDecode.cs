using System;
using System.Text;
using urlShortener.Models;

namespace urlShortener.Helpers
{
    public class encodeDecode
    {
        public encodeDecode()
        {
        }

        public static string generateRandom()
        {



            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }



    }
}

