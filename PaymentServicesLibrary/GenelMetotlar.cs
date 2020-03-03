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

        /*
         * Public Function GetSHA1(ByVal SHA1Data As String) As String

                Dim sha As SHA1 = New SHA1CryptoServiceProvider()
                Dim HashedPassword As String = SHA1Data
                Dim hashbytes As Byte() = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword)
                Dim inputbytes As Byte() = sha.ComputeHash(hashbytes)
                Return GetHexaDecimal(inputbytes)

            End Function
            Public Shared Function GetHexaDecimal(ByVal bytes As Byte()) As String

                Dim s As New StringBuilder()
                Dim length As Integer = bytes.Length
                For n As Integer = 0 To length - 1
                    s.Append([String].Format("{0,2:x}", bytes(n)).Replace(" ", "0"))
                Next
                Return s.ToString()

            End Function
         */

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

    }
}
