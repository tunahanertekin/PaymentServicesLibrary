using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// İptal/İade Servisi (IRN) için kullanılan sınıfı temsil etmektedir.
    /// İlgili parametreler MD5 yöntemi ile hashlenerek HTTPPOST yöntemi ile ilgili url'e post edilmektedir.
    /// Execute metodu api çağrısının yapıldığı yeri temsil etmektedir.
    /// </summary>
    public class IRNRequest
    {
        public string MERCHANT { get; set; }
        public string ORDER_REF { get; set; }
        public string ORDER_AMOUNT { get; set; }
        public string ORDER_CURRENCY { get; set; }
        public string IRN_DATE { get; set; }
        public string ORDER_HASH { get; set; }
        public string AMOUNT { get; set; }

        public static string Execute(IRNRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.PostDataReturnString(options.Url, hashString);
        }

        public static string CreateHashString(IRNRequest request, string secretKey)
        {
            var hashString = "MERCHANT=" + request.MERCHANT;
            hashString += "&ORDER_REF=" + request.ORDER_REF;
            hashString += "&ORDER_AMOUNT=" + request.ORDER_AMOUNT;
            hashString += "&ORDER_CURRENCY=" + request.ORDER_CURRENCY;
            hashString += "&IRN_DATE=" + request.IRN_DATE;
            hashString += "&AMOUNT=" + request.AMOUNT;

            hashString += "&ORDER_HASH=" + CreateMD5Hash(secretKey, request);
            return hashString;
        }

        protected static string CreateMD5Hash(string secretKey, IRNRequest request)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = Helper.GetLengthAsByte(request.MERCHANT) + request.MERCHANT;
            hashString += Helper.GetLengthAsByte(request.ORDER_REF) + request.ORDER_REF;
            hashString += Helper.GetLengthAsByte(request.ORDER_AMOUNT) + request.ORDER_AMOUNT;
            hashString += Helper.GetLengthAsByte(request.ORDER_CURRENCY) + request.ORDER_CURRENCY;
            hashString += Helper.GetLengthAsByte(request.IRN_DATE) + request.IRN_DATE;
            hashString += Helper.GetLengthAsByte(request.AMOUNT) + request.AMOUNT;

            HASHED_CONTENT = Helper.CreateHash(hashString, secretKey);
            return HASHED_CONTENT;
        }

    }
}
