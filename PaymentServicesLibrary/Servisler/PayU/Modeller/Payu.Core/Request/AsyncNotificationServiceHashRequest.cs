using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Asenkron bildirim servisi için gerekli parametreler bu alanda tanımlanmaktadır.
    /// </summary>
    public class AsyncNotificationServiceHashRequest
    {
        public string IPN_PID { get; set; }
        public string IPN_PNAME { get; set; }
        public string IPN_DATE { get; set; }
        public string DATE { get; set; }

        public static string CalculateHash(AsyncNotificationServiceHashRequest request,string secretKey)
        {
            string HASH_CONTENT = string.Empty;
            string HASHED_CONTENT = string.Empty;
            HASH_CONTENT += Helper.GetLengthAsByte(request.IPN_PID) + request.IPN_PID;
            HASH_CONTENT += Helper.GetLengthAsByte(request.IPN_PNAME) + request.IPN_PNAME;
            HASH_CONTENT += Helper.GetLengthAsByte(request.IPN_DATE) + request.IPN_DATE;
            HASH_CONTENT += Helper.GetLengthAsByte(request.DATE) + request.DATE;
            HASHED_CONTENT = Helper.CreateHash(HASH_CONTENT, secretKey);
            return HASHED_CONTENT;
        }

    }
}
