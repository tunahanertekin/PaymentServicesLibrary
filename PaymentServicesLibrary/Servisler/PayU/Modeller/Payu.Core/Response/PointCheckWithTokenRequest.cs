using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Response
{
    public class PointCheckWithTokenRequest
    {
        public string MERCHANT { get; set; }
        public string CURRENCY { get; set; }
        public string DATE { get; set; }
        public string TOKEN { get; set; }

        public static PointCheckResponse Execute(PointCheckWithTokenRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<PointCheckResponse>(options.Url, hashString);
        }

        public static string CreateHashString(PointCheckWithTokenRequest request, string secretKey)
        {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("MERCHANT", request.MERCHANT);
            dictionary.Add("CURRENCY", request.CURRENCY);
            dictionary.Add("DATE", request.DATE);
            dictionary.Add("TOKEN", request.TOKEN);
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
