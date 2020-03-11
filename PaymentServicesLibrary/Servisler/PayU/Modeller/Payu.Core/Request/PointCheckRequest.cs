using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{

    /// <summary>
    /// Puan sorgu  servisi token detaylarını listelemek için kullanılan servistir.
    /// İlgili parametreler MD5 yöntemi ile hashlenmektedir.
    /// Verilen Url'ye oluşan hash değeri ve diğer parametreler eklenerek HTTPPOST yöntemi ile gönderilemektedir..
    /// Response cevabı son kullanıcıya gösterilmektedir.
    /// </summary>
    public class PointCheckRequest
    {
        public string MERCHANT { get; set; }
        public string CURRENCY { get; set; }
        public string DATE { get; set; }
        public string CC_CVV { get; set; }
        public string CC_OWNER { get; set; }
        public string EXP_YEAR { get; set; }
        public string EXP_MONTH { get; set; }
        public string CC_NUMBER { get; set; }


        public static PointCheckResponse Execute(PointCheckRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<PointCheckResponse>(options.Url, hashString);
        }

        public static string CreateHashString(PointCheckRequest request, string secretKey)
        {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("MERCHANT", request.MERCHANT);
            dictionary.Add("CURRENCY", request.CURRENCY);
            dictionary.Add("DATE", request.DATE);
            dictionary.Add("CC_CVV", request.CC_CVV);
            dictionary.Add("CC_OWNER", request.CC_OWNER);
            dictionary.Add("EXP_YEAR", request.EXP_YEAR);
            dictionary.Add("EXP_MONTH", request.EXP_MONTH);
            dictionary.Add("CC_NUMBER", request.CC_NUMBER);
            var ordered = dictionary.OrderBy(x => x.Key);
            string hash = string.Empty;
            foreach (var item in ordered)
            {
                hash += "&" + item.Key + "=" + item.Value;
            }
            hash = hash.Substring(1);

            hash += "&HASH=" + CreateMD5Hash(secretKey, dictionary);
            return hash;
        }

        protected static string CreateMD5Hash(string secretKey, Dictionary<string, string> dictionary)
        {
            string HASH_CONTENT = string.Empty;
            string HASHED_CONTENT = string.Empty;
            var dictionaryOrder = dictionary.OrderBy(x => x.Key);
            foreach (var item in dictionaryOrder)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.Value) + item.Value;
            }
            HASHED_CONTENT = Helper.CreateHash(HASH_CONTENT, secretKey);
            return HASHED_CONTENT;
        }

    }
}
