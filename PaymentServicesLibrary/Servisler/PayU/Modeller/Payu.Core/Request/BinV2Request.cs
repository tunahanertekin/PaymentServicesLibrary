using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Bu sınıf Binv2 puan sorgulama ve puan kullanma işlemi için kullanılan sınıfı temsil etmektedir.
    /// Execute metodu içerisinde ilgili parametreler, option içerisinde secret key ve url bilgisi bulunmaktadır.
    /// Parametreler SHA256 ile hashlendikten sonra get metodu ile get işlemi yapılmaktadır.
    /// </summary>
    public class BinV2Request
    {
        public string MERCHANT { get; set; }
        public string EXTRAINFO { get; set; }
        public string DATETIME { get; set; }
        public string CC_CVV { get; set; }
        public string CC_OWNER { get; set; }
        public string EXP_YEAR { get; set; }
        public string EXP_MONTH { get; set; }
        public string CC_NUMBER { get; set; }


        public static string Execute(BinV2Request request, Options options)
        {
            var nameValueCollection = CreateHashString(request, options.SecretKey);
            return HttpCaller.PostDataArrayReturnString(options.Url, nameValueCollection); 
        }     
        public static NameValueCollection CreateHashString(BinV2Request request, string secretKey)
        {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("merchant", request.MERCHANT);
            dictionary.Add("extraInfo", request.EXTRAINFO);
            dictionary.Add("dateTime", request.DATETIME);
            dictionary.Add("cc_cvv", request.CC_CVV);
            dictionary.Add("cc_owner", request.CC_OWNER);
            dictionary.Add("exp_month", request.EXP_MONTH);
            dictionary.Add("exp_year", request.EXP_YEAR);
            dictionary.Add("cc_number", request.CC_NUMBER);
            var ordered = dictionary.OrderBy(x => x.Key);
            

            NameValueCollection collection = new NameValueCollection();
            collection.Add("cc_cvv", request.CC_CVV);
            collection.Add("cc_number", request.CC_NUMBER);
            collection.Add("cc_owner", request.CC_OWNER);
            collection.Add("dateTime", request.DATETIME);
            collection.Add("exp_month", request.EXP_MONTH);
            collection.Add("exp_year", request.EXP_YEAR);
            collection.Add("extraInfo", request.EXTRAINFO);
            collection.Add("merchant", request.MERCHANT);
            collection.Add("signature", CreateSHA256Hash(secretKey, dictionary));
            return collection;
        }

        protected static string CreateSHA256Hash(string secretKey, Dictionary<string, string> dictionary)
        {
            string HASH_CONTENT = string.Empty;
            string HASHED_CONTENT = string.Empty;
            var dictionaryOrder = dictionary.OrderBy(x => x.Key);
            foreach (var item in dictionaryOrder)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.Value) + item.Value;
            }
            HASHED_CONTENT = Helper.CreateSha256Hash(HASH_CONTENT, secretKey);
            return HASHED_CONTENT;
        }

    }


}

