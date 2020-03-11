using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// işlem sorgu servisi için kullanılann sınıfı temsil etmektedir.
    /// İlgili parametreler MD5 yöntemi ile hashlenerek HTTPPOST yöntemi ile ilgili url'e post edilmektedir.
    /// Execute metodu api çağrısının yapıldığı yeri temsil etmektedir.
    /// </summary>
    public class IOSRequest
    {
        public string MERCHANT { get; set; }
        public string REFNOEXT { get; set; }
        public static IOSResponse Execute(IOSRequest request,Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<IOSResponse>(options.Url, hashString);
        }
        public static string CreateHashString(IOSRequest request, string secretKey)
        {
            var hashString = "MERCHANT=" + request.MERCHANT;
            hashString += "&REFNOEXT=" + request.REFNOEXT;
            hashString += "&HASH=" + CreateMD5Hash(secretKey, request);
            return hashString;
        }
        protected static string CreateMD5Hash(string secretKey, IOSRequest request)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = Helper.GetLengthAsByte(request.MERCHANT) + request.MERCHANT;
            hashString += Helper.GetLengthAsByte(request.REFNOEXT) + request.REFNOEXT;     
            HASHED_CONTENT = Helper.CreateHash(hashString, secretKey);
            return HASHED_CONTENT;
        }
    }
}
