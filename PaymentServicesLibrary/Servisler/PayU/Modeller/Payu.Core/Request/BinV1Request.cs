using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Bu sınıf Bin numarası sorgulama işlemi için kullanılan sınıfı temsil etmektedir.
    /// Execute metodu içerisinde ilgili parametreler, option içerisinde secret key ve url bilgisi bulunmaktadır.
    /// Parametreler SHA256 ile hashlendikten sonra get metodu ile get işlemi yapılmaktadır.
    /// </summary>
    public class BinV1Request
    {
        public string MERCHANT { get; set; }
        public int TIMESTAMP { get; set; }
        public string BIN { get; set; }

        public static string Execute(BinV1Request request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            string createUrl = options.Url + request.BIN + "?" + hashString;

            return HttpCaller.GetDataToUrl(createUrl);
        }
        public static string CreateHashString(BinV1Request request, string secretKey)
        {
            var hashString = "merchant=" + request.MERCHANT;
            hashString += "&timestamp=" + request.TIMESTAMP;
            hashString += "&signature=" + CreateSha256Hash(secretKey, request);
            return hashString;
        }

        protected static string CreateSha256Hash(string secretKey, BinV1Request request)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString =  request.MERCHANT;
            hashString +=  request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;
        }

    }
}
