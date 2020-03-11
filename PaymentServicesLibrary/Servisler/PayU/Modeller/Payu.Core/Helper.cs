using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller
{
    public class Helper
    {


        //Sha 256 ile hash
        public static string CreateSha256Hash(string strToHash, string hashKey)
        {


            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(hashKey);
            HMACSHA256 hmac = new HMACSHA256(keyBytes);

            byte[] hashbytes = System.Text.Encoding.UTF8.GetBytes(strToHash);
            hmac.ComputeHash(hashbytes);
            return GetHexaDecimal(hmac.Hash);
        }

        //MD5 Hash yöntemi için kullanılır
        public static string CreateHash(string strToHash, string hashKey)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(hashKey);
            HMACMD5 hmac = new HMACMD5(keyBytes);

            byte[] hashbytes = System.Text.Encoding.UTF8.GetBytes(strToHash);
            hmac.ComputeHash(hashbytes);
            return GetHexaDecimal(hmac.Hash);
        }

        private static string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }
        public static string GetLengthAsByte(string strToCount)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            if (!string.IsNullOrEmpty(strToCount))
            {
                return encoding.GetByteCount(strToCount).ToString();
            }

            return "0";
        }
        private static string ConvertDecimalToString(decimal value)
        {
            return String.Format("{0:0.00}", (value).ToString(CultureInfo.CreateSpecificCulture("en-GB")));
        }
        public static string TrimInvalidCharacters(string originalName)
        {
            originalName = originalName.TrimEnd('+').TrimStart('+').Replace("+", "-").Replace("--", "-").TrimEnd('.').TrimStart('.');

            string array = "?|,|~|!|@|%|^|(|)|<|>|:|;|{|}|[|]|&|`|?|«|´|»|°|++|+++|++++|”|“|[|]|..|...|....|’|‘|\\|'|-|/|#|*|?";
            string[] removeChars = array.Split('|');
            foreach (string item in removeChars)
            {
                if (originalName.Contains(item))
                {
                    originalName = originalName.TrimEnd(item.ToCharArray());
                    originalName = originalName.TrimStart(item.ToCharArray());
                }
            }
            originalName = originalName.Replace("|", "-");
            return originalName;
        }
        /// <summary>
        /// Limits an input string to the specified length.
        /// </summary>
        /// <param name="str">The string to keep within the specified limit.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <returns>If the input string is less than or equal to the maxLength, the original string is returned.  
        /// If the input string is greater than maxLength, the string is truncated to the correct length.</returns>
        public static String Truncate(String str, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (str.Length <= maxLength) return str;
            return str.Substring(0, maxLength);
        }


        /// <summary>
        /// Generate Friendly Name for url
        /// </summary>
        /// <param name="originalName"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string GenerateFriendlyName(string originalName, bool upperCase)
        {
            if (upperCase)
            {
                originalName = originalName.ToUpper();
            }
            else
            {
                originalName = originalName.ToLower();
            }

            originalName = originalName.Replace("ı", "i").Replace("İ", "I");
            originalName = originalName.Replace("ğ", "g").Replace("Ğ", "G");
            originalName = originalName.Replace("ş", "s").Replace("Ş", "S");
            originalName = originalName.Replace("ü", "u").Replace("Ü", "U");
            originalName = originalName.Replace("ö", "o").Replace("Ö", "O");
            originalName = originalName.Replace("ç", "c").Replace("Ç", "C");
            originalName = originalName.Replace("'", "");
            originalName = Regex.Replace(originalName, @"\.{2,}", "-");
            originalName = Regex.Replace(originalName, @"\s{2,}", "-");
            originalName = HttpUtility.UrlEncode(originalName);
            originalName = originalName.Replace("+", "-");
            originalName = Regex.Replace(originalName, @"[%]\w\w", "");

            return originalName;
        }


        public static string SerializeToJsonString(string jsonData)
        {
            return JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
