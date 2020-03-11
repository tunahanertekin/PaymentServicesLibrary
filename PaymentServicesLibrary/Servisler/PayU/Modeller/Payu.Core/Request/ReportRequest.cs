using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Rapor servisi rapor detaylarını listelemek için kullanılan servistir.
    /// İlgili parametreler MD5 yöntemi ile hashlenmektedir.
    /// Verilen Url'ye oluşan hash değeri ve diğer parametreler eklenerek HTTPGET yöntemi ile  sorgulanmaktadır.
    /// Response cevabı son kullanıcıya gösterilmektedir.
    /// </summary>
    public class ReportRequest
    {
        public string merchant { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string timeStamp { get; set; }


        public static string Execute(ReportRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);

            string url = options.Url + "?" + hashString;
            return HttpCaller.GetDataToUrl(url);
        }
        public static string CreateHashString(ReportRequest request, string secretKey)
        {
            var hashString = "merchant=" + request.merchant;
            hashString += "&startDate=" + request.startDate;
            hashString += "&endDate=" + request.endDate;
            hashString += "&timeStamp=" + request.timeStamp;
            hashString += "&signature=" + CreateMD5Hash(secretKey, request);
            return  hashString;
        }

        protected static string CreateMD5Hash(string secretKey, ReportRequest request)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = Helper.GetLengthAsByte(request.merchant) + request.merchant;
            hashString += Helper.GetLengthAsByte(request.startDate) + request.startDate;
            hashString += Helper.GetLengthAsByte(request.endDate) + request.endDate;
            hashString += Helper.GetLengthAsByte(request.timeStamp) + request.timeStamp;
            HASHED_CONTENT = Helper.CreateHash(hashString, secretKey);
            return HASHED_CONTENT;
        }

    }
}
