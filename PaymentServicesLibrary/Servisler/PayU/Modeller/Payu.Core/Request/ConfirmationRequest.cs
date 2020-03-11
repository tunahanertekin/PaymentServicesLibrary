using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Konfirmasyon Servisi(IDN) için kullanılan sınıfı temsil etmektedir.
    /// İlgili parametreler MD5 ile hashlenerek url'e post edilmektedir.
    /// Execute metodunda ilgili servis çağrısı yapılmaktadır.
    /// Response sonucunda oluşan metin son kullanıcıya gösterilmektedir.
    /// </summary>
    public class ConfirmationRequest
    {
        public string MERCHANT { get; set; }
        public string ORDER_REF { get; set; }
        public string ORDER_AMOUNT { get; set; }
        public string ORDER_CURRENCY { get; set; }
        public string CHARGE_AMOUNT { get; set; }
        public string IDN_DATE { get; set; }
        public string ORDER_HASH { get; set; }
        public string REF_URL { get; set; }

        public static string Execute(ConfirmationRequest request,Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.PostDataReturnString(options.Url, hashString);
        }
        public static string CreateHashString(ConfirmationRequest request, string secretKey)
        {
            var hashString = "MERCHANT=" + request.MERCHANT;
            hashString += "&ORDER_REF=" + request.ORDER_REF;
            hashString += "&ORDER_AMOUNT=" + request.ORDER_AMOUNT;
            hashString += "&ORDER_CURRENCY=" + request.ORDER_CURRENCY;
            hashString += "&IDN_DATE=" + request.IDN_DATE;
            hashString += "&CHARGE_AMOUNT=" + request.CHARGE_AMOUNT;
            hashString += "&ORDER_HASH=" + CreateMD5Hash(secretKey, request);
            return hashString;
        }
        protected static string CreateMD5Hash(string secretKey, ConfirmationRequest request)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = Helper.GetLengthAsByte(request.MERCHANT) + request.MERCHANT;
            hashString += Helper.GetLengthAsByte(request.ORDER_REF) + request.ORDER_REF;
            hashString += Helper.GetLengthAsByte(request.ORDER_AMOUNT) + request.ORDER_AMOUNT;
            hashString += Helper.GetLengthAsByte(request.ORDER_CURRENCY) + request.ORDER_CURRENCY;
            hashString += Helper.GetLengthAsByte(request.IDN_DATE) + request.IDN_DATE;
            hashString += Helper.GetLengthAsByte(request.CHARGE_AMOUNT) + request.CHARGE_AMOUNT;
            HASHED_CONTENT = Helper.CreateHash(hashString, secretKey);
            return HASHED_CONTENT;
        }

    }
}
