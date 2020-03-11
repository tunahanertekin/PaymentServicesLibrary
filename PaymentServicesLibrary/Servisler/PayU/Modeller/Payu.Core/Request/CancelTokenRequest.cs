using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Token iptal etme servisi için kullanılan servisi temsil etmektedir.
    ///  ilgili parametreler Sha256 ile hashlenerek Execute metodu içerisinde get ilgili parametreye get işlemi yapılmaktadır.
    ///  Oluşan sorgu sonucunda ilgili response değerleri ekranda son kullanıcıya gösterilmektedir.
    /// </summary>
    public class CancelTokenRequest
    {
        public string MERCHANT { get; set; }
        public string CANCELREASON { get; set; }
        public string TIMESTAMP { get; set; }
        public string TOKEN { get; set; }

        public static string Execute(CancelTokenRequest request, Options options)
        {
            string hash = CreateSignature(request, options.SecretKey);
            var url = options.Url + request.TOKEN + "?merchant=" + request.MERCHANT + "&timestamp=" + request.TIMESTAMP + "&signature=" + hash+ "&cancelReason="+request.CANCELREASON;
            return HttpCaller.GetDataToUrl(url);
        }

        public static string CreateSignature(CancelTokenRequest request, string secretKey)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = request.CANCELREASON;
            hashString += request.MERCHANT;
            hashString += request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;

        }

    }
}
