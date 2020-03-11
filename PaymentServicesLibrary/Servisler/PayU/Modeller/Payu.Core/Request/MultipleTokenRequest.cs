using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{

    /// <summary>
    /// Token servisi birden fazla token detaylarını listelemek için kullanılan servistir.
    /// İlgili parametreler SHA256 yöntemi ile hashlenmektedir.
    /// Verilen Url'ye oluşan hash değeri ve diğer parametreler eklenerek HTTPGET yöntemi ile token bigileri sorgulanmaktadır.
    /// Response cevabı son kullanıcıya gösterilmektedir.
    /// </summary>
    public class MultipleTokenRequest
    {
        public string MERCHANT { get; set; }
        public string TIMESTAMP { get; set; }
        public List<Token> TOKENS { get; set; }

        public static string Execute(MultipleTokenRequest request,Options options)
        {
            string hash = CreateSignature(request, options.SecretKey);
            int i = 0;
            string tokenString = string.Empty;
            foreach (var item in request.TOKENS)
            {
                tokenString += "&tokens[" + i + "]=" + item.TOKEN;
                i++;
            }
            tokenString = tokenString.Substring(1);

            string url = options.Url + "merchantToken?" + tokenString + "&merchant=" + request.MERCHANT + "&timestamp=" + request.TIMESTAMP + "&signature=" + hash;

            return HttpCaller.GetDataToUrl(url);
        }
        public static string CreateSignature(MultipleTokenRequest request, string secretKey)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = request.MERCHANT;
            foreach (var tokeenItem in request.TOKENS)
            {
                hashString += tokeenItem.TOKEN;
            }
            hashString += request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;

        }


    }
    public class Token
    {
        public string TOKEN { get; set; }
    }

   
}
