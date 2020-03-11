using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{

    /// <summary>
    /// Token servisi token detaylarını listelemek için kullanılan servistir.
    /// İlgili parametreler SHA256 yöntemi ile hashlenmektedir.
    /// Verilen Url'ye oluşan hash değeri ve diğer parametreler eklenerek HTTPGET yöntemi ile token bigileri sorgulanmaktadır.
    /// Response cevabı son kullanıcıya gösterilmektedir.
    /// </summary>
    public class GetTokenInformationRequest
    {
        public string MERCHANT { get; set; }
        public string TIMESTAMP { get; set; }
        public string TOKEN { get; set; }


        public static string Execute(GetTokenInformationRequest request, Options options)
        {
            string hash = CreateSignature(request, options.SecretKey);
            var url= options.Url+request.TOKEN+ "?merchant=" + request.MERCHANT+ "&timestamp=" + request.TIMESTAMP + "&signature="+ hash; //secure.payu.com.tr/order/token/v2/merchantToken/b7e5d8649c9e2e75726b59c56c29e91d?merchant=CC921&timestamp=1428046996&signature=34b084915a67bf2b54eff4a29e677c2718e26a6632496bfb4c5880a5d938b96e
            return HttpCaller.GetDataToUrl(url);
        }



        public static string CreateSignature(GetTokenInformationRequest request, string secretKey)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = request.MERCHANT;
            hashString += request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;
            
        }
    }
}
