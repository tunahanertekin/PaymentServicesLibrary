using PaymentServicesLibrary.Servisler.PayU.Modeller.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Specialized;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller
{
    public class HttpCaller
    {
        public static HttpCaller Create()
        {
            return new HttpCaller();
        }
        public T PostData <T>(string postUrl,string hashstring)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //Make the request data ready
            Byte[] req = Encoding.GetEncoding("UTF-8").GetBytes(hashstring);
            // Send the data and get the response
            Byte[] res = wc.UploadData(postUrl, "POST", req);
            Char[] returnValue = Encoding.GetEncoding("UTF-8").GetChars(res);
            var responseData = new string(returnValue);
            return XmlBuilder.DeserializeObject<T>(responseData);
        }


        public static string PostDataArrayReturnString(string postUrl,NameValueCollection nameValueCollection)
        {
            WebClient myWebClient = new WebClient();
            byte[] responseArray = myWebClient.UploadValues(postUrl, nameValueCollection);
            Char[] returnValue = Encoding.GetEncoding("UTF-8").GetChars(responseArray);
            var responseData = new string(returnValue);
            return responseData;

        }


        public static string PostDataReturnString(string postUrl, string hashstring)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //Make the request data ready
            Byte[] req = Encoding.GetEncoding("UTF-8").GetBytes(hashstring);


            // Send the data and get the response
            Byte[] res = wc.UploadData(postUrl, "POST", req);
            Char[] returnValue = Encoding.GetEncoding("UTF-8").GetChars(res);
            var responseData = new string(returnValue);
            return responseData;

        }


        public static string GetDataToUrl(string url)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync(url).Result;
            var content = httpResponseMessage.Content;
            string responseString = content.ReadAsStringAsync().Result;
            return responseString;
        }
    }
}
