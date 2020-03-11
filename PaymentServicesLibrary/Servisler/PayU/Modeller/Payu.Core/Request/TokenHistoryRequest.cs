using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Token servisi token token geçmişini listelemek için kullanılan servistir.
    /// İlgili parametreler SHA256 yöntemi ile hashlenmektedir.
    /// Verilen Url'ye oluşan hash değeri ve diğer parametreler eklenerek HTTPGET yöntemi ile token bigileri sorgulanmaktadır.
    /// Response cevabı son kullanıcıya gösterilmektedir.
    /// </summary>
    public class TokenHistoryRequest
    {
        public string MERCHANT { get; set; }
        public string TIMESTAMP { get; set; }
        public string TOKEN { get; set; }


        public static string Execute(TokenHistoryRequest request, Options options)
        {
            string hash = CreateSignature(request, options.SecretKey);
            var url = options.Url + request.TOKEN  + "/history" + "?merchant=" + request.MERCHANT + "&timestamp=" + request.TIMESTAMP + "&signature=" + hash; 
            return HttpCaller.GetDataToUrl(url);
        }


        public static string CreateSignature(TokenHistoryRequest request, string secretKey)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = request.MERCHANT;
            hashString += request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;

        }
    }
}
