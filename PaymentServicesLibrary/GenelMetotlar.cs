using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Security.Cryptography;

namespace PaymentServicesLibrary
{
    public class GenelMetotlar
    {
        public static String getDomain()
        {
            try
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            }
            catch (Exception e)
            {
                return "https://localhost:44395";
            }
        }


        public static String GetSHA1(String data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            String hashedPassword = data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(hashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexadecimal(inputbytes);
        }

        public static String GetHexadecimal(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int length = bytes.Length;
            for(int i=0; i<length; i++)
            {
                sb.Append(String.Format("{0,2:x}", bytes[i]).Replace(" ", "0"));
                
            }

            return sb.ToString();
        }
        public static String getIP()
        {
            return "46.197.222.101";
            //return HttpContext.Current.Request.UserHostAddress;
        }

        public static String SHA256Hash(String hashKey)
        { 
            System.Text.Encoding encoding = Encoding.UTF8;
            byte[] plainBytes = encoding.GetBytes(hashKey);
            System.Security.Cryptography.SHA256Managed sha256Engine = new SHA256Managed();
            string hashedData = String.Empty;
            byte[] hashedBytes = sha256Engine.ComputeHash(plainBytes, 0, encoding.GetByteCount(hashKey));
            foreach (byte bit in hashedBytes)
            {
                hashedData += bit.ToString("x2");
            }
            return hashedData;
        }

    }
}
