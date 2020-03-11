using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    public class TokenServiceCreateTokenRequest
    {
        public string MERCHANT { get; set; }
        public string REFNO { get; set; }
        public string TIMESTAMP { get; set; }

        public static string Execute(TokenServiceCreateTokenRequest request,Options options)
        {
            var nameValueCollection = CreatePostDataArray(request, options.SecretKey);
            return HttpCaller.PostDataArrayReturnString(options.Url, nameValueCollection);

        }
        public static NameValueCollection CreatePostDataArray(TokenServiceCreateTokenRequest request, string secretKey)
        {

            NameValueCollection collection = new NameValueCollection();
            collection.Add("merchant", request.MERCHANT);
            collection.Add("refNo", request.REFNO);
            collection.Add("timestamp", request.TIMESTAMP);
            collection.Add("signature", CreateSignature(request, secretKey));
            return collection;
        }
        public static string CreateSignature(TokenServiceCreateTokenRequest request,string secretKey)
        {
            string HASHED_CONTENT = string.Empty;
            var hashString = Helper.GetLengthAsByte(request.MERCHANT)+ request.MERCHANT;
            hashString += Helper.GetLengthAsByte(request.REFNO) +request.REFNO;
            hashString += Helper.GetLengthAsByte(request.TIMESTAMP) + request.TIMESTAMP;
            HASHED_CONTENT = Helper.CreateSha256Hash(hashString, secretKey);
            return HASHED_CONTENT;

        }
    }
}
